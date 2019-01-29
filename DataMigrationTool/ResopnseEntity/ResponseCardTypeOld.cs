using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataMigrationTool.Old;

namespace DataMigrationTool.ResopnseEntity
{
    class ResponseCardTypeOld : ResponseBase
    {
        public List<CardTypeOld> List { get; set; }
    }
}
