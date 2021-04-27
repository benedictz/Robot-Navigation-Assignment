using System;
using System.Collections.Generic;

namespace Assignment1
{
    class cus2
    {
        //This is similar to the A* method in every way, except instead of just using the four horizontal and vertical directions, we will also be using diagonal directions.
        //This increases our moveset from four to eight, giving a more natural 2D movement set. This will lower the amount of moves in all situations, as well as provide access between gaps
        //that would otherwise block an agent that can only move in the original four directions.

        static string[,] world;
        static int nodes = 0;
        static bool StartGoalExists = false;    //Check if both Start and Goal positions exist
        static bool goalReached = false;        //Check if goal has been reached
        static List<string> directions = new List<string>();

        //<x, y> data
        //static Tuple<int, int>                            sPosition;                                                      //Start position
        static Tuple<List<string>, Tuple<int, int>> gPosition;                                                      //Used as final reached goal
        static List<Tuple<List<string>, Tuple<int, int>>> gPositions = new List<Tuple<List<string>, Tuple<int, int>>>();   //List of all goal positions
        //static goalPosition newGoalPosition = new goalPosition(); //Class instead of tuples, change names after

        //<<f steps, h heuristic>, directions>, <x, y> currentPosition
        static Tuple<Tuple<Tuple<int, int>, List<string>>, Tuple<int, int>> cPosition;

        //List of <<steps, heuristic>, directions>, <x, y>
        static List<Tuple<Tuple<Tuple<int, int>, List<string>>, Tuple<int, int>>> successors = new List<Tuple<Tuple<Tuple<int, int>, List<string>>, Tuple<int, int>>>();
        static List<Tuple<Tuple<Tuple<int, int>, List<string>>, Tuple<int, int>>> openSet = new List<Tuple<Tuple<Tuple<int, int>, List<string>>, Tuple<int, int>>>();
        static List<Tuple<Tuple<Tuple<int, int>, List<string>>, Tuple<int, int>>> closedSet = new List<Tuple<Tuple<Tuple<int, int>, List<string>>, Tuple<int, int>>>();

