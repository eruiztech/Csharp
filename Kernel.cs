/* Edgar Ruiz
 * CS 431
 * December 5, 2016
 */

using System;
using System.Collections.Generic;
using Sys = Cosmos.System;

namespace CosmosKernel1
{
    public class Kernel : Sys.Kernel
    {
        public static LinkedList<File> file_directory;
        public static LinkedList<Variable> variables;
        public static LinkedList<File> readyQueue;
        public static LinkedList<File> masterQueue;

        protected override void BeforeRun()
        {
            Console.WriteLine("Cosmos booted successfully.");
            file_directory = new LinkedList<File>();
            variables = new LinkedList<Variable>();
            readyQueue = new LinkedList<File>();
            masterQueue = new LinkedList<File>();
        }

        protected override void Run()
        {
            Console.Write("Please enter command: ");
            var input = Console.ReadLine();
            Commands.interpret(input);
        }
    }
}
