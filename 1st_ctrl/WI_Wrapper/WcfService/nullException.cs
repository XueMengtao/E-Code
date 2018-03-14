using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;
namespace WcfService
{
    
    public class nullException:Exception
    {
        public nullException() { }
        public nullException(string message) : base(message) { }
        public nullException(string message, Exception inner) : base(message, inner) { }
    }
}