        public static string beginSearch(string[,] pWorld)
        {
            List<string> outcome = new List<string>();
            world = pWorld;

            findStartEnd();
            while (openSet.Count != 0 && !goalReached && StartGoalExists) //While openSet is not empty, and a Goal has not been found, and both Start and Goal positions exist
            {
                nodes++;                //Increment node count
                findLowestF();          //Visit first node in openSet with lowest distance to goal
                findValidSuccessors();  //Generate surrounding successors
                successorHeuristics();  //Create heuristics for successors
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
                        //Console.WriteLine($"Start found at '{x}, {y}'");
                        //sPosition = Tuple.Create(x, y);
                        openSet.Add(Tuple.Create(Tuple.Create(Tuple.Create(0, 0), directions), Tuple.Create(x, y))); //<-- default '0' for heuristic start value
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

        //Find openSet position with lowest 'f' value
        static void findLowestF()
        {
            int lowestH = openSet[0].Item1.Item1.Item2;
            int lowestOpenSet = 0;
            for (int i = 0; i < openSet.Count; i++)
            {
                //Console.WriteLine($"Current h: {lowestH} - h of openSet[{i}] is {openSet[i].Item1.Item1.Item2} - openSet[{i}] position is '{openSet[i].Item2.Item1}, {openSet[i].Item2.Item2}'");
                if (openSet[i].Item1.Item1.Item2 < lowestH)
                {
                    lowestH = openSet[i].Item1.Item1.Item2;
                    lowestOpenSet = i;
                }
            }
            cPosition = openSet[lowestOpenSet];
            //Console.WriteLine($"Lowest heuristic is {cPosition.Item1.Item1.Item2} at position {cPosition.Item2.Item1}, {cPosition.Item2.Item2}");
            openSet.Remove(openSet[lowestOpenSet]);
            if (world[cPosition.Item2.Item2, cPosition.Item2.Item1] != "S") //Keep 'Start' position for visual reference
            {
                world[cPosition.Item2.Item2, cPosition.Item2.Item1] = "-";
            }
        }

        //Generate surrounding successors
        static void findValidSuccessors()
        {
            successors.Clear(); //Reset successors list
            //Up =          y-1
            if (cPosition.Item2.Item2 - 1 >= 0)
            {
                generateSuccessor("up", cPosition.Item2.Item1, cPosition.Item2.Item2 - 1);
            }
            //UpLeft =      x-1, y-1
            if (cPosition.Item2.Item1 - 1 >= 0 && cPosition.Item2.Item2 - 1 >= 0){
                generateSuccessor("upleft", cPosition.Item2.Item1 - 1, cPosition.Item2.Item2 - 1);
            }
            //Left =        x-1
            if (cPosition.Item2.Item1 - 1 >= 0)
            {
                generateSuccessor("left", cPosition.Item2.Item1 - 1, cPosition.Item2.Item2);
            }
            //DownLeft =    x-1, y+1
            if (cPosition.Item2.Item1 - 1 >= 0 && cPosition.Item2.Item2 + 1 < world.GetLength(0))
            {
                generateSuccessor("downleft", cPosition.Item2.Item1 - 1, cPosition.Item2.Item2 + 1);
            }
            //Down =        y+1
            if (cPosition.Item2.Item2 + 1 < world.GetLength(0))
            {
                generateSuccessor("down", cPosition.Item2.Item1, cPosition.Item2.Item2 + 1);
            }
            //DownRight =   x+1, y+1
            if (cPosition.Item2.Item1 + 1 < world.GetLength(1) && cPosition.Item2.Item2 + 1 < world.GetLength(0))
            {
                generateSuccessor("downright", cPosition.Item2.Item1 + 1, cPosition.Item2.Item2 + 1);
            }
            //Right =       x+1
            if (cPosition.Item2.Item1 + 1 < world.GetLength(1))
            {
                generateSuccessor("right", cPosition.Item2.Item1 + 1, cPosition.Item2.Item2);
            }
            //UpRight=      x+1, y-1
            if (cPosition.Item2.Item1 + 1 < world.GetLength(1) && cPosition.Item2.Item2 - 1 >= 0)
            {
                generateSuccessor("upright", cPosition.Item2.Item1 + 1, cPosition.Item2.Item2 - 1);
            }
        }

        //Create successor
        static void generateSuccessor(string nextDir, int xCheck, int yCheck)
        {
            directions = new List<string>(cPosition.Item1.Item2.Count);
            if (cPosition.Item1.Item2.Count > 0)
            {
                foreach (var pos in cPosition.Item1.Item2)
                {
                    directions.Add(pos);
                }
            }

            directions.Add(nextDir);

            //Console.Write($"Checking {nextDir} position ({xCheck}, {yCheck})...");
            if (world[yCheck, xCheck] == " ")
            {
                successors.Add(Tuple.Create(Tuple.Create(cPosition.Item1.Item1, directions), Tuple.Create(xCheck, yCheck)));
            }
            else if (world[yCheck, xCheck] == "G")
            {   //Goal found in this position
                successors.Add(Tuple.Create(Tuple.Create(cPosition.Item1.Item1, directions), Tuple.Create(xCheck, yCheck)));
                gPosition = Tuple.Create(directions, Tuple.Create(xCheck, yCheck));
                goalReached = true;
            }
        }

        //Create heuristics for successors
        static void successorHeuristics()
        {
            for (int i = 0; i < successors.Count; i++)
            {
                if (world[successors[i].Item2.Item2, successors[i].Item2.Item1] != "G")
                {
                    int f, g, h;
                    f = successors[i].Item1.Item1.Item1 + 1;
                    g = manhattanDist(successors[i].Item2);
                    h = f + g;

                    if (existsInSet(openSet, successors[i], h) || existsInSet(closedSet, successors[i], h))
                    {
                        //Already exists and has lowest possible heuristic value, do nothing
                    }
                    else
                    {   //Add to openSet
                        openSet.Add(Tuple.Create(Tuple.Create(Tuple.Create(f, h), successors[i].Item1.Item2), Tuple.Create(successors[i].Item2.Item1, successors[i].Item2.Item2)));
                        world[successors[i].Item2.Item2, successors[i].Item2.Item1] = "N";
                    }
                }
            }
            closedSet.Add(cPosition);
            worldGrid.printGrid(world);
        }
        
        static int manhattanDist(Tuple<int, int> successor)
        {
            int manDist = Math.Abs(successor.Item1 - gPositions[0].Item2.Item1) + Math.Abs(successor.Item2 - gPositions[0].Item2.Item2);
            foreach (Tuple<List<string>, Tuple<int, int>> goal in gPositions)
            {   //Check the Manhattan Distance against all goal positions, take the lowest one
                int currDist = Math.Abs(successor.Item1 - goal.Item2.Item1) + Math.Abs(successor.Item2 - goal.Item2.Item2);
                if (currDist < manDist)
                {
                    manDist = currDist;
                }
            }
            return manDist;
        }

        static bool existsInSet(List<Tuple<Tuple<Tuple<int, int>, List<string>>, Tuple<int, int>>> setCheck, Tuple<Tuple<Tuple<int, int>, List<string>>, Tuple<int, int>> successor, int h)
        {
            bool exists = false;
            for (int i = 0; i < setCheck.Count; i++)
            {
                if (setCheck[i].Item2.Item1 == successor.Item2.Item1 && setCheck[i].Item2.Item2 == successor.Item2.Item2)
                {
                    if (setCheck[i].Item1.Item1.Item2 <= h)
                    {
                        exists = true;
                    }
                }
            }
            return exists;
        }
    }
}
