using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace TextAdventure
{
    class MapReader : Helpers
    {
        public static Room Parse(Stream stream)
        {
            var settings = new XmlReaderSettings
            {
                IgnoreComments = true,
                IgnoreWhitespace = true
            };

            using (var xmlReader = XmlReader.Create(stream, settings))
            {
                var mapReader = new MapReader(xmlReader);

                try
                {
                    mapReader.Parse();

                    return mapReader.m_startRoom;
                }
                catch (Exception e)
                {
                    string message = e.Message;

                    var lineInfo = e as IXmlLineInfo;

                    if (lineInfo == null)
                    {
                        lineInfo = xmlReader as IXmlLineInfo;
                    }

                    if (lineInfo != null)
                    {
                        message = $"Line {lineInfo.LineNumber}, Position = {lineInfo.LinePosition}: {message}";
                    }

                    Console.Error.WriteLine("Error: {0}", message);

                    return null;
                }
            }
        }

        private MapReader(XmlReader reader)
        {
            m_reader = reader;
        }

        private void Fail(string message)
        {
            throw new ApplicationException(message);
        }

        private void Parse()
        {
            if (!m_reader.IsStartElement("GameMap"))
                Fail("GameMap element expected.");

            if (m_reader.IsEmptyElement)
                Fail("GameMap element is empty.");

            m_reader.Read();

            while (m_reader.IsStartElement())
            {
                switch (m_reader.LocalName)
                {
                    case "Room":
                        ParseRoom();
                        break;

                    case "Key":
                        ParseKey();
                        break;

                    case "Link":
                        ParseLink();
                        break;

                    default:
                        Fail("Room or Link element expected.");
                        break;
                }
            }

            m_reader.ReadEndElement();

            if (m_startRoom == null)
                Fail("No Room elements specified.");
        }

        private void ParseRoom()
        {
            string id = GetRequiredAttribute("ID");
            string name = GetRequiredAttribute("Name");

            if (m_rooms.ContainsKey(id))
            {
                Fail($"Duplicate room ID: {id}.");
            }

            var room = new Room(name);

            m_rooms.Add(id, room);

            if (m_startRoom == null)
            {
                m_startRoom = room;
            }

            if (m_reader.IsEmptyElement)
            {
                m_reader.Read();
            }
            else
            {
                m_reader.Read();
                ParseDescription(room);
                ParseItems(room.Items);
                m_reader.ReadEndElement();
            }
        }

        private void ParseKey()
        {
            var key = GetItem(GetRequiredAttribute("IDRef"));
            var unlocks = GetItem(GetRequiredAttribute("Unlocks"));

            var openable = unlocks as IOpenable;
            if (openable == null)
            {
                Fail($"{unlocks.Name} is not a container.");
            }
            else
            {
                openable.Key = key;
            }

            m_reader.Skip();
        }

        private void ParseItems(IList<Item> container)
        {
            while (m_reader.IsStartElement())
            {
                switch (m_reader.LocalName)
                {
                    case "Item":
                        container.Add(ParseItem());
                        break;

                    case "Container":
                        container.Add(ParseContainer());
                        break;

                    default:
                        return;
                }
            }
        }

        private void ParseItemAttributes(Item item)
        {
            string id = GetRequiredAttribute("ID");

            if (m_items.ContainsKey(id))
            {
                Fail($"Duplicate item ID: {id}.");
            }

            item.Name = GetRequiredAttribute("Name");
            item.IsFixed = GetOptionalBool("IsFixed", false);

            m_items.Add(id, item);
        }

        private Item ParseItem()
        {
            var item = new SimpleItem();

            ParseItemAttributes(item);

            if (m_reader.IsEmptyElement)
            {
                m_reader.Read();
            }
            else
            {
                m_reader.Read();
                ParseDescription(item);
                m_reader.ReadEndElement();
            }

            return item;
        }

        private Item ParseContainer()
        {
            var item = new ContainerItem();

            ParseItemAttributes(item);
            ParseOpenableAttributes(item);

            if (m_reader.IsEmptyElement)
            {
                m_reader.Read();
            }
            else
            {
                m_reader.Read();
                ParseDescription(item);
                ParseItems(item.Items);
                m_reader.ReadEndElement();
            }

            return item;
        }

        private void ParseDescription(IDescribable item)
        {
            while (m_reader.IsStartElement("p"))
            {
                var condition = Description.Condition.None;

                string s = m_reader["if"];
                if (!string.IsNullOrEmpty(s))
                {
                    if (!Description.ParseCondition(s, out condition))
                    {
                        Fail($"Invalid condition: {s}.");
                    }
                }

                var text = m_reader.ReadElementContentAsString();

                if (item.Description == null)
                {
                    item.Description = new Description();
                }

                item.Description.Add(condition, text);
            }
        }

        private void ParseLink()
        {
            Room from = GetRoom(GetRequiredAttribute("From"));
            Room to = GetRoom(GetRequiredAttribute("To"));

            Direction dir = ParseDirection(GetRequiredAttribute("Direction"));
            if (dir == Direction.None)
            {
                Fail("Invalid Direction.");
            }

            Door door = null;

            if (m_reader.IsEmptyElement)
            {
                m_reader.Read();
            }
            else
            {
                m_reader.Read();
                if (m_reader.IsStartElement("Door"))
                {
                    door = ParseDoor();
                }
                m_reader.ReadEndElement();
            }

            LinkRooms(from, to, dir, door);
        }

        private void ParseOpenableAttributes(IOpenable openable)
        {
            openable.IsLocked = GetOptionalBool("IsLocked", false);
            openable.IsOpen = GetOptionalBool("IsOpen", false);
        }

        private Door ParseDoor()
        {
            var door = new Door();

            ParseOpenableAttributes(door);
            door.Key = GetItem(m_reader["Key"]);

            m_reader.Skip();

            return door;
        }

        private Room GetRoom(string id)
        {
            if (id == null)
                return null;

            Room room;
            if (!m_rooms.TryGetValue(id, out room))
            {
                Fail($"The room '{id}' is not defined.");
            }
            return room;
        }

        private Item GetItem(string id)
        {
            if (id == null)
                return null;

            Item item;
            if (!m_items.TryGetValue(id, out item))
            {
                Fail($"The item '{id}' is not defined.");
            }
            return item;
        }

        private bool GetOptionalBool(string name, bool defaultValue)
        {
            string value = m_reader[name];
            if (string.IsNullOrEmpty(value))
                return defaultValue;

            switch (value.ToLowerInvariant())
            {
                case "true": return true;
                case "false": return false;
            }

            Fail($"Expected 'true' or 'false' for {name}.");
            return false;
        }

        private string GetRequiredAttribute(string name)
        {
            var value = m_reader[name];
            if (string.IsNullOrEmpty(value))
            {
                Fail($"Missing required {name} attribute.");
            }
            return value;
        }

        XmlReader m_reader;
        Dictionary<string, Room> m_rooms = new Dictionary<string, Room>();
        Dictionary<string, Item> m_items = new Dictionary<string, Item>();
        Room m_startRoom;
    }
}
