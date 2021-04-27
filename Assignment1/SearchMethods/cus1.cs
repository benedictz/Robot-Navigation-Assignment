using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment1 {
    class cus1 {
        //This is the Random Mouse algorithm. It is normally used for mazes that have strict pathways of one width size, however this assignment implements an entire grid. As such, it is not
        //anywhere near as useful and can often loop itself into its own self-made corner. In addition to this, it theoretically might never mind the goal, due to its random nature.

        static string[,]        world;
        static int              nodes = 0;
        static bool             StartGoalExists = false;    //Check if both Start and Goal positions exist
        static bool             goalReached = false;        //Check if goal has been reached

        static List<string>                                 directions = new List<string>();
        static Tuple<List<string>, Tuple<int, int>>         currentPosition;        //Directions and current position
        static List<Tuple<List<string>, Tuple<int, int>>>   availableDirections;    //Directions and positions around current position


        public static string beginSearch(string[,] pWorld)
        {
            List<string> outcome = new List<string>();
            world = pWorld;

            findStartEnd();
            if (StartGoalExists)
            {
                while (!goalReached)
                {
                    nodes++;
                    createAllDir();
                    pickRandomDir();
                }
            }

            outcome.Add(nodes.ToString());

            return string.Join("", outcome);
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
                        currentPosition = Tuple.Create(directions, Tuple.Create(y, x));
                        foundStart = true;
                    }
                    else if (world[y, x] == "G")
                    {
                        foundGoal = true;
                    }
                }
            }

            StartGoalExists = foundStart && foundGoal;
        }

        static void createAllDir()
        {
            availableDirections = new List<Tuple<List<string>, Tuple<int, int>>>();
            //Up        y-1
            if (currentPosition.Item2.Item2 - 1 >= 0)
            {
                //availableDirections.Add()
            }
            //Left      x-1
            //Down      y+1
            //Right     x+1

            if (availableDirections.Count == 0)
            {
                //Get rid of all visited notes
                createAllDir();
            }
        }

        static void pickRandomDir()
        {
            //Pick random direction

        }
    }
}
