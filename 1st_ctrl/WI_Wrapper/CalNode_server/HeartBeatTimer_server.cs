using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Net;

namespace CalNode_server
{
    class HeartBeatTimer_server
    {
         private Timer timer;
        private Guid guid;
        public HeartBeatTimer_server(long time)
        {
            timer = new Timer(time);
            timer.Elapsed += new ElapsedEventHandler(timeOut);
            timer.AutoReset = true;
            timer.Enabled = false;
            guid = new Guid();
        }
        public void timeOut(object source, ElapsedEventArgs e)
        {
            ServiceReference1.Service1Client client = new ServiceReference1.Service1Client();
            client.HeartBeat(guid, GetIPAdress());
            client.Abort();
        }
        public void Start()
        {
            timer.Enabled = true;

        }
        private string GetIPAdress()
        {
            string HostName = Dns.GetHostName();
            IPAddress[] ipAdresses = Dns.GetHostAddresses(HostName);
            foreach (IPAddress ip in ipAdresses)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    return ip.ToString();
            }
            return null;
        }
    }
}
