using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using RayTracingProceed;
using RayCalInfo;
using System.Reflection;
using System.Configuration;
using CalculateModelClasses;

namespace TestForCal
{
    class Program
    {
        static void Main(string[] args)
        {
            string setuppath = ".\\.\\project\\xmt2018030803\\xmt2018030803.setup";
            string terpath = ".\\.\\project\\xmt2018030803\\xmt2018030803.ter";
            string txpath = ".\\.\\project\\xmt2018030803\\xmt2018030803.tx";
            string rxpath = ".\\.\\project\\xmt2018030803\\xmt2018030803.rx";

            RayTracingProceed.RayTracingProceed.Calculate(setuppath, terpath, txpath, rxpath);

            Console.WriteLine("计算节点跑完了");
        }
    }
}
