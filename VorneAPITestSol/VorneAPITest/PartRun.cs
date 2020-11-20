using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VorneAPITest
{
    namespace PartRunNS
    {

        public struct Disable_When
        {
            public double at_least;
            public string channel;
            public string metric;
            public string type;
        }

        public struct Data
        {
            public string changeover_reason;
            public string changeover_target;
            public List<double> count_multipliers;
            public List<Disable_When> disable_when;
            public double down_threshold;
            public double earned_labor;
            public double goal_count;
            public double good_pieces_left;
            public double ideal_cycle_time;
            public string job_description;
            public int job_event_id;
            public string job_id;
            public string part_description;
            public int part_event_id;
            public string part_id;
            public double percent_done;
            public bool start_with_changeover;
            public double takt_time;
            public double target_labor_per_piece;
            public double target_multiplier;
            
        }

        public struct Meta
        {
            public string asset_id;
            public string data_version;
            public string device_id;
        }


        public class PartRun
        {
            public Data data;
            public Meta meta;
        }
    }
}
