using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataMigrationTool.Old;

namespace DataMigrationTool.ResopnseEntity
{
    class ResponseCardOld:ResponseBase
    {
        public List<CardOld> List { get; set; }
    }
}
