using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.Reflection;
namespace LogFileManager
{
    public class ObjLog
    {
        public static void debug(string message) 
        {
             ILog log=LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
             if (log.IsDebugEnabled) 
                { 
                 log.Debug(message); 
                }
            log = null;
        }
        public static void debug(string message, Exception exception)
        {
            ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            if (log.IsDebugEnabled)
                { 
                    log.Debug(message, exception);
                } 
            log = null; 
        }
        public static void info(string message)
        {
            ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            if (log.IsInfoEnabled)
            {
                log.Info(message);
            }
            log = null;
        }
        public static void info(string message, Exception exception)
        {
            ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            if (log.IsInfoEnabled)
            {
                log.Info(message, exception);
            }
            log = null;
        }
        public static void error(string message) 
        {
            ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            if (log.IsErrorEnabled)
            {
                log.Error(message);
            }
            log = null;
        }
        public static void error(string message, Exception exception)
        {
            ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            if (log.IsErrorEnabled)
            {
                log.Error(message, exception);
            }
            log = null;
        }
        public static void fatal(string message, Exception exception)
        {
            ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            if (log.IsFatalEnabled)
            {
                log.Fatal(message, exception);
            }
            log = null;
        }
        public static void fatal(string message)
        {
            ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            if (log.IsErrorEnabled)
            {
                log.Error(message);
            }
            log = null;
        }
    }
}
