using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace list.remove
{
    class Program
    {
        static void Main(string[] args)
        {
            List<int> lists = new List<int>();
            for (int i = 0; i < 10; i++)
            {
                lists.Add(i);
            }
           

                for (int i = 0; i < lists.Count; i++)
                {


                    if ((lists[i] == 3) || (lists[i] == 4) || (lists[i] == 5))
                    {
                        lists.RemoveAt(i);
                        i--;
                    }

                    foreach (var item in lists)
                    {
                        Console.Write(item + " ");
                    }
                    Console.WriteLine();
                }
                
            
            Console.ReadLine();
        }
    }
}
