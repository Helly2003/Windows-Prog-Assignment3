/*
FILE          : program.cs
PROJECT       : Assignment 3
PROGRAMMER    : Helly Shah (8958841)
FIRST VERSION : 2024-10-19
DESCRIPTION   : This program measure the performance of some data structures.
*/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Check the correct number of command line arguments
            if (args.Length != 2)
            {
                ShowUsage();
                return;
            }

            if (!int.TryParse(args[0], out int totalElements) || totalElements < 100 || totalElements >= 5000000)
            {
                Console.WriteLine("Error- The number of elements must be between 100 and 5,000,000.");
                ShowUsage();
                return;
            }

            if (!int.TryParse(args[1], out int testArraySize) || testArraySize < 1 || testArraySize > totalElements / 100)
            {
                Console.WriteLine("Error- The test array size should be at least 1 and no more than 1% of total elements.");
                ShowUsage();
                return;
            }

            // Create data structures
            string[] stringArray = new string[totalElements];
            List<string> stringList = new List<string>(totalElements);
            Dictionary<string, string> stringDictionary = new Dictionary<string, string>(totalElements);
            Random random = new Random();

            // Load data structures with random guid
            for (int i = 0; i < totalElements; i++)
            {
                string guid = Guid.NewGuid().ToString();
                stringArray[i] = guid;
                stringList.Add(guid);
                // Using guid as both key and value
                stringDictionary[guid] = guid; 
            }

            stringList.Sort();

            // Create valid test data array from string array
            string[] validTestArray = new string[testArraySize];
            for (int i = 0; i < testArraySize; i++)
            {
                validTestArray[i] = stringArray[random.Next(totalElements)];
            }

            // Create invalid test data array with new Guids
            string[] invalidTestArray = new string[testArraySize];
            for (int i = 0; i < testArraySize; i++)
            {
                invalidTestArray[i] = Guid.NewGuid().ToString();
            }

            // Perform tests and measure time for valid data
            Console.WriteLine("Performing tests with valid data:");
            MeasurePerformance("Valid Data", stringArray, validTestArray, stringList, stringDictionary);

            // Perform tests and measure time for invalid data
            Console.WriteLine("\nPerforming tests with invalid data:");
            MeasurePerformance("Invalid Data", stringArray, invalidTestArray, stringList, stringDictionary);

            // Display command line arguments
            Console.WriteLine($"\nCommand Line Arguments: Total Elements = {totalElements}, Test Array Size = {testArraySize}");

            Console.ReadLine();
        }

        static void MeasurePerformance(string testType, string[] stringArray, string[] testArray, List<string> stringList, Dictionary<string, string> stringDictionary)
        {
            Stopwatch stopwatch = new Stopwatch();

            // Measure time for String Array
            stopwatch.Start();
            for (int i = 0; i < testArray.Length; i++)
            {
                Array.Find(stringArray, x => x == testArray[i]);
            }

            stopwatch.Stop();
            double arrayTime = stopwatch.Elapsed.TotalMilliseconds;
            Console.WriteLine($"{testType}: String Array, Total Time: {arrayTime}ms, Average Time per Search: {arrayTime / testArray.Length}ms");

            // Measure time for List
            stopwatch.Restart();
            for (int i = 0; i < testArray.Length; i++)
            {
                stringList.BinarySearch(testArray[i]);
            }

            stopwatch.Stop();
            double listTime = stopwatch.Elapsed.TotalMilliseconds;
            Console.WriteLine($"{testType}: List, Total Time: {listTime}ms, Average Time per Search: {listTime / testArray.Length}ms");

            // Measure time for Dictionary
            stopwatch.Restart();
            for (int i = 0; i < testArray.Length; i++)
            {
                stringDictionary.TryGetValue(testArray[i], out _);
            }

            stopwatch.Stop();
            double dictionaryTime = stopwatch.Elapsed.TotalMilliseconds;
            Console.WriteLine($"{testType}: Dictionary, Total Time: {dictionaryTime}ms, Average Time per Search: {dictionaryTime / testArray.Length}ms");
        }

        static void ShowUsage()
        {
            Console.WriteLine("Usage: DataStructurePerformanceTest <totalElements> <testArraySize>");
        }
    }
}

