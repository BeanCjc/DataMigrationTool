using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMigrationTool.RequestEntity
{
    class RECardOld
    {
        public string token { get; set; }
        public string card_id { get; set; }
        public string id { get; set; }
        public List<string> appoint_ids { get; set; }
    }
}
