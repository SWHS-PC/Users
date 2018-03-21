using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Life
{
    class Program
    {
        public static int w = Console.LargestWindowWidth / 2, h = Console.LargestWindowHeight / 2;
        public static char cell = (char)183;
        public static bool[,] Map;
        
        public bool this[int x, int y]
        {
            get { return Map[x, y]; }
            set { Map[x, y] = value; }
        }

        static void Main(string[] args)
        {
            Program link = new Program();
            Console.SetWindowSize(w, h);
            Console.SetWindowPosition(0, 0);
            Console.CursorVisible = false;
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Clear();
            Map = new bool[w, h];


            while (true)
            {
                link.Draw();
                Console.Read();


                if (Ones < 2 || (Ones > 3 && Neighbours[4] == 1))
                {
                    Neighbours[4] = 0;
                }
                if (Ones == 3 || Ones == 4 || (Neighbours[4] == 0 && Ones == 3))
                {
                    Neighbours[4] = 1;
                }
                bool currentValue = Map[x, y];
                return this.world[x, y] = !currentValue;
            }
        }

       
        public void Draw()
        {
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    Console.Write(this[x, y] ? "*" : " ");
                }
            }  
        }
    }
}


//public class LifeSimulation
//{
//    private bool[,] world;
//    private bool[,] nextGeneration;
//    private Task processTask;

//    public LifeSimulation(int size)
//    {
//        if (size < 0) throw new ArgumentOutOfRangeException("Size must be greater than zero");
//        this.Size = size;
//        world = new bool[size, size];
//        nextGeneration = new bool[size, size];
//    }

//    public int Size { get; private set; }
//    public int Generation { get; private set; }

//    public Action<bool[,]> NextGenerationCompleted;

//    public bool this[int x, int y]
//    {
//        get { return this.world[x, y]; }
//        set { this.world[x, y] = value; }
//    }

//    public bool ToggleCell(int x, int y)
//    {
//        bool currentValue = this.world[x, y];
//        return this.world[x, y] = !currentValue;
//    }

//    public void Update()
//    {
//        if (this.processTask != null && this.processTask.IsCompleted)
//        {
//            // when a generation has completed
//            // now flip the back buffer so we can start processing on the next generation
//            var flip = this.nextGeneration;
//            this.nextGeneration = this.world;
//            this.world = flip;
//            Generation++;

//            // begin the next generation's processing asynchronously
//            this.processTask = this.ProcessGeneration();

//            if (NextGenerationCompleted != null) NextGenerationCompleted(this.world);
//        }
//    }

//    public void BeginGeneration()
//    {
//        if (this.processTask == null || (this.processTask != null && this.processTask.IsCompleted))
//        {
//            // only begin the generation if the previous process was completed
//            this.processTask = this.ProcessGeneration();
//        }
//    }

//    public void Wait()
//    {
//        if (this.processTask != null)
//        {
//            this.processTask.Wait();
//        }
//    }

//    private Task ProcessGeneration()
//    {
//        return Task.Factory.StartNew(() =>
//        {
//            Parallel.For(0, Size, x =>
//            {
//                Parallel.For(0, Size, y =>
//                {
//                    int numberOfNeighbors = IsNeighborAlive(world, Size, x, y, -1, 0)
//                        + IsNeighborAlive(world, Size, x, y, -1, 1)
//                        + IsNeighborAlive(world, Size, x, y, 0, 1)
//                        + IsNeighborAlive(world, Size, x, y, 1, 1)
//                        + IsNeighborAlive(world, Size, x, y, 1, 0)
//                        + IsNeighborAlive(world, Size, x, y, 1, -1)
//                        + IsNeighborAlive(world, Size, x, y, 0, -1)
//                        + IsNeighborAlive(world, Size, x, y, -1, -1);

//                    bool shouldLive = false;
//                    bool isAlive = world[x, y];

//                    if (isAlive && (numberOfNeighbors == 2 || numberOfNeighbors == 3))
//                    {
//                        shouldLive = true;
//                    }
//                    else if (!isAlive && numberOfNeighbors == 3) // zombification
//                    {
//                        shouldLive = true;
//                    }

//                    nextGeneration[x, y] = shouldLive;

//                });
//            });
//        });
//    }

//    private static int IsNeighborAlive(bool[,] world, int size, int x, int y, int offsetx, int offsety)
//    {
//        int result = 0;

//        int proposedOffsetX = x + offsetx;
//        int proposedOffsetY = y + offsety;
//        bool outOfBounds = proposedOffsetX < 0 || proposedOffsetX >= size | proposedOffsetY < 0 || proposedOffsetY >= size;
//        if (!outOfBounds)
//        {
//            result = world[x + offsetx, y + offsety] ? 1 : 0;
//        }
//        return result;
//    }
//}

//class Program
//{
//    public static int w = Console.LargestWindowWidth / 2, h = Console.LargestWindowHeight / 2;

//    static void Main(string[] args)
//    {
//        LifeSimulation sim = new LifeSimulation(50);

//        Console.SetWindowSize(w, h);
//        Console.SetWindowPosition(0, 0);
//        Console.CursorVisible = false;
//        Console.BackgroundColor = ConsoleColor.White;
//        Console.ForegroundColor = ConsoleColor.Black;
//        Console.Clear();
//        // initialize with a blinker
//        sim.ToggleCell(25, 25);
//        sim.ToggleCell(25, 26);
//        sim.ToggleCell(25, 27);

//        sim.BeginGeneration();
//        sim.Wait();
//        OutputBoard(sim);
//        sim.Update();
//        sim.Wait();
//        OutputBoard(sim);




//        Console.ReadKey();
//    }

//    private static void OutputBoard(LifeSimulation sim)
//    {
//        var line = new String('-', sim.Size);
//        Console.WriteLine(line);

//        for (int y = 0; y < sim.Size; y++)
//        {
//            for (int x = 0; x < sim.Size; x++)
//            {
//                Console.Write(sim[x, y] ? "1" : "0");
//            }

//            Console.WriteLine();
//        }
//    }
//}