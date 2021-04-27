using System;
using System.Collections.Generic;
using System.Threading;

namespace Assignment1 {
    class worldGrid {
        static string[,] world;
        public static string searchType = "";

        public static void gridValues(int lineNumber, List<int> intNumbers)
        {
            if (lineNumber == 0)        //Create world boundaries
            {
                world = new string[intNumbers[0], intNumbers[1]];
                for (int x = 0; x < world.GetLength(0); x++)
                {
                    for (int y = 0; y < world.GetLength(1); y++)
                    {
                        world[x, y] = " ";
                    }
                }
                Console.WriteLine($"Size: [{world.GetLength(0)},{world.GetLength(1)}]");
            }
            else if (lineNumber == 1)   //Create Start position
            {
                if (intNumbers.Count == 2)
                {
                    world[intNumbers[1], intNumbers[0]] = "S";
                }
            }
            else if (lineNumber == 2)   //Create Goal location(s)
            {
                int counter = 0;
                int firstVal = 0;
                foreach (int i in intNumbers)
                {
                    counter++;
                    if (counter != 2)
                    {
                        firstVal = i;
                    } else
                    {   //goal value = firstval and new i
                        Console.WriteLine($"{firstVal}, {i}");
                        world[i, firstVal] = "G";
                        counter = 0;
                    }
                }
            }
            else    //Create obstructions
            {
                for (int y = intNumbers[1]; y < intNumbers[1] + intNumbers[3]; y++)
                {
                    for (int x = intNumbers[0]; x < intNumbers[0] + intNumbers[2]; x++)
                    {
                        world[y, x] = "\u2588";
                    }
                }
            }
        }

        public static string[,] getWorld()
        {
            return world;
        }

        //For updating the world grid. I suppose it could just be a completely new function name and not have any discernable difference in functionality.
        public static void printGrid(string[,] pWorld)
        {
            world = pWorld;
            printGrid();
        }

        public static void printGrid()
        {
            Console.Clear();
            Console.WriteLine(searchType);
            for (int x = 0; x < world.GetLength(0); x++)
            {
                Console.Write("| ");
                for (int y = 0; y < world.GetLength(1); y++)
                {
                    switch (world[x, y])
                    {
                        case "S":
                            printColour(world[x, y], ConsoleColor.Red);
                            break;
                        case "G":
                            printColour(world[x, y], ConsoleColor.Green);
                            break;
                        case "-":
                            printColour(world[x, y], ConsoleColor.DarkGray);
                            break;
                        case "N":
                            printColour(world[x, y], ConsoleColor.Yellow);
                            break;
                        default:
                            Console.Write(world[x, y]);
                            break;
                    }
                    Console.Write(" | ");
                }
                Console.Write("\n");
            }
            Thread.Sleep(25);
        }

        static void printColour(string value, ConsoleColor colour)
        {
            Console.ForegroundColor = colour;
            Console.Write(value);
            Console.ResetColor();
        }

        
    }
}
