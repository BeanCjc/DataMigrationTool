using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMigrationTool.RequestEntity
{
    class RECardOld
    {
        public string Token { get; set; }
        public string Card_Id { get; set; }
        public string Id { get; set; }
        public List<string> Appoint_Ids { get; set; }
    }
}
