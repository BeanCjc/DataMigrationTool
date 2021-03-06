﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMigrationTool.Old
{
    class CardOld
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
        /// 积分
        /// </summary>
        public decimal? Integral { get; set; }

        /// <summary>
        /// 余额
        /// </summary>
        public List<BalanceOld> Balance { get; set; }

        ///// <summary>
        ///// 积分余额
        ///// </summary>
        //public decimal Integral { get; set; }

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
        public string Reg_time { get; set; }


        public string NickName { get; set; }

        public string User_Id { get; set; }

        /// <summary>
        /// 卡券
        /// </summary>
        public List<CouponOld> Coupon { get; set; }
    }
}
