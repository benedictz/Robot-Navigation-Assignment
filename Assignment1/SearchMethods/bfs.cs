using System;
using System.Collections.Generic;
using System.Linq;

namespace Assignment1 {
    class bfs
    {
        static string[,]    world;
        static int          nodes = 0;
        static bool         StartGoalExists = false;    //Check if both Start and Goal positions exist
        static bool         goalReached = false;        //Check if Goal has been reached
        static List<string> directions = new List<string>();

        //<x, y> data. Goal and Current position also contains list of existing directions
        //static Tuple<int, int>                            sPosition;
        static Tuple<List<string>, Tuple<int, int>>         gPosition;
        static List<Tuple<List<string>, Tuple<int, int>>>   gPositions = new List<Tuple<List<string>, Tuple<int, int>>>();
        static Tuple<List<string>, Tuple<int, int>>         currentPosition;
        
        //List of adjacent positions, unvisited and visited positions
        static List<Tuple<List<string>, Tuple<int, int>>>   adjacents = new List<Tuple<List<string>, Tuple<int, int>>>();
        static List<Tuple<List<string>, Tuple<int, int>>>   notVisited = new List<Tuple<List<string>, Tuple<int, int>>>();
        static List<Tuple<int, int>>                        visited = new List<Tuple<int, int>>();

        public static string beginSearch(string[,] pWorld)
        {
            List<string> outcome = new List<string>();
            world = pWorld;
            
            findStartEnd();
            while (notVisited.Count != 0 && !goalReached && StartGoalExists)
            {
                nodes++;                //Increment node count
                visitPosition();        //Take first notVisited item, put into visited
                findValidAdjacents();   //Create list of adjacent positions.
                addAdjacentsToList();   //Add positions that are not in the visited list to the end of notVisited
            }

            outcome.Add(nodes.ToString());

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

        //Find the start and end positions
        static void findStartEnd()
        {
            //Console.WriteLine($"\n\tFunction '{System.Reflection.MethodBase.GetCurrentMethod().Name}'");
            bool foundStart = false;
            bool foundGoal = false;

            for (int y = 0; y < world.GetLength(0); y++)
            {
                for (int x = 0; x < world.GetLength(1); x++)
                {
                    if (world[y, x] == "S")
                    {
                        //Console.WriteLine($"Start found at '{x}, {y}'");
                        //sPosition = Tuple.Create(x, y);
                        notVisited.Add(Tuple.Create(directions, Tuple.Create(x, y)));
                        foundStart = true;
                    }
                    else if (world[y, x] == "G")
                    {
                        //Console.WriteLine($"End found at '{x}, {y}'");
                        //gPositions.Add(Tuple.Create(directions, Tuple.Create(x, y)));
                        foundGoal = true;
                    }
                }
            }

            StartGoalExists = foundStart && foundGoal;
        }

        //Visit the first position in the notVisited list, remove it and set as cPosition.
        //Place position into visited list. Denote 'visited' with a '-' in the world grid.
        static void visitPosition()
        {
            //Console.WriteLine($"\n\tFunction '{System.Reflection.MethodBase.GetCurrentMethod().Name}'");
            currentPosition = notVisited[0];
            notVisited.RemoveAt(0);
            visited.Add(currentPosition.Item2);
            world[currentPosition.Item2.Item2, currentPosition.Item2.Item1] = "-";
        }

        //Check the four directions around cPosition to check that it is within the world grid.
        static void findValidAdjacents()
        {
            //Console.WriteLine($"\n\tFunction '{System.Reflection.MethodBase.GetCurrentMethod().Name}'");
            //Console.WriteLine($"Checking adjacents around {currentPosition.Item2}");
            adjacents.Clear();
            //Up = y-1
            if (currentPosition.Item2.Item2 - 1 >= 0)
            {
                //Console.Write($"Up position '{currentPosition.Item2.Item1}, {currentPosition.Item2.Item2 - 1}'");
                checkAdjacentValue("up", currentPosition.Item2.Item1, currentPosition.Item2.Item2 - 1);
            }
            //Left = x-1
            if (currentPosition.Item2.Item1 - 1 >= 0)
            {
                //Console.Write($"Left position '{currentPosition.Item2.Item1 - 1}, {currentPosition.Item2.Item2}'");
                checkAdjacentValue("left", currentPosition.Item2.Item1 - 1, currentPosition.Item2.Item2);
            }
            //Down = y+1
            if (currentPosition.Item2.Item2 + 1 < world.GetLength(0))
            {
                //Console.Write($"Down position '{currentPosition.Item2.Item1}, {currentPosition.Item2.Item2 + 1}'");
                checkAdjacentValue("down", currentPosition.Item2.Item1, currentPosition.Item2.Item2 + 1);
            }
            //Right = x+1
            if (currentPosition.Item2.Item1 + 1 < world.GetLength(1))
            {
                //Console.Write($"Right position '{currentPosition.Item2.Item1 + 1}, {currentPosition.Item2.Item2}'");
                checkAdjacentValue("right", currentPosition.Item2.Item1 + 1, currentPosition.Item2.Item2);
            }
        }

        //Create new list of directions from existing cPosition.
        //Check current value in given world space. If goal, flag goalFound boolean.
        //Add next direction to the existing list of directions
        static void checkAdjacentValue(string nextDir, int item1, int item2)
        {
            directions = new List<string>(currentPosition.Item1.Count);
            if (currentPosition.Item1.Count > 0)
            {
                foreach (var pos in currentPosition.Item1)
                {
                    directions.Add(pos);
                }
            }

            directions.Add(nextDir);

            if (world[item2, item1] == "G")
            {
                //Console.WriteLine(" - Goal found!");
                gPosition = Tuple.Create(directions, Tuple.Create(item1, item2));
                goalReached = true;
            }
            else if (world[item2, item1] == " ")
            {
                //Console.WriteLine(" - Empty");
                adjacents.Add(Tuple.Create(directions, Tuple.Create(item1, item2)));
            }
        }

        //Check that the position hasn't already been visited by another node.
        //Ensure no duplicates exist in the notVisited list. Add position to the notVisited list.
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
                    notVisited.Add(adj);
                    Console.WriteLine($"'{adj.Item2}' has been added to list.");
                    world[adj.Item2.Item2, adj.Item2.Item1] = "N";
                }
            }
            worldGrid.printGrid(world);
        }
    }
}
