using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace StringPerformanceTest
{
    [MemoryDiagnoser]
    public class Benchmark
    {
        string[] Lines;

        public int NumberOfLines;

        [Params("Bacon", "pork", "prosciutto")]
        public string SearchValue;

        [Params("Files/Bacon10.txt", "Files/Bacon25.txt", "Files/Bacon50.txt")]
        public string FileToRead;

        [GlobalSetup]
        public void GlobalSetup()
        {
            string fileLocation = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), FileToRead);
            Lines = File.ReadAllLines(fileLocation);
            NumberOfLines = Lines.Count();
        }


        [Benchmark]
        public int CountOccurrences()
        {
            string pattern = $"(?<=){SearchValue.ToLower()}";
            int count = 0;

            foreach (var line in Lines.Select(x => x.ToLower()).Where(y => y.Contains(SearchValue.ToLower())))
            {
                count += Regex.Matches(line, pattern).Count();
            }
            return count;
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            BenchmarkRunner.Run<Benchmark>();
        }
    }
}
