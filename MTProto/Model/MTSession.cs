using MTProto.Secure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTProto.Model
{
    class MTSession
    {
        public DataCenter DataCenter { get; set; }
        public long Id { get; }

        public MTSession(DataCenter dataCenter, int id)
        {
            DataCenter = dataCenter;
            Id = id;
        }
    }
}
