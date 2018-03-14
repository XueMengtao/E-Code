using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using RayTracingProceed;

namespace CalNode_server
{
    class ComputerContral_server
    {
        private string profold;   //工程文件夹路径
        private string setup;     //setup文件名称
        private string ter;       //地形文件名称
        private string tx;        //tx文件名
        private string rx;         //rx文件名
        private string resultfold; //结果文件路径
        private string computeModelpath;
        public ComputerContral_server(string profold, string setup, string ter, string tx, string rx, string resultfold, string computeModelpath)
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
            //***********
            //setup = profold +setup;
            //ter = profold + ter;
            //tx = profold + tx;
            //rx = profold+ rx;
            //resultfold = profold  + resultfold;
            //###########
            string args = profold + " " + setup + " " + ter + " " + tx + " " + rx + " " + resultfold;
            try
            {
                LogFileManager.ObjLog.error(computeModelpath+"|"+args);
                ProcessStartInfo psi = new ProcessStartInfo(computeModelpath, args);
                psi.UseShellExecute = false;
                psi.CreateNoWindow = true;
                Process ps = Process.Start(psi);
                ps.WaitForExit();//需要定时，还未处理
                exitCode = ps.ExitCode;

            }
            catch (Exception e)
            {
                LogFileManager.ObjLog.error(e.Message, e);
                LogFileManager.ObjLog.error("进程打开失败");
            }
            return exitCode;

        }
        public int Computer2()
        {
            LogFileManager.ObjLog.info("进入computer2方法");
            LogFileManager.ObjLog.info("profold=" + profold + "---setup=" + setup + "---ter=" + ter + "---tx=" + tx + "---rx=" + rx + "---resultfold" + resultfold);
            string[] args = { profold, setup, ter, tx, rx, resultfold };
            //Thread t1 = new Thread(new ParameterizedThreadStart(RayTracingProceed.RayTracingProceed.test));
            //t1.Start(args);
            RayTracingProceed.RayTracingProceed.test(args);
            return 0;
        }
    }
}
