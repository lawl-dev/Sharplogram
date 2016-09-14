using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTProto.Model
{
    class DataCenter
    {
        public string Ip { get; set; }
        public int Port { get; set; }

        public DataCenter(string ip, string port)
        {
            Ip = ip;
            Port = port;
        }
    }
}
