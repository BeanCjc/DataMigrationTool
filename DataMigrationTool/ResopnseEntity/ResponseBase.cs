using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMigrationTool.ResopnseEntity
{
    class ResponseBase
    {    
        /// <summary>
        /// 0失败1成功
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 信息
        /// </summary>
        public string Info { get; set; }
    }
}
