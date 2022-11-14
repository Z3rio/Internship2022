using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Program
    {
        static void MainLoop()
        {
            Console.WriteLine("Input any mathmatical equation: ");
            string mathEq = Console.ReadLine();

            DataTable dt = new DataTable();

            try
            {
                var mathEqResult = dt.Compute(mathEq, "");

                Console.WriteLine($"The answer for \"{mathEq}\" is {mathEqResult}");

                Console.WriteLine("Press anything to start over");
            }
            catch (Exception e)
            {
                Console.WriteLine("Seems like your math equation was invalid...");

                Console.WriteLine("Press anything to start over");
            }

            Console.ReadLine();

            Console.Clear();
            MainLoop();
        }

        static void Main(string[] args)
        {
            MainLoop();
        }
    }
}
