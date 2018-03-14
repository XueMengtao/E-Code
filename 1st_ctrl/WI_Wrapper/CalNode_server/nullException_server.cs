using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CalNode_server
{
    class nullException_server:Exception
    {
        public nullException_server() { }
        public nullException_server(string message) : base(message) { }
        public nullException_server(string message, Exception inner) : base(message, inner) { }
    }
}
