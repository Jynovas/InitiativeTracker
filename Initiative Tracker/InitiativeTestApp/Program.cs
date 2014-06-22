using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace InitiativeTestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Regex integerRegex = new Regex(@"^-?[0-9]+$");

            string test0 = "hello";
            string test1 = "0";
            string test2 = "54325164325";
            string test3 = "-54316";
            string test4 = "1,000"; // this should technically return as true, however, I am not getting into that right now
            string test5 = "14.002";

            Console.WriteLine("Test 0, {0} is an integer? {1}", test0, integerRegex.IsMatch(test0));
            Console.WriteLine("Test 1, {0} is an integer? {1}", test1, integerRegex.IsMatch(test1));
            Console.WriteLine("Test 2, {0} is an integer? {1}", test2, integerRegex.IsMatch(test2));
            Console.WriteLine("Test 3, {0} is an integer? {1}", test3, integerRegex.IsMatch(test3));
            Console.WriteLine("Test 4, {0} is an integer? {1}", test4, integerRegex.IsMatch(test4));
            Console.WriteLine("Test 5, {0} is an integer? {1}", test5, integerRegex.IsMatch(test5));

            Console.Read();
        }
    }
}
