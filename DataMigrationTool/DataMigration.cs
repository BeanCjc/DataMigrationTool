using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataMigrationTool.New;
using DataMigrationTool.Old;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using static System.String;

namespace DataMigrationTool
{
    public partial class DataMigration : Form
    {
        public DataMigration()
        {
            InitializeComponent();
        }

        private bool _flag, _importFlag;
        private Dictionary<string, string> _cardTypeAndMaxCardId = new Dictionary<string, string>();
        private List<CardTypeOld> _cardList = new List<CardTypeOld>();
        private string _brandId = Common.GetConfigValue("brandid");
        private string _urlGetCardType = @"";
        private string _urlGetCard = @"";
        private string _urlPostCard = @"";



        private void DataMigration_Load(object sender, EventArgs e)
        {
            var dataSource = new List<CardTypeOld>
            {
                new CardTypeOld {Card_Name = "A类", Id = "0", Type = "0"},
                new CardTypeOld {Card_Name = "B类", Id = "1", Type = "1"},
                new CardTypeOld {Card_Name = "C类", Id = "2", Type = "1"}
            };
            //var result = Common.Get(_urlGetCardType + $@"?token={_brandId}");
            //if (result == null || result.Status != 1)
            //{
            //    MessageBox.Show($@"API request failed,reason:{result?.Info}");
            //}
            //var dataSource = (List<CardTypeOld>)result.List ?? new List<CardTypeOld>();
            foreach (var type in dataSource)
            {
                if (IsNullOrEmpty(Common.GetConfigValue(type.Id)))
                {
                    Common.SetConfigValueByKey(type.Id, "0");
                }
                _cardTypeAndMaxCardId.Add(type.Id, Common.GetConfigValue(type.Id));
            }
            comboBox1.DataSource = dataSource;
            comboBox1.DisplayMember = "Card_Name";
            comboBox1.ValueMember = "Id";
            comboBox1.SelectedValue = "-1";
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            _flag = true;
        }


