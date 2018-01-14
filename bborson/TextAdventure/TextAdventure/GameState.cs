using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdventure
{
    class GameState
    {
        public void Run()
        {
            Program.WriteParagraph("You have arrived at the edge of a small seaside town. A sign welcomes you to Downdale. Main Street forges ahead, lined with shops.");
            Console.WriteLine();
            Program.WriteParagraph("Press n, s, e, or w to move North, South, East, or West, 'take [item name]' to pick up an item, 'use [item name]' to use an item, and i to list the contents of your inventory.");
            Console.WriteLine();
            m_currentRoom.Describe();

            while (!m_isGameOver)
            {
                Console.WriteLine();
                Console.Write("---------------------------------------------\nWhat next, ye adventurer? ");

                ProcessCommand(Console.ReadLine().ToLower().Split());
            }
        }
        void TryGo(MapLink link, string direction)
        {
            if (link == null)
            {
                Console.WriteLine("You cannot go that way.");
            }
            else if (link.To == null)
            {
                m_currentRoom.DescribeLink(link, direction);
                Console.WriteLine("You cannot go that way.");
            }
            else if (link.Door != null && link.Door.IsLocked)
            {
                Console.WriteLine($"The {link.Door.Name} is locked and blocks your passage.");
            }
            else
            {
                m_currentRoom = link.To;
                m_currentRoom.Describe();
                if (m_currentRoom == m_victoryRoom)
                {
                    m_isGameOver = true;
                    Program.WriteParagraph("Stumbling through the ruin, along dusty hallways and up decaying stairs, you see a peculiar sight: a narrow entrance, what evidently used to be a secret passage, now fallen in to reveal a small chamber. The place is bare but for an enormous sea chest. The lock has rusted and wood rotted, so you are able to see what is inside. All the stories are true! A fabulous treasure sparkles before your eyes. Your happily ever after, adventurer.");
                    Console.WriteLine("Press enter to exit.");
                    Console.ReadLine();
                }
            }
        }
        void ProcessCommand(string[] words)
        {
            Console.WriteLine();
            if (words.Length == 0)
            {
                Console.WriteLine("Please type a valid command.");
                return;
            }
            switch (words[0])
            {
                case "n":
                    {
                        TryGo(m_currentRoom.North, "North");
                        break;
                    }
                case "s":
                    {
                        TryGo(m_currentRoom.South, "South");
                        break;
                    }
                case "e":
                    {
                        TryGo(m_currentRoom.East, "East");
                        break;
                    }
                case "w":
                    {
                        TryGo(m_currentRoom.West, "West");
                        break;
                    }
                case "take":
                    {
                        if (words.Length == 2)
                        {
                            var item = FindItem(m_currentRoom.Items, words[1]);
                            if (item != null)
                            {
                                m_currentRoom.Items.Remove(item);
                                m_inventory.Add(item);
                                Console.WriteLine($"You take the {item.Name}.");
                            }
                            else
                            {
                                Console.WriteLine($"There is no {words[1]} here.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Please specify a valid item.");
                        }
                        break;
                    }
                case "use":
                    {
                        if (words.Length == 2)
                        {
                            var item = FindItem(m_inventory, words[1]);
                            if (item != null)
                            {
                                item.Use(ref m_currentRoom);
                            }
                            else
                            {
                                Console.WriteLine($"You don't have a {words[1]}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Please specify a valid item.");
                        }
                        break;
                    }
                case "i":
                    {
                        foreach (var item in m_inventory)
                        {
                            Console.WriteLine(item.Name);
                        }
                        break;
                    }
                default:
                    {
                        Console.WriteLine("Whaaaat? Don't be a cotton-headed ninnymuggins.");
                        break;
                    }
            }
        }

        Item FindItem(IList<Item> list, string name)
        {
            foreach (var item in list)
            {
                if (item.Name == name)
                {
                    return item;
                }
            }
            return null;
        }

        bool m_isGameOver;

        Room m_currentRoom;
        Room m_victoryRoom;
        List<Item> m_inventory = new List<Item>();

        public GameState()
        {
            var park = new Room("Smith Park") {Description = "A plot of withered grass, an old windswept tree, and a worn statue of a fisherman, bordered on all sides by Main Street, make up the entirety of the park before you. It is deserted, and doesn't appear to be maintained." };
            var cemetery = new Room("Downdale Cemetery") {Description = "Ancient headstones, covered in moss and ivy, pop out of the cold ground sporadically, jumbled together in mysterious configurations. What were once small decorative trees have merged with the forest that has overcome the fence on all sides." }; 
            var barber = new Room("Hal's Barbershop") { Description = "A moustached barber is shaving a client's bald head, removing what you don't know."};
            var postoffice = new Room("Downdale Post Office") { Description = "A small empty room constitutes the local mailing station. It doesn't seem like it is ever used; it is certainly empty now."};
            var pharmacy = new Room("Druggo's Pharmacy") { Description = "A gaunt man in a white apron stands behind the counter of a drug store strangely large for the small town it is located in. Long rows of pills and potions are starkly illuminated by the harsh lighting."};
            var butcher = new Room("Johnny's Meats") { Description = "As you enter, you notice the bloody carcasses displayed on hooks in the front window. A doorbell tinkles. This shop appears not to be serviced by electricity; Johnny informs you that he stores his cuts the old way: packed in ice in an underground cellar."};
            var genstore = new Room("the General Store") { Description = "This store sells everything. It has several customers, the only shop to be attracting such a crowd. Kindly greetings and pleasant decor create a nice atmosphere. You feel welcomed here, a stark contrast to the lonely desolation all around."};
            var forest = new Room("the Old Forest") { Description = "A section of the cemetery's fencing has collapsed under the strain of weather and mossy growth. The forest beyond is close, the air heavy and humid, even in the frosty sea breeze. Forests this dense are a rare sight these days. This one seems to be more alive than you There are no paths, and struggling through the undergrowth, you know that without a compass you would never find your way."};
            var glen = new Room("a Secret Glen") { Description = "In the depths of the forest now, you come across a little steep-sided valley. There are signs that this place was once inhabited, but it is all overgrown now. A thinness in a section of forest to the east suggests that Main Street once came this way. Moving forward, you see what appears to be a structure a little ways off, mostly obscured by the curve of the valley walls."};
            var ancientmanor = new Room("an Ancient Manor");
            var pier = new Room("the Pier") { Description = "The pilings stretch out into the surf, but no boats are moored. This harbor fell into disuse long ago. A grizzled old mariner stands near you, looking out at the gray waves and calling gulls."};
            var beach = new Room("the Beach") { Description = "A narrow strip of rocks and coarse sand constitutes Downdale's beach. A rocky outcrop blocks any view northward."};
            var cliffs = new Room("the Seaside Cliffs") { Description = "Everything is gray as you look out from the heights. The waves, the sky, even the town. You can see the land sloping down towards the pier, and off to the west a hazy forest."};
            var sroadside = new Room("South Main Street Roadside");
            var nroadside = new Room("North Main Street Roadside") { Description = "Main Street continues on ahead of you, soon becoming unpaved and rutted, barely discernible from the meadows to either side. It has not been used for a very long time. Following it, you round a bend and see that the road has been washed out by some storm. There is no going that way."};
            var townhall = new Room("Downdale Town Hall") { Description = "A shabby waiting room greets you as you step into the musty warmth. The old lady at the desk before you looks surprised to see a visitor and unsure what to do about it. You wonder if this place has any children. A brochure advertises the 'famously sublime' Downdale beach."};
            var gunstore = new Room("Ray's Guns") { Description = "This place appears to have gone out of business. The interior is bare, though, investigating, you see something glimmer in a back room."};
            var longdirtroad = new Room("a long dirt road") { Description = "The path stretches out before you over a meadow and wends between mossy trees, disappearing into an old growth forest in the distance."};

            m_currentRoom = sroadside;
            m_victoryRoom = ancientmanor;

            var gate = new Door { Name = "Rusty Iron Gate", IsLocked = true };
            var gundoor = new Door { Name = "Padlocked Door", IsLocked = true };

            var crowbar = new KeyItem { Name = "crowbar", Door = gate };
            var key = new KeyItem { Name = "key", Door = gundoor };

            var photo = new MessageItem { Name = "photograph", ActiveRoom = pier, Message = "You ask the ancient sea salt if he could name the individuals in an old photograph for you. The mariner looks with wonder at the black-and-white photo. 'My God! He exclaims. This is of the McCyntyre family! Back when the McCyntyres lived in these parts and operated their fleet, our village was rich and full of people. They lived up at the end of Main Street. It couldn't last of course. The McCyntyres gradually died out, and something strange happened out there on the sea. The last of the McCyntyres, Old Man Macintosh, came back one day with no fish, in the dead of night. Some say they saw him unload a cargo of treasure, but fools always talk for the attention it gets them. Macintosh, a man I knew in my youth mind you, acted mighty strange after that, and wouldn't say what had happened to him. Poor fool died soon after. Something had addled his brains, right enough. He's buried in the old cemetery, the one out west that filled up. Damn disgrace it is, letting it fall to shambles like it is. This town is sick, and dying. Go back where you came from, kid. I don't know where you got this photograph, but you'd best not worry about it any more. No one else does.'"};

            townhall.Items.Add(photo);
            gunstore.Items.Add(new TeleportItem { Name = "compass", Room1 = forest, Room2 = glen });
            park.Items.Add(key);
            genstore.Items.Add(crowbar);

            park.North = new MapLink
            {
                To = barber
            };
            barber.South = new MapLink { To = park };

            gunstore.South = new MapLink { To = townhall, Door = gundoor };
            townhall.North = new MapLink { To = gunstore, Door = gundoor };

            beach.South = new MapLink { To = pier };
            pier.North = new MapLink { To = beach };

            pier.South = new MapLink { To = cliffs };
            cliffs.North = new MapLink { To = pier };

            pier.West = new MapLink { To = townhall };
            townhall.East = new MapLink { To = pier };

            townhall.West = new MapLink { To = park };
            park.East = new MapLink { To = townhall };

            park.South = new MapLink { To = genstore };
            genstore.North = new MapLink { To = park };

            genstore.South = new MapLink { To = postoffice };
            postoffice.North = new MapLink { To = genstore };

            postoffice.South = new MapLink { To = sroadside };
            sroadside.North = new MapLink { To = postoffice };

            barber.North = new MapLink { To = pharmacy };
            pharmacy.South = new MapLink { To = barber };

            pharmacy.North = new MapLink { To = nroadside };
            nroadside.South = new MapLink { To = pharmacy };

            park.West = new MapLink { To = butcher };
            butcher.East = new MapLink { To = park };

            butcher.West = new MapLink { To = longdirtroad };
            longdirtroad.East = new MapLink { To = butcher };

            longdirtroad.West = new MapLink { To = cemetery, Door = gate };
            cemetery.East = new MapLink { To = longdirtroad, Door = gate};

            cemetery.North = new MapLink { To = forest };
            forest.South = new MapLink { To = cemetery };

            glen.North = new MapLink { To = ancientmanor };
        }
    }
}
