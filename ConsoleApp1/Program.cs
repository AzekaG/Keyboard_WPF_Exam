using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int[] arrs = new int[1000];
            for(int i = 0; i < arrs.Length; i++) 
            {
                arrs[i] = i;
                Console.WriteLine(arrs[i]);
            }

        }
    }
}
