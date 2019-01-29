using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMigrationTool.ResopnseEntity
{
    class ResponseImportCard:ResponseBase
    {
        public List<int> Success { get; set; }
        public List<FailInfo> Fail { get; set; }
    }
    class FailInfo
    {
        public int Id { get; set; }
        public string Reason { get; set; }
    }
}
