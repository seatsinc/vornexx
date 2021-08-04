using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VorneAPITest
{
    namespace TeamNS
    {

        public struct Data
        {
            public string team_id;
            public double team_size;
        }

        public struct Meta
        {
            public string asset_id;
            public string data_version;
            public string device_id;
        }

        public class Team
        {
            public Data data;
            public Meta meta;
        }
    }
}
