using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using LogFileManager;

namespace WF1
{
    class FileCopyUI
    {
        //从完整路径名sourcepath拷贝到完整路径名destinationpath
        public static void FileCopy(string sourcepath,string destinationpath)
        {
            FileStream fi=null;
            FileStream fs=null;
            try
            {
                fi = new FileStream(destinationpath , FileMode.OpenOrCreate);
                fs = new FileStream(sourcepath  ,FileMode.Open);
                int i;
                do
                {
                    i = fs.ReadByte();
                    if (i != -1)
                        fi.WriteByte((byte)i);
                }
                while (i != -1);
            }
            catch (Exception exc)
            {
                ObjLog.debug(exc.Message);
            }
            finally
            {
                if(fs!=null)
                    fs.Close();
                if(fs!=null)
                    fi.Close();
             }
        }
    }
}
