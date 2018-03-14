using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace WindowsServiceNoteClient
{
    class ComputeContral
    {
        private string profold;   //工程文件夹路径
        private string setup;     //setup文件名称
        private string ter;       //地形文件名称
        private string tx;        //tx文件名
        private string rx;         //rx文件名
        private string resultfold; //结果文件路径
        private string computeModelpath;
        public ComputeContral(string profold, string setup, string ter, string tx, string rx, string resultfold,string computeModelpath)
        {
            this.profold = profold;
            this.setup = setup;
            this.ter = ter;
            this.tx = tx;
            this.rx = rx;
            this.resultfold = resultfold;
            this.computeModelpath = computeModelpath;
        }

        //返回调用计算模块后的计算结果
        public int Compute()
        {
            int exitCode = -1;
             string args=profold+" "+setup+" "+ter+" "+tx+" "+rx+" "+resultfold;
          
            Process ps = Process.Start(computeModelpath, args);
            ps.WaitForExit();//需要定时，还未处理
            exitCode = ps.ExitCode;
          

          return exitCode;
        }
    }
}
