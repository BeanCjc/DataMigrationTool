using DataMigrationTool.New;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMigrationTool.RequestEntity
{
    class REImportCard
    {
        public string tokenKey { get; set; }
        public string card_id { get; set; }
        public List<CardNew> data { get; set; }
    }
}
