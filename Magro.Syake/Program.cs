using Magro.Common.MiddleLevel;
using Magro.Syake.Syntax;
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

            ModuleDeclaration module;
            using (var reader = new StreamReader("../../main.ss"))
            {
                module = parser.Parse("main", reader);
            }
        }
    }
}
