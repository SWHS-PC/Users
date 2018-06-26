using System;
using System.Collections.Generic;
using System.Xml;

namespace TextAdventure
{
    class MapReader : Helpers
    {
        public static Room Parse(XmlReader reader)
        {
            try
            {
                var mapReader = new MapReader(reader);

                mapReader.Parse();

                return mapReader.m_startRoom;
            }
            catch (Exception e)
            {
                string message = e.Message;

                var lineInfo = e as IXmlLineInfo;

                if (lineInfo == null)
                {
                    lineInfo = reader as IXmlLineInfo;
                }

                if (lineInfo != null)
                {
                    message = $"Line {lineInfo.LineNumber}, Position = {lineInfo.LinePosition}: {message}";
                }

                Console.Error.WriteLine("Error: {0}", message);

                return null;
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

            m_reader.Skip();
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

        private Door ParseDoor()
        {
            m_reader.Skip();
            return new Door();
        }

        private Room GetRoom(string id)
        {
            Room room;
            if (!m_rooms.TryGetValue(id, out room))
            {
                Fail($"Room {id} is not defined.");
            }
            return room;
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
        Room m_startRoom;
    }
}
