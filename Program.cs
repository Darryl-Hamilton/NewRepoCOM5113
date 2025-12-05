// COM 5113 Sample Code - Nick Mitchell 2025
using System;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;

namespace PathFinderAssessment
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool runAgain = false;

            do

            {
                Console.Clear();

                Console.Write("Enter map name (xxx for xxxMap.txt): ");
                string mapName = Console.ReadLine();
                string fileName = mapName + "Map.txt";

                if (!File.Exists(fileName))
                {
                    Console.WriteLine("Map file not found.");
                    Console.ReadKey();
                    continue;
                }

                int[,] map;
                Coord start;
                Coord goal;

                // ---- load the map ----
                using (StreamReader reader = new StreamReader(fileName))
                {
                    string[] dims = reader.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    int rows = int.Parse(dims[0]);
                    int cols = int.Parse(dims[1]);

                    string[] startData = reader.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    start = new Coord(int.Parse(startData[0]), int.Parse(startData[1]));

                    string[] goalData = reader.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    goal = new Coord(int.Parse(goalData[0]), int.Parse(goalData[1]));

                    map = new int[rows, cols];

                    for (int r = 0; r < rows; r++)
                    {
                        string[] rowData = reader.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                        for (int c = 0; c < cols; c++)
                        {
                            map[r, c] = int.Parse(rowData[c]);
                        }
                    }
                }

                // ---- choose algorithm (example) ----
                Console.WriteLine("Choose search algorithm:");
                Console.WriteLine("1 - Breadth First");
                Console.WriteLine("2 - Depth First");
                Console.WriteLine("3 - Hill Climbing");

                string choice = Console.ReadLine();

                Algorithm chosenAlgorithm = Algorithm.BreadthFirst;
                if (choice == "2") chosenAlgorithm = Algorithm.DepthFirst;
                else if (choice == "3") chosenAlgorithm = Algorithm.HillClimbing;

                // ⛔ CHECK IF START IS BLOCKED BEFORE SEARCH
                if (IsStartBlocked(map, start))
                {
                    Console.WriteLine("\nUnable to move");
                    Console.WriteLine("The start position is completely surrounded by walls.");
                    Console.ReadKey();
                    continue;
                }

                PathFinderInterface myPathFinder = PathFinderFactory.NewPathFinder(chosenAlgorithm);
                LinkedList<Coord> path = new LinkedList<Coord>();

                bool found = myPathFinder.FindPath(map, start, goal, ref path);

               

                if (found)
                {
                    SavePathToFile(mapName, chosenAlgorithm, path);
                    DisplayMapWithPath(map, start, goal, mapName, chosenAlgorithm);
                }
                else
                {
                    Console.WriteLine("\nNo path found.");
                }

                Console.WriteLine("\nPress any key to exit...");
                Console.ReadKey();




                static LinkedList<Coord> CopyPath(LinkedList<Coord> original)
                {
                    LinkedList<Coord> copy = new LinkedList<Coord>();
                    LinkedList<Coord> temp = new LinkedList<Coord>();

                    // Move to temp and copy
                    while (!original.IsEmpty())
                    {
                        Coord p;
                        original.PopFront(out p);

                        temp.PushBack(p);
                        copy.PushBack(p);
                    }

                    // Restore original
                    while (!temp.IsEmpty())
                    {
                        Coord p;
                        temp.PopFront(out p);
                        original.PushBack(p);
                    }

                    return copy;
                }




                static bool IsStartBlocked(int[,] map, Coord start)
                {
                    int r = start.Row;
                    int c = start.Col;

                    int rows = map.GetLength(0);
                    int cols = map.GetLength(1);

                    Coord[] neighbours =
                    {
                    new Coord(r - 1, c), // North
                    new Coord(r, c + 1), // East
                    new Coord(r + 1, c), // South
                    new Coord(r, c - 1)  // West
                };

                    foreach (var n in neighbours)
                    {
                        if (n.Row < 0 || n.Col < 0 ||
                            n.Row >= rows || n.Col >= cols)
                            continue;

                        if (map[n.Row, n.Col] != 0)
                            return false;
                    }


                    return true;
                }


                static void DisplayMapWithPath(int[,] map, Coord start, Coord goal, string mapName, Algorithm algorithm)
                {
                    int rows = map.GetLength(0);
                    int cols = map.GetLength(1);

                    string fileName = mapName + "Path_" + algorithm.ToString() + ".txt";
                    List<Coord> pathCoords = new List<Coord>();

                   
                    using (StreamReader reader = new StreamReader(fileName))
                    {
                        while (!reader.EndOfStream)
                        {
                            string line = reader.ReadLine().Trim();
                            if (line.Length == 0) continue;

                            string[] parts = line.Split(' ');
                            int r = int.Parse(parts[0]);
                            int c = int.Parse(parts[1]);
                            pathCoords.Add(new Coord(r, c));
                        }
                    }

                    Console.WriteLine("\nMAP DISPLAY:\n");

                    for (int r = 0; r < rows; r++)
                    {
                        for (int c = 0; c < cols; c++)
                        {
                            Coord current = new Coord(r, c);
                            bool isPath = false;

                            foreach (var p in pathCoords)
                            {
                                if (p.Row == r && p.Col == c)
                                {
                                    isPath = true;
                                    break;
                                }
                            }

                            if (current.Row == start.Row && current.Col == start.Col)
                                Console.Write("S ");
                            else if (current.Row == goal.Row && current.Col == goal.Col)
                                Console.Write("G ");
                            else if (isPath)
                                Console.Write("* ");
                            else
                            {
                                switch (map[r, c])
                                {
                                    case 0: Console.Write("# "); break;
                                    case 1: Console.Write(". "); break;
                                    case 2: Console.Write("T "); break;
                                    case 3: Console.Write("~ "); break;
                                }
                            }
                        }
                        Console.WriteLine();
                    }
                }





                static void SavePathToFile(string mapName, Algorithm algorithm, LinkedList<Coord> path)
                {
                    string algorithmName = algorithm.ToString();
                    string fileName = mapName + "Path_" + algorithmName + ".txt";

                    using (StreamWriter writer = new StreamWriter(fileName))
                    {
                        LinkedList<Coord> safePath = CopyPath(path);

                        List<Coord> steps = new List<Coord>();

                        while (!safePath.IsEmpty())
                        {
                            Coord p;
                            safePath.PopFront(out p);
                            steps.Add(p);
                        }

                        foreach (var step in steps)
                        {
                            writer.WriteLine($"{step.Row} {step.Col}");
                        }
                    }

                    Console.WriteLine($"\nPath written to file: {fileName}");
                }
                
                Console.WriteLine("\nRun another map? (y/n): ");
                string answer = Console.ReadLine().ToUpper();
                runAgain = (answer == "Y");
            } while (runAgain);


        }

    }
}