        private bool autoImport;
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.rtxtInfo.Focus();
            if (!_flag) return;
            if (_importFlag)
            {
                return;
            }
            _cardList.Clear();
            var cardTypeId = comboBox1.SelectedValue.ToString();
            var cardTypeName = ((CardTypeOld)comboBox1.SelectedItem).Card_Name;
            var cardTypeType = ((CardTypeOld)comboBox1.SelectedItem).Type;
            var data = new List<CardOld>
            {
                new CardOld
                {
                    Id ="1",
                    Name = "会员卡1",
                    Card_no = "1111111111",
                    Mobile = "13100001111",
                    Balance = 1000,
                    Integral = 111,
                    Sex = "男",
                    Level_name = "一级会员",
                    OpenId = "o01",
                    Solar_birthday = "2001-01-01",
                    Pay_password = "111111",
                    Reg_time = DateTime.Now,
                    Coupon = new List<CouponOld>
                    {
                        new CouponOld
                        {
                            Id="1001",
                            Type = "0",
                            Title = "卡券标题1",
                            Card_type = "1",
                            Start_date = DateTime.Now,
                            End_date = DateTime.Now.AddDays(7),
                            Face_value = 1000,
                            Min_price = 100,
                            Location_id_list = "1",
                            Status = 4,
                            Card_id = "1001",
                            Get_time = DateTime.Now,
                            UserCardCode = "1001",
                            OpenId = "o1001",
                            OuterId = "1001",
                            Remind = 0,
                            Product_codes="asdfg",
                            Store_id ="1001"
                        },
                        new CouponOld
                        {
                            Id="1002",
                            Type = "0",
                            Title = "卡券标题2",
                            Card_type = "1",
                            Start_date = DateTime.Now,
                            End_date = DateTime.Now.AddDays(7),
                            Face_value = 2000,
                            Min_price = 200,
                            Location_id_list = "1",
                            Status = 4,
                            Card_id = "2002",
                            Get_time = DateTime.Now,
                            UserCardCode = "2002",
                            OpenId = "o2002",
                            OuterId = "2002",
                            Remind = 0,
                            Product_codes="asdfg",
                            Store_id ="2002"
                        }
                    }
                },
                new CardOld
                {
                    Id ="2",
                    Name = "会员卡2",
                    Card_no = "2222222222",
                    Mobile = "13200002222",
                    Balance = 2000,
                    Integral = 222,
                    Sex = "男",
                    Level_name = "一级会员",
                    OpenId = "o02",
                    Solar_birthday = "2002-02-02",
                    Pay_password = "222222",
                    Reg_time = DateTime.Now,
                    Coupon = new List<CouponOld>
                    {
                        new CouponOld
                        {
                            Id="2002",
                            Type = "0",
                            Title = "卡券标题2",
                            Card_type = "1",
                            Start_date = DateTime.Now,
                            End_date = DateTime.Now.AddDays(7),
                            Face_value = 2000,
                            Min_price = 200,
                            Location_id_list = "1",
                            Status = 4,
                            Card_id = "2002",
                            Get_time = DateTime.Now,
                            UserCardCode = "2002",
                            OpenId = "o2002",
                            OuterId = "2002",
                            Remind = 0,
                            Product_codes="asdfg",
                            Store_id ="2002"
                        },
                        new CouponOld
                        {
                            Id="2002",
                            Type = "0",
                            Title = "卡券标题2",
                            Card_type = "1",
                            Start_date = DateTime.Now,
                            End_date = DateTime.Now.AddDays(7),
                            Face_value = 2000,
                            Min_price = 200,
                            Location_id_list = "1",
                            Status = 4,
                            Card_id = "2002",
                            Get_time = DateTime.Now,
                            UserCardCode = "2002",
                            OpenId = "o2002",
                            OuterId = "2002",
                            Remind = 0,
                            Product_codes="asdfg",
                            Store_id ="2002"
                        }
                    }
                }
            }.Where(t => Compare(t.Id, _cardTypeAndMaxCardId[cardTypeId], StringComparison.Ordinal) >= 0).ToList();
            //var result = Common.Get(_urlGetCard + $@"?token={_brandId}&card_id={cardTypeId}&id={_cardTypeAndMaxCardId[cardTypeId]}");
            //if (result == null || result.Status != 1)
            //{
            //    MessageBox.Show($@"API request failed.Reason:{result?.Info}");
            //}
            //var data = (List<CardOld>)result.List ?? new List<CardOld>();
            var converter = new IsoDateTimeConverter
            {
                DateTimeFormat = "yyyy-MM-dd HH:mm:ss"
            };
            rtxt_data.Invoke(new Action(() => rtxt_data.Text = $@"{Common.ConvertJsonString(JsonConvert.SerializeObject(data, converter))}"));
            _cardList.Add(new CardTypeOld
            {
                Id = cardTypeId,
                Card_Name = cardTypeName,
                Type = cardTypeType,
                Data = data
            });
            txtMaxId.Text = _cardTypeAndMaxCardId[cardTypeId];
            rtxtInfo.Invoke(new Action(() => rtxtInfo.Text += $"Get data successfully.Id begins:{_cardTypeAndMaxCardId[cardTypeId]}.Data recorders are:{data.Count}.\r\n"));
            if (autoImport)
            {
                btnImport_Click(null, null);
            }
            //MessageBox.Show($@"Get data successfully.Id begins:{_cardTypeAndMaxCardId[cardTypeId]}.Data recorders are:{data.Count}");
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            if (_importFlag) return;
            try
            {
                _importFlag = true;
                autoImport = true;
                if (_cardList.Count <= 0)
                {
                    autoImport = false;
                    return;
                }
                var d = new List<string>();
                foreach (var item in _cardList)
                {
                    if (item.Data.Count <= 0)
                    {
                        autoImport = false;
                        rtxtInfo.Invoke(new Action(() => rtxtInfo.Text += $"CardTypeId[{item.Id}] don't have more data.\r\n"));
                        return;
                    }

                    //POST请求
                    var param = new Dictionary<string, string>();
                    var converter = new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" };
                    param.Add("token", _brandId);
                    param.Add("card_id", item.Id);
                    param.Add("data", JsonConvert.SerializeObject(item.Data, converter));
                    //var result = Common.Post(_urlPostCard, param);
                    //var maxIdOld = item.Data.Min(t => t.Id);
                    //if (result == null || result.Status != 1)
                    //{
                    //    rtxtInfo.Text += $"-------------------------------------Error-------------------------------------\r\nImport data failed[CardTypeId:{item.Id} MaxCardId:{maxIdOld}].Reason:{result?.Info}\r\n";
                    //    break;
                    //}
                    var maxId = Convert.ToInt32(item.Data.Max(t => t.Id)) + 1;
                    rtxtInfo.Invoke(new Action(() => rtxtInfo.Text += $"Import successfully.Id begins {maxId - 1}.Import recorders are {item.Data.Count}.\r\n"));
                    _cardTypeAndMaxCardId[item.Id] = maxId.ToString();
                    Common.SetConfigValueByKey(item.Id, maxId.ToString());
                    d.Add(item.Id);
                }

                foreach (var item in d)
                {
                    _cardList.Remove(_cardList.FirstOrDefault(t => t.Id == item));
                }
                rtxt_data.Clear();
            }
            catch (Exception exception)
            {
                MessageBox.Show($"-------------------------------------Error-------------------------------------\r\n ExceptionInfo:{exception.Message}.\r\n");
            }
            finally
            {
                _importFlag = false;
                if (autoImport)
                {
                    comboBox1_SelectedIndexChanged(null, null);
                }
            }
        }
        private void rtxt_data_TextChanged(object sender, EventArgs e)
        {
            rtxt_data.SelectionStart = rtxt_data.Text.Length;
            rtxt_data.ScrollToCaret();
        }
        private void rtxtInfo_TextChanged(object sender, EventArgs e)
        {
            rtxtInfo.SelectionStart = rtxtInfo.Text.Length;
            rtxtInfo.ScrollToCaret();
        }

    }
}
