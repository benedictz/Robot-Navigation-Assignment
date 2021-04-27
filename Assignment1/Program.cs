using System;

namespace Assignment1 {
    class Program {
        static string[] fileLines;
        static string[,] world;

        static void Main(string[] args) {
            //Check that args has 2 objects
            if (args.Length != 2) {
                Console.WriteLine("Please enter the filepath and searchtype and try again");
                return;
            }
            
            //If file exists -> Create grid
            if (validation.IsFileValid(args[0]))
            {
                Console.WriteLine("File exists!");
                fileLines = validation.createTextArray();
                validation.parseTextFile(fileLines);
                world = worldGrid.getWorld();
                worldGrid.printGrid();
            } else {
                Console.WriteLine("File doesn't exist!");
                return;
            }

            //Send args[1] to validation
            validation.parseSearchType(args[1], world);
        }
    }
}
