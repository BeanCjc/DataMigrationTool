using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMigrationTool.New
{
    class CardTypeNew
    {
        /// <summary>
        /// 会员卡类型id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 会员卡类型名称
        /// </summary>
        public string Card_Name { get; set; }

        /// <summary>
        /// 卡类型【1=实体卡,0=微信卡】
        /// </summary>
        public string Type { get; set; }
    }
}
