﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMigrationTool.New
{
    class CouponNew
    {
        /// <summary>
        /// 旧小微用户卡券ID
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// 类型(0=微信卡券，-1=活动卡券)
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// 优惠券标题
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// 卡券类型
        /// </summary>
        public string card_type { get; set; }

        /// <summary>
        /// 有效期开始时间
        /// </summary>
        public DateTime start_date { get; set; }

        /// <summary>
        /// 有效期结束时间
        /// </summary>
        public DateTime end_date { get; set; }

        /// <summary>
        /// 面值(折扣)
        /// </summary>
        public decimal face_value { get; set; }

        /// <summary>
        /// 最低消费金额
        /// </summary>
        public decimal min_price { get; set; }

        /// <summary>
        /// 关联旧小微门店ID
        /// </summary>
        public string location_id_list { get; set; }

        /// <summary>
        /// 状态(4=未使用,5=已使用,6=过期，7=用户删除卡券,8=系统删除)
        /// </summary>
        public int status { get; set; }

        /// <summary>
        /// 旧小微卡券ID
        /// </summary>
        public string card_id { get; set; }

        /// <summary>
        /// 领取时间
        /// </summary>
        public string get_time { get; set; }

        /// <summary>
        /// code序列号
        /// </summary>
        public string usercardcode { get; set; }

        /// <summary>
        /// 领取用户openid
        /// </summary>
        public string openid { get; set; }

        /// <summary>
        /// 领取场景值
        /// </summary>
        public string outerid { get; set; }

        /// <summary>
        /// 卡券快到期提醒(0=未提醒，1=已提醒)
        /// </summary>
        public int remind { get; set; }

        /// <summary>
        /// 可使用的商品编码
        /// </summary>
        public string product_codes { get; set; }

        /// <summary>
        /// 卡券所属旧小微门店
        /// </summary>
        public string store_id { get; set; }
    }
}
