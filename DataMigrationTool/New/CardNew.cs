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
        public string Id { get; set; }

        /// <summary>
        /// 会员名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 会员卡号
        /// </summary>
        public string Card_no { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 余额
        /// </summary>
        public decimal Balance { get; set; }

        /// <summary>
        /// 积分余额
        /// </summary>
        public decimal Integral { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public string Sex { get; set; }

        /// <summary>
        /// 等级名称
        /// </summary>
        public string Level_name { get; set; }

        /// <summary>
        /// 粉丝openid
        /// </summary>
        public string OpenId { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        public string Solar_birthday { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Pay_password { get; set; }

        /// <summary>
        /// 注册日期
        /// </summary>
        public DateTime Reg_time { get; set; }

        /// <summary>
        /// 卡券
        /// </summary>
        public List<CouponNew> Coupon { get; set; }
    }
}
