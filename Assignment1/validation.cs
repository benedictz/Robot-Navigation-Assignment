using System;
using System.IO;
using System.Text.RegularExpressions;   //So Regex can work
using System.Collections.Generic;

namespace Assignment1 {
    class validation {
        static string filePathway;
        static string[] strNumbers;
        static List<int> intNumbers;

        //Check file actually exists -> save destination for later use
        public static bool IsFileValid (string directory)
        {
            Console.Write($"Now checking for file: {directory} --> ");
            if (File.Exists(directory))
            {
                filePathway = directory;
            }
            return File.Exists(directory);
        }

        //Split into individual lines
        public static string[] createTextArray()
        {
            return File.ReadAllLines(filePathway);
        }

        //For each individual line, get individual characters
        public static void parseTextFile(string[] lines)
        {            
            for (int lineNumber = 0; lineNumber < lines.Length; lineNumber++)
            {
                intNumbers = new List<int>();

                //Parse text, accept only digits
                strNumbers = Regex.Split(lines[lineNumber], @"\D+");
                foreach (string value in strNumbers)
                {
                    if (!string.IsNullOrEmpty(value))
                    {
                        intNumbers.Add(int.Parse(value));
                    }
                }

            //World Creation
            worldGrid.gridValues(lineNumber, intNumbers);
            }
        }

        public static void parseSearchType(string searchType, string[,] world)
        {
            string outcome = "";
            Console.Write("Search type: ");
            //Check searchType for valid search type -> run specified search type
            switch (searchType)
            {
                //UNINFORMED
                case "dfs":
                    worldGrid.searchType = "Depth-First Search";
                    outcome = dfs.beginSearch(world);
                    break;
                case "bfs":
                    worldGrid.searchType = "Breadth-First Search";
                    outcome = bfs.beginSearch(world);
                    break;

                //INFORMED
                case "gbfs":
                    worldGrid.searchType = "Greedy Best-First Search";
                    outcome = gbfs.beginSearch(world);
                    break;
                case "as":
                    worldGrid.searchType = "A* Search";
                    outcome = astar.beginSearch(world);
                    break;

                //CUSTOM
                case "cus1":
                    worldGrid.searchType = "Custom1 Search";
                    outcome = "\nCustom 1 does not currently work";
                    break;
                case "cus2":
                    worldGrid.searchType = "Custom2 Search";
                    outcome = cus2.beginSearch(world);
                    break;

                //ERROR
                default:
                    Console.WriteLine($"'{searchType}' does not match any available options. Please enter 'dfs', 'bfs', 'gbfs', 'as', 'cus1' or 'cus2'");
                    return;
            }

            Console.WriteLine($"{filePathway} {searchType} {outcome}");
        }
    }
}