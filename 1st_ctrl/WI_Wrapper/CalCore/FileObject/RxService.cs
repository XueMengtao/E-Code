using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileObject
{
    class RxService
    {
        //工厂函数
        public static  RxObjectNew rxObjectNewFac(string txpath)
        { 
            //todo 由tx文件的路径，读取tx文件，进而为txobj对象每个属性赋值
            RxObjectNew rx = new RxObjectNew();
            //...
            return rx;
        }
    }
}
