using System;
using System.Collections.Generic;
using System.Linq;

namespace Assignment1 {
    class dfs
    {
        static string[,]    world;
        static int          nodes = 0;
        static bool         StartGoalExists = false;    //Check if both Start and Goal positions exist
        static bool         goalReached = false;          //Check if goal has been reached
        static List<string> directions = new List<string>();

        //Position data. Goal and Current position also contains list of existing directions
        //static Tuple<int, int>                      sPosition;
        static Tuple<List<string>, Tuple<int, int>> gPosition;
        static Tuple<List<string>, Tuple<int, int>> currentPosition;

        //List of adjacent positions, unvisited and visited positions
        static List<Tuple<List<string>, Tuple<int, int>>>   adjacents = new List<Tuple<List<string>, Tuple<int, int>>>();
        static List<Tuple<List<string>, Tuple<int, int>>>   notVisited = new List<Tuple<List<string>, Tuple<int, int>>>();
        static List<Tuple<int, int>>                        visited = new List<Tuple<int, int>>();

        public static string beginSearch(string [,] pWorld)
        {
            List<string> outcome = new List<string>();
            world = pWorld;
            
            findStartEnd();
            if (StartGoalExists)
            {
                while (notVisited.Count != 0 && !goalReached)
                {
                    nodes++;                //Increment node count
                    visitPosition();        //Take first notVisited item, put into visited
                    findValidAdjacents();   //Create list of adjacent positions.
                    addAdjacentsToList();   //Add positions that are not in the visited list to the start of notVisited
                }
            }

            outcome.Add(nodes.ToString());
            outcome.Add("\n");

            if (!goalReached)
            {
                outcome.Add("No solution found.");
            }
            else
            {
                outcome.AddRange(gPosition.Item1);
            }

            return string.Join(" ", outcome);
        }

        static void findStartEnd()
        {
            bool foundStart = false;
            bool foundGoal = false;

            for (int y = 0; y < world.GetLength(0); y++)
            {
                for (int x = 0; x < world.GetLength(1); x++)
                {
                    if (world[y, x] == "S")
                    {
                        notVisited.Add(Tuple.Create(directions, Tuple.Create(x, y)));
                        foundStart = true;
                    }
                    else if (world[y, x] == "G")
                    {
                        gPosition = Tuple.Create(directions, Tuple.Create(x, y));
                        foundGoal = true;
                    }
                }
            }

            StartGoalExists = foundStart && foundGoal;
        }

        static void visitPosition()
        {
            //Console.WriteLine($"\n\tFunction '{System.Reflection.MethodBase.GetCurrentMethod().Name}'");
            currentPosition = notVisited[0];
            notVisited.RemoveAt(0);
            visited.Add(currentPosition.Item2);
            //world[cPosition.Item2.Item2, cPosition.Item2.Item1] = "-";
            if (world[currentPosition.Item2.Item2, currentPosition.Item2.Item1] != "S") //Keep 'Start' position for visual reference
            {
                world[currentPosition.Item2.Item2, currentPosition.Item2.Item1] = "-";
            }
        }

        //This is checked in reverse due to the backwards nature of insertion into 
        static void findValidAdjacents()
        {
            //Console.WriteLine($"\n\tFunction '{System.Reflection.MethodBase.GetCurrentMethod().Name}'");
            //Console.WriteLine($"Checking adjacents around '{cPosition.Item2.Item1}, {cPosition.Item2.Item2}'");
            adjacents.Clear();
            //Right = x+1
            if (currentPosition.Item2.Item1 + 1 < world.GetLength(1))
            {
                checkAdjacentValue("right", currentPosition.Item2.Item1 + 1, currentPosition.Item2.Item2);
            }
            //Down = y+1
            if (currentPosition.Item2.Item2 + 1 < world.GetLength(0))
            {
                checkAdjacentValue("down", currentPosition.Item2.Item1, currentPosition.Item2.Item2 + 1);
            }
            //Left = x-1
            if (currentPosition.Item2.Item1 - 1 >= 0)
            {
                checkAdjacentValue("left", currentPosition.Item2.Item1 - 1, currentPosition.Item2.Item2);
            }
            //Up = y-1
            if (currentPosition.Item2.Item2 - 1 >= 0)
            {
                checkAdjacentValue("up", currentPosition.Item2.Item1, currentPosition.Item2.Item2 - 1);
            }
        }

        static void checkAdjacentValue(string nextDir, int xCheck, int yCheck)
        {
            //Console.Write($"Checking {nextDir} position ({xCheck}, {yCheck})...");
            directions = new List<string>(currentPosition.Item1.Count);
            if (currentPosition.Item1.Count > 0)
            {
                foreach (var pos in currentPosition.Item1)
                {
                    directions.Add(pos);
                }
            }

            directions.Add(nextDir);

            if (world[yCheck, xCheck] == "G")
            {
                //Console.WriteLine(" - Goal found!");
                gPosition = Tuple.Create(directions, Tuple.Create(xCheck, yCheck));
                goalReached = true;
            }
            else if (world[yCheck, xCheck] == " " || world[yCheck, xCheck] == "N")
            {
                //Console.WriteLine(" - Empty or N");
                adjacents.Add(Tuple.Create(directions, Tuple.Create(xCheck, yCheck)));
            }
        }

        static void addAdjacentsToList()
        {
            //Console.WriteLine($"\n\tFunction '{System.Reflection.MethodBase.GetCurrentMethod().Name}'");
            foreach (var adj in adjacents)
            {
                bool exists = false;
                foreach (var vis in visited)
                {
                    if (adj.Item2.Item1 == vis.Item1 && adj.Item2.Item2 == vis.Item2)
                    {
                        //Console.WriteLine($"'{adj.Item1}, {adj.Item2}' has already been visited.");
                        exists = true;
                    }
                }
                if (!exists)
                {
                    //If the position already exists in the notVisited list, it needs to be removed first
                    for (int i = 0; i < notVisited.Count(); i++)
                    {
                        if (notVisited[i].Item2.Item1 == adj.Item2.Item1 && notVisited[i].Item2.Item2 == adj.Item2.Item2)
                        {
                            notVisited.RemoveAt(i);
                        }
                    }
                    notVisited.Insert(0, adj);
                    //Console.WriteLine($"'{adj.Item2}' has been added to list.");
                    world[adj.Item2.Item2, adj.Item2.Item1] = "N";
                }
            }
            worldGrid.printGrid(world);
        }
    }
}
