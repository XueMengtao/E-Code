using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebAPIinHost
{
    class Program
    {
        //测试该项目中的方法用，被测方法必须改成public
        static void Main(string[] args)
        {
            ProjectResultMethodWebAPI p = new ProjectResultMethodWebAPI();
            p.SetProjectState(2, 2);
        }
    }
}
