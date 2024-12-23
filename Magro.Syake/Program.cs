using Magro.Syake.Parsing;
using System;
using System.IO;

namespace Magro.Syake
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("SyakeScript Compiler");
            var parser = new Parser();
            var reader = new StreamReader("main.ss");
            var module = parser.Parse("main", reader);
        }
    }
}
