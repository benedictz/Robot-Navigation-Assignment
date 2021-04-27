using System;
using System.Collections.Generic;

namespace Assignment1 {
    class gbfs
    {
        static string[,]    world;
        static int          nodes = 0;
        static bool         StartGoalExists = false;    //Check if both Start and Goal positions exist
        static bool         goalReached = false;        //Check if goal has been reached
        static List<string> directions = new List<string>();

        //<x, y> data
        //static Tuple<int, int>                            sPosition;                                                      //Start position
        static Tuple<List<string>, Tuple<int, int>>         gPosition;                                                      //Used as final found goal
        static List<Tuple<List<string>, Tuple<int, int>>>   gPositions = new List<Tuple<List<string>, Tuple<int, int>>>();  //List of all goal positions

        //<heuristic, directions>, <x, y> currentPosition
        static Tuple<Tuple<int, List<string>>, Tuple<int, int>> currentPosition;

        //List of <heuristic, directions>, <x, y>
        static List<Tuple<Tuple<int, List<string>>, Tuple<int, int>>>   successors = new List<Tuple<Tuple<int, List<string>>, Tuple<int, int>>>();
        static List<Tuple<Tuple<int, List<string>>, Tuple<int, int>>>   openSet = new List<Tuple<Tuple<int, List<string>>, Tuple<int, int>>>();
        static List<Tuple<Tuple<int, List<string>>, Tuple<int, int>>>   closedSet = new List<Tuple<Tuple<int, List<string>>, Tuple<int, int>>>();

        public static string beginSearch(string[,] pWorld)
        {
            List<string> outcome = new List<string>();
            world = pWorld;

            findStartEnd();
            while (openSet.Count != 0 && !goalReached && StartGoalExists)
            {
                nodes++;                //Increment node count
                findLowestG();          //Visit first node in openSet with lowest distance to goal
                findValidNeighbours();  //Generate neighbours of node
                neighbourHeuristics();  //Generate Manhattan distance of each neighbour
            }

            outcome.Add(nodes.ToString());
            //outcome.Add("\n");

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
                    if (world[y,x] == "S")
                    {
                        //Console.WriteLine($"Start found at '{x}, {y}'");
                        //sPosition = Tuple.Create(x, y);
                        openSet.Add(Tuple.Create(Tuple.Create(0,directions), Tuple.Create(x, y)));
                        foundStart = true;
                    }
                     else if (world[y, x] == "G")
                    {
                        //Console.WriteLine($"Goal found at '{x}, {y}'");
                        //gPosition = Tuple.Create(directions, Tuple.Create(x, y));
                        gPositions.Add(Tuple.Create(directions, Tuple.Create(x, y)));
                        foundGoal = true;
                    }
                }
            }

