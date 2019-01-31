using DataMigrationTool.Old;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMigrationTool.New
{
    class CardNew
    {
        /// <summary>
        /// 旧小微主键（根据主键大小排序，分批获取）
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// 会员名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 会员卡号
        /// </summary>
        public string card_no { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string mobile { get; set; }

        /// <summary>
        /// 积分
        /// </summary>
        public decimal? integral { get; set; }

        /// <summary>
        /// 余额
        /// </summary>
        public List<BalanceNew> balance { get; set; }

        ///// <summary>
        ///// 积分余额
        ///// </summary>
        //public decimal Integral { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public string sex { get; set; }

        /// <summary>
        /// 等级名称
        /// </summary>
        public string level_name { get; set; }

        /// <summary>
        /// 粉丝openid
        /// </summary>
        public string openid { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        public string solar_birthday { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string pay_password { get; set; }

        /// <summary>
        /// 注册日期
        /// </summary>
        public string reg_time { get; set; }

        public string nickname { get; set; }

        public string user_id { get; set; }

        /// <summary>
        /// 卡券
        /// </summary>
        public List<CouponNew> coupon { get; set; }
    }
}
