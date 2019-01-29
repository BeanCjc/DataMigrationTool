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
        public string Token { get; set; }
        public string CardTypeId { get; set; }
        public List<CardNew> Data { get; set; }
    }
}