            StartGoalExists = foundStart && foundGoal;
        }

        static void findLowestG()
        {
            //Console.WriteLine($"\tFunction '{System.Reflection.MethodBase.GetCurrentMethod().Name}'");
            int lowestG = openSet[0].Item1.Item1;
            int lowestOpenSet = 0;
            for (int i = 0; i < openSet.Count; i++)
            {
                //Console.WriteLine($"Current g: {lowestG}, g of openSet[{i}] is {openSet[i].Item1.Item1}, openSet[{i}] position is '{openSet[i].Item2.Item1}, {openSet[i].Item2.Item2}'");
                if (openSet[i].Item1.Item1 < lowestG)
                {
                    lowestG = openSet[i].Item1.Item1;
                    lowestOpenSet = i;
                }
            }
            currentPosition = openSet[lowestOpenSet];
            //Console.WriteLine($"Lowest heuristic is {cPosition.Item1.Item1} at position {cPosition.Item2.Item1}, {cPosition.Item2.Item2}");
            openSet.Remove(openSet[lowestOpenSet]);
            if (world[currentPosition.Item2.Item2, currentPosition.Item2.Item1] != "S") //Keep 'Start' position for visual reference
            {
                world[currentPosition.Item2.Item2, currentPosition.Item2.Item1] = "-";
            }
        }

        static void findValidNeighbours()
        {
            //Console.WriteLine($"\tFunction '{System.Reflection.MethodBase.GetCurrentMethod().Name}'");
            successors.Clear();
            //Up =      y-1
            if (currentPosition.Item2.Item2 - 1 >= 0)
            {
                generateNeighbour("up", currentPosition.Item2.Item1, currentPosition.Item2.Item2 - 1);
            }
            //Left =    x-1
            if (currentPosition.Item2.Item1 - 1 >= 0)
            {
                generateNeighbour("left", currentPosition.Item2.Item1 - 1, currentPosition.Item2.Item2);
            }
            //Down =    y+1
            if (currentPosition.Item2.Item2 + 1 < world.GetLength(0))
            {
                generateNeighbour("down", currentPosition.Item2.Item1, currentPosition.Item2.Item2 + 1);
            }
            //Right =   x+1
            if (currentPosition.Item2.Item1 + 1 < world.GetLength(1))
            {
                generateNeighbour("right", currentPosition.Item2.Item1 + 1, currentPosition.Item2.Item2);
            }
        }

        static void generateNeighbour(string nextDir, int xCheck, int yCheck)
        {
            directions = new List<string>(currentPosition.Item1.Item2.Count);
            if (currentPosition.Item1.Item2.Count > 0)
            {
                foreach (var pos in currentPosition.Item1.Item2)
                {
                    directions.Add(pos);
                }
            }

            directions.Add(nextDir);

            //Console.Write($"Checking {nextDir} position ({xCheck}, {yCheck})...");
            if (world[yCheck, xCheck] == "G")
            {
                //Console.WriteLine("Goal found!");
                successors.Add(Tuple.Create(Tuple.Create(0, directions), Tuple.Create(xCheck, yCheck)));
                gPosition = Tuple.Create(directions, Tuple.Create(xCheck, yCheck));
                goalReached = true;
            }
            else if (world[yCheck, xCheck] == " ")
            {
                //Console.WriteLine("added");
                successors.Add(Tuple.Create(Tuple.Create(0, directions), Tuple.Create(xCheck, yCheck)));
            }
        }

        static void neighbourHeuristics()
        {
            //Console.WriteLine($"\tFunction '{System.Reflection.MethodBase.GetCurrentMethod().Name}'");
            foreach (var neighbour in successors)
            {
                if (world[neighbour.Item2.Item2, neighbour.Item2.Item1] != "G")
                {
                    //Manhattan distance
                    int g = manhattanDistance(neighbour.Item2);

                    //Check exists
                    if (existsInSet(openSet, neighbour.Item2) || existsInSet(closedSet, neighbour.Item2))
                    {
                        //Console.WriteLine($"Ignoring neighbour {neighbour.Item2} because it currently exists");
                    }
                    else
                    {
                        openSet.Add(Tuple.Create(Tuple.Create(g, neighbour.Item1.Item2), neighbour.Item2));
                        world[neighbour.Item2.Item2, neighbour.Item2.Item1] = "N";
                    }
                }
            }
            closedSet.Add(currentPosition);
            worldGrid.printGrid(world);
        }

        static int manhattanDistance(Tuple<int, int> posCheck)
        {
            int manDist = Math.Abs(posCheck.Item1 - gPositions[0].Item2.Item1) + Math.Abs(posCheck.Item2 - gPositions[0].Item2.Item2);
            foreach (Tuple<List<string>, Tuple<int, int>> goal in gPositions)
            {   //Check the Manhattan Distance against all goal positions, take the lowest one
                int currDist = Math.Abs(posCheck.Item1 - goal.Item2.Item1) + Math.Abs(posCheck.Item2 - goal.Item2.Item2);
                if (currDist < manDist)
                {
                    manDist = currDist;
                }
            }
            return manDist;
        }

        static bool existsInSet(List<Tuple<Tuple<int, List<string>>, Tuple<int, int>>> setCheck, Tuple<int, int> posCheck)
        {
            bool exists = false;
            for (int i = 0; i < setCheck.Count; i++)
            {
                if (setCheck[i].Item2.Item1 == posCheck.Item1 && setCheck[i].Item2.Item2 == posCheck.Item2)
                {
                    exists = true;
                }
            }
            return exists;
        }
    }
}
