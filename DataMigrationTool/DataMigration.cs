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
using DataMigrationTool.Old;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using static System.String;
using DataMigrationTool.RequestEntity;
using DataMigrationTool.ResopnseEntity;
using DataMigrationTool.New;

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
        private string _tokenOld = Common.GetConfigValue("tokenold");
        private string _tokenNew = Common.GetConfigValue("tokennew");
        private string _urlGetCardType = Common.GetConfigValue("urlgetcardtype");
        private string _urlGetCardTypeCount = Common.GetConfigValue("urlgetcardtypecount");
        private string _urlGetCard = Common.GetConfigValue("urlgetcard");
        private string _urlGetCardTypeNew = Common.GetConfigValue("urlgetcardtypenew");
        private string _urlImportCard = Common.GetConfigValue("urlimportcard");
        private string _cardTypeId = "";
        private string _cardTypeName = "";
        private string _cardTypeType = "";
        

        private void DataMigration_Load(object sender, EventArgs e)
        {
            ///加载窗体时，
            ///1.获取新旧小微卡券类型数据
            ///2.同时将对应类型的卡券总数和当前最大卡券Id(默认当前是0)写入配置文件
            ///3.绑定下拉框的数据源为新旧小微共有的卡券类型数据(以名称相同判断为同一条数据)

            progressBar1.Visible = false;//不显示进度条
            var cardTypeOldData = GetCardTypesOld(_tokenOld);
            var cardTypeNewData = GetCardTypesNew(_tokenNew);
            if (cardTypeOldData == null || cardTypeNewData == null)
            {
                if (MessageBox.Show(cardTypeOldData == null ? "获取旧小微卡券类型数据失败，请检查API地址是否正确" : "获取新小微卡券类型数据失败，请检查API地址是否正确") == DialogResult.OK)
                {
                    Dispose();
                    return;
                }
            }
            var cardTypeList = new List<CardTypeNew>();
            foreach (var old in cardTypeOldData)
            {
                if (cardTypeNewData.Any(t => t.Card_Name?.Trim() == old.Card_Name?.Trim()))
                {
                    var cardCount = GetCardCount(_tokenOld, old.Id);
                    if (cardCount == -1)
                    {
                        if (MessageBox.Show("获取旧小微卡券总数数据失败，请检查API地址是否正确") == DialogResult.OK)
                        {
                            Dispose();
                            return;
                        }
                    }
                    var cardType = cardTypeNewData.FirstOrDefault(t => t.Card_Name?.Trim() == old.Card_Name?.Trim());
                    cardTypeList.Add(cardType);
                    if (IsNullOrEmpty(Common.GetConfigValue(cardType.Id)))
                    {
                        Common.SetConfigValueByKey(cardType.Id, "");//卡券最大ID不能每次加载窗体都改变，只能在更新数据处才可以修改改值
                    }
                    _cardTypeAndMaxCardId.Add(cardType.Id, Common.GetConfigValue(cardType.Id));//将卡券类型Id对应的最大卡券Id记录下来，以便后面使用
                    Common.SetConfigValueByKey(cardType.Id + "_max", cardCount.ToString());//最大数量每次加载时都会刷新
                }
            }
            comboBox1.DataSource = cardTypeList;
            comboBox1.DisplayMember = "Card_Name";
            comboBox1.ValueMember = "Id";
            comboBox1.SelectedValue = "-1";//默认不选择任何卡券类型
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;//不可编辑的下拉框

            #region 旧代码 已注释
            //progressBar1.Maximum = Convert.ToInt32(Common.GetConfigValue("recordercount"));

            //var param = JsonConvert.SerializeObject(new RECardTypeOld { Token = _token });
            //var resultStr = Common.Post(_urlGetCardType, param);
            //var result = JsonConvert.DeserializeObject<ResponseCardTypeOld>(resultStr);
            //if (result == null || result.Status != 1)
            //{
            //    MessageBox.Show($@"API request failed,reason:{result?.Info}");
            //}


            //var dataSource = result?.List ?? new List<CardTypeOld>();
            //foreach (var type in dataSource)
            //{
            //    if (IsNullOrEmpty(Common.GetConfigValue(type.Id)))
            //    {
            //        Common.SetConfigValueByKey(type.Id, "0");
            //    }
            //    _cardTypeAndMaxCardId.Add(type.Id, Common.GetConfigValue(type.Id));
            //}
            //comboBox1.DataSource = dataSource;
            //comboBox1.DisplayMember = "Card_Name";
            //comboBox1.ValueMember = "Id";
            //comboBox1.SelectedValue = "-1";
            //comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            //_flag = true;
            #endregion
        }
        
        private bool autoImport;
        /// <summary>
        /// 根据选择的卡券类型进行展示数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ///根据下拉框的值，展示数据
            ///这个的功能用处不大

            var cardTypeId = comboBox1.SelectedValue.ToString();
            var cards = GetCardsOld(_tokenOld, cardTypeId, _cardTypeAndMaxCardId[cardTypeId]);
            if (cards == null)
            {
                return;//获取数据失败直接返回
            }
            var converter = new IsoDateTimeConverter
            {
                DateTimeFormat = "yyyy-MM-dd HH:mm:ss"
            };
            rtxt_data.Invoke(new Action(() => rtxt_data.Text = $"{Common.ConvertJsonString(JsonConvert.SerializeObject(cards, converter))}\r\n"));

            
            #region 旧代码 已注释
            //rtxtInfo.Focus();
            //if (!_flag) return;
            //if (_importFlag)
            //{
            //    return;
            //}
            //_cardList.Clear();
            //_cardTypeId = !autoImport ? comboBox1.SelectedValue.ToString() : _cardTypeId;
            //_cardTypeName = !autoImport ? ((CardTypeOld)comboBox1.SelectedItem).Card_Name : _cardTypeName;
            //_cardTypeType = !autoImport ? ((CardTypeOld)comboBox1.SelectedItem).Type : _cardTypeType;

            //var param = JsonConvert.SerializeObject(new RECardOld { Token = _tokenOld, Card_Id = _cardTypeId, Id = _cardTypeAndMaxCardId[_cardTypeId] });
            //var resultStr = Common.Post(_urlGetCard, param);
            //var result = JsonConvert.DeserializeObject<ResponseCardOld>(resultStr);
            //if (result == null || result.Status != 1)
            //{
            //    MessageBox.Show($@"API request failed.Reason:{result?.Info}");
            //}
            //var data = (List<CardOld>)result.List ?? new List<CardOld>();
            //var converter = new IsoDateTimeConverter
            //{
            //    DateTimeFormat = "yyyy-MM-dd HH:mm:ss"
            //};
            //rtxt_data.Invoke(new Action(() => rtxt_data.Text = $@"{Common.ConvertJsonString(JsonConvert.SerializeObject(data, converter))}"));
            //_cardList.Add(new CardTypeOld
            //{
            //    Id = _cardTypeId,
            //    Card_Name = _cardTypeName,
            //    Type = _cardTypeType,
            //    Data = data
            //});
            //txtMaxId.Text = _cardTypeAndMaxCardId[_cardTypeId];
            //rtxtInfo.Invoke(new Action(() => rtxtInfo.Text += $"Get data successfully.Id begins:{_cardTypeAndMaxCardId[_cardTypeId]}.Data recorders are:{data.Count}.\r\n"));
            //if (autoImport)
            //{
            //    btnImport_Click(null, null);
            //}
            #endregion
        }
        
        private Dictionary<string, bool> keyValuePairs = new Dictionary<string, bool>();
        /// <summary>
        /// 导入操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImport_Click(object sender, EventArgs e)
        {
            ///界面选择一个卡券类型然后点击导入即可导入该类型的数据，异步执行
            ///前一个为执行完毕也可以执行其他的卡券类型的导入
            ///当前存在的导入将提示不可以重复导入
            ///判断当前卡券类型Id是否在集合中，若在集合中则提示并返回
            ///将对应的卡券类型Id加入集合即可执行导入函数
            ///


            var cardTypeId = comboBox1.SelectedValue.ToString();
            if (keyValuePairs.Any(t => t.Key == cardTypeId && t.Value == true))
            {
                if (MessageBox.Show("当前卡券类型正在导入数据中，请勿重复操作") == DialogResult.OK)
                {
                    return;
                }
            }
            else if (keyValuePairs.Any(t => t.Key == cardTypeId && t.Value == false))
            {
                keyValuePairs[cardTypeId] = true;
            }
            else
            {
                keyValuePairs.Add(cardTypeId, true);
            }
            Task.Run(() =>
            {
                ///另起线程各自卡券类型跑各自的
                ///1.获取数据
                ///2.导入数据
                ///
                var cardTypeId1 = cardTypeId;
                var count = 0;
                var max = Convert.ToInt32(Common.GetConfigValue(cardTypeId + "_max"));
                while (count < max)
                {
                    if (!keyValuePairs[cardTypeId1])
                    {
                        //导入被取消了，给个提示并返回
                        rtxtInfo.Invoke(new Action(() => rtxtInfo.Text += $"卡券类型[ID:{cardTypeId1}导入已被取消。已经导入的数据量为:{count}/{max}\r\n"));
                        return;
                    }
                    var cards = GetCardsOld(_tokenOld, cardTypeId1, _cardTypeAndMaxCardId[cardTypeId1]);
                    if (cards == null)
                    {
                        return;//获取数据失败或者获取到最后一次数据但记录数小于最大数量是退出
                    }
                    var cardsImport = AutoMapper.Mapper.Map<List<CardOld>, List<CardNew>>(cards);
                    var resultImport = Import(_tokenNew, cardTypeId1, cardsImport);
                    if (resultImport == null)
                    {
                        return;//插入失败直接退出
                    }
                    if (resultImport.Status != 1)
                    {
                        var sb = new StringBuilder();
                        foreach (var item in resultImport.Fail)
                        {
                            sb.Append($"id:{item.Id} failReason:{item.Reason}\r\n");
                        }
                        if (sb.Length > 0)
                        {
                            Common.WriteLog(DateTime.Now.ToString("yyyyMMdd") + "ImportFailed.txt", sb.ToString());
                        }
                    }
                    var sb1 = new StringBuilder();
                    foreach (var item in resultImport.Success)
                    {
                        sb1.Append($"id:{item}\r\n");
                    }
                    if (sb1.Length > 0)
                    {
                        Common.WriteLog(DateTime.Now.ToString("yyyyMMdd") + "Success.txt", sb1.ToString());
                    }
                    //更新最大Id
                    var maxId = resultImport.Success.Max() + 1;
                    Common.SetConfigValueByKey(cardTypeId, maxId.ToString());
                    count += resultImport.Fail.Count + resultImport.Success.Count;
                    rtxtInfo.Invoke(new Action(() => rtxtInfo.Text += $"卡券类型[ID:{cardTypeId1}已导入数据量为:{count}/{max}\r\n"));//给界面提示，不做进度条
                }
                //导入完成，删除键值对里对应的数据
                keyValuePairs.Remove(cardTypeId1);
            });

            
            #region 旧代码 已注释
            //if (_importFlag) return;
            //try
            //{
            //    _importFlag = true;
            //    autoImport = true;
            //    progressBar1.Visible = true;
            //    if (progressBar1.Value + 100 >= progressBar1.Maximum)
            //    {
            //        progressBar1.Value = progressBar1.Maximum;
            //        progressBar1.Visible = false;
            //    }
            //    else
            //    {
            //        progressBar1.Value += 100;
            //    }
            //    if (_cardList.Count <= 0)
            //    {
            //        autoImport = false;
            //        progressBar1.Visible = false;
            //        progressBar1.Value = 0;
            //        return;
            //    }
            //    var d = new List<string>();
            //    foreach (var item in _cardList)
            //    {
            //        if (item.Data.Count <= 0)
            //        {
            //            autoImport = false;
            //            rtxtInfo.Invoke(new Action(() => rtxtInfo.Text += $"CardTypeId[{item.Id}] don't have more data.\r\n"));
            //            progressBar1.Visible = false;
            //            progressBar1.Value = 0;
            //            return;
            //        }

            //        //POST请求
            //        var param = new Dictionary<string, string>();
            //        var converter = new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" };
            //        param.Add("token", _tokenOld);
            //        param.Add("card_id", item.Id);
            //        param.Add("data", JsonConvert.SerializeObject(item.Data, converter));
            //        //var result = Common.Post(_urlPostCard, param);
            //        //var maxIdOld = item.Data.Min(t => t.Id);
            //        //if (result == null || result.Status != 1)
            //        //{
            //        //    rtxtInfo.Text += $"-------------------------------------Error-------------------------------------\r\nImport data failed[CardTypeId:{item.Id} MaxCardId:{maxIdOld}].Reason:{result?.Info}\r\n";
            //        //    Common.WriteLog(DateTime.Now.ToString("yyyyMMdd") + "Error.txt", $"-------------------------------------Error-------------------------------------\r\nImport data failed[CardTypeId:{item.Id} MaxCardId:{maxIdOld}].Reason:{result?.Info}");
            //        //    break;
            //        //}
            //        var maxId = Convert.ToInt32(item.Data.Max(t => t.Id)) + 1;
            //        rtxtInfo.Invoke(new Action(() => rtxtInfo.Text += $"Import successfully.Id begins {maxId - 1}.Import recorders are {item.Data.Count}.\r\n"));
            //        _cardTypeAndMaxCardId[item.Id] = maxId.ToString();
            //        Common.SetConfigValueByKey(item.Id, maxId.ToString());
            //        d.Add(item.Id);
            //    }

            //    foreach (var item in d)
            //    {
            //        _cardList.Remove(_cardList.FirstOrDefault(t => t.Id == item));
            //    }
            //    rtxt_data.Clear();
            //}
            //catch (Exception exception)
            //{
            //    rtxtInfo.Invoke(new Action(() => rtxtInfo.Text += $"-------------------------------------Error-------------------------------------\r\n ExceptionInfo:{exception.Message}.\r\n"));
            //    Common.WriteLog(DateTime.Now.ToString("yyyyMMdd") + "Error.txt", $"-------------------------------------Error-------------------------------------\r\n ExceptionInfo:{exception.Message}.");
            //}
            //finally
            //{
            //    _importFlag = false;
            //    if (autoImport)
            //    {
            //        comboBox1_SelectedIndexChanged(null, null);
            //    }
            //}
            #endregion
        }
        
        /// <summary>
        /// 富文本框的滚动条永远在最底端
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rtxt_data_TextChanged(object sender, EventArgs e)
        {
            rtxt_data.SelectionStart = rtxt_data.Text.Length;
            rtxt_data.ScrollToCaret();
        }

        /// <summary>
        /// 取消对应卡券类型的导入操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cancelmenuitem_Click(object sender, EventArgs e)
        {
            //autoImport = false;
            var cardTypeId = comboBox1.SelectedValue.ToString();
            if (!keyValuePairs.Any(t => t.Key == cardTypeId)) return;
            keyValuePairs[cardTypeId] = false;
            //rtxtInfo.Invoke(new Action(() => rtxtInfo.Text += $"\r\n"));
        }

        /// <summary>
        /// 关闭窗体前将富文本框的内容记录常规日志
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataMigration_FormClosing(object sender, FormClosingEventArgs e)
        {
            Common.WriteLog(DateTime.Now.ToString("yyyyMMdd") + ".txt", rtxtInfo.Text);
        }

        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rtxt_data.Size = new Size(667, 348);
            progressBar1.Value += 10000;
            progressBar1.Visible = true;
        }

        /// <summary>
        /// 富文本框内容太多进行记录常规日志
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rtxtInfo_TextChanged(object sender, EventArgs e)
        {
            rtxtInfo.SelectionStart = rtxtInfo.Text.Length;
            rtxtInfo.ScrollToCaret();//滚动条永远滚到最底端
            if (rtxtInfo.Text.Length <= 999999999) return;
            Common.WriteLog(DateTime.Now.ToString("yyyyMMdd") + ".txt", rtxtInfo.Text);
            rtxtInfo.Clear();
        }

        /// <summary>
        /// 获取旧小微卡券类型
        /// </summary>
        /// <param name="token">旧小微token</param>
        /// <returns>返回旧小微卡券类型集合，返回null表示请求失败并记录错误日志</returns>
        List<CardTypeOld> GetCardTypesOld(string token)
        {
            if (IsNullOrEmpty(token))
            {
                Common.WriteLog(DateTime.Now.ToString("yyyyMMdd") + "Error.txt", $"获取旧小微卡券类型参数错误.token:{token}");
                return null;
            }
            var param = JsonConvert.SerializeObject(new RECardTypeOld { Token = token });
            var resultStr = Common.Post(_urlGetCardType, param);
            var result = JsonConvert.DeserializeObject<ResponseCardTypeOld>(resultStr);
            if (result == null || result.Status != 1 || result.List == null)
            {
                Common.WriteLog(DateTime.Now.ToString("yyyyMMdd") + "Error.txt", $"获取旧小微卡券类型返回失败.ErrorMessage:{result?.Info}");
                return null;
            }
            return result.List;
        }

        /// <summary>
        /// 获取旧小微指定卡券类型下卡券的总数
        /// </summary>
        /// <param name="token">旧小微token</param>
        /// <param name="cardTypeId">旧小微卡券类型ID</param>
        /// <returns>返回旧小微指定卡券类型的卡券总数，返回-1表示请求失败并记录错误日志</returns>
        int GetCardCount(string token, string cardTypeId)
        {
            if (IsNullOrEmpty(token) || IsNullOrEmpty(cardTypeId))
            {
                Common.WriteLog(DateTime.Now.ToString("yyyyMMdd") + "Error.txt", $"获取旧小微卡券类型总数参数错误.token:{token} card_id:{cardTypeId}");
                return -1;
            }
            var param = JsonConvert.SerializeObject(new RECardCountOld { Token = token, CardTypeId = cardTypeId });
            var resultStr = Common.Post(_urlGetCardTypeCount, param);
            var result = JsonConvert.DeserializeObject<ResponseCardCountOld>(resultStr);
            if (result == null || result.Status != 1)
            {
                Common.WriteLog(DateTime.Now.ToString("yyyyMMdd") + "Error.txt", $"获取旧小微卡券类型总数返回错误.ErrorMessage:{result?.Info}");
                return -1;
            }
            return result.Total;
        }

        /// <summary>
        /// 获取旧小微卡券数据
        /// </summary>
        /// <param name="token">旧小微token</param>
        /// <param name="cardTypeId">旧小微卡券类型ID</param>
        /// <param name="maxId">记录在本地的旧小微最大卡券ID</param>
        /// <returns>返回旧小微指定卡券类型的卡券信息，返回null表示请求失败并记录错误日志</returns>
        List<CardOld> GetCardsOld(string token, string cardTypeId, string maxId)
        {
            if (IsNullOrEmpty(token) || IsNullOrEmpty(cardTypeId) || IsNullOrEmpty(maxId))
            {
                Common.WriteLog(DateTime.Now.ToString("yyyyMMdd") + "Error.txt", $"获取旧小微卡券参数错误.token:{token} card_id:{cardTypeId} maxId:{maxId}");
                return null;
            }

            var param = JsonConvert.SerializeObject(new RECardOld { Token = token, Card_Id = cardTypeId, Id = maxId });
            var resultStr = Common.Post(_urlGetCard, param);
            var result = JsonConvert.DeserializeObject<ResponseCardOld>(resultStr);
            if (result == null || result.Status != 1 || result.List == null)
            {
                Common.WriteLog(DateTime.Now.ToString("yyyyMMdd") + "Error.txt", $"获取旧小微卡券返回错误.ErrorMessage:{result?.Info}");
                return null;
            }
            return result.List;
        }

        /// <summary>
        /// 获取新小微卡券类型
        /// </summary>
        /// <param name="token">新小微token</param>
        /// <returns>返回新小微卡券类型集合，返回null表示请求失败并记录错误日志</returns>
        List<CardTypeNew> GetCardTypesNew(string token)
        {
            if (IsNullOrEmpty(token))
            {
                Common.WriteLog(DateTime.Now.ToString("yyyyMMdd") + "Error.txt", $"获取新小微卡券类型参数错误.token:{token}");
                return null;
            }
            var param = JsonConvert.SerializeObject(new RECardTypeNew { TokenKey = token });
            var resultStr = Common.Post(_urlGetCardTypeNew, param);
            var result = JsonConvert.DeserializeObject<ResponseCardTypeNew>(resultStr);
            if (result == null || result.Status != 1 || result.List == null)
            {
                Common.WriteLog(DateTime.Now.ToString("yyyyMMdd") + "Error.txt", $"获取新小微卡券类型返回错误.ErrorMessage:{result?.Info}");
                return null;
            }
            return result.List;
        }

        /// <summary>
        /// 导入新小微卡券信息
        /// </summary>
        /// <param name="token">新小微token</param>
        /// <param name="cardTypeId">卡券类型ID</param>
        /// <param name="data">待导入的卡券数据</param>
        /// <returns>返回导入的信息，包括成功表示、成功和失败卡券的ID集合，返回null表示请求失败并记录错误日志</returns>
        ResponseImportCard Import(string token, string cardTypeId, List<CardNew> data)
        {
            if (IsNullOrEmpty(token) || IsNullOrEmpty(cardTypeId) || data == null)
            {
                Common.WriteLog(DateTime.Now.ToString("yyyyMMdd") + "Error.txt", $"导入参数错误.token:{token} card_id:{cardTypeId} data:{data}");
                return null;
            }
            var param = JsonConvert.SerializeObject(new REImportCard { Token = token, CardTypeId = cardTypeId, Data = data });
            var resultStr = Common.Post(_urlImportCard, param);
            var result = JsonConvert.DeserializeObject<ResponseImportCard>(resultStr);
            if (result == null || result.Status != 1)
            {
                Common.WriteLog(DateTime.Now.ToString("yyyyMMdd") + "Error.txt", $"导入新小微卡券返回错误.ErrorMessage:{result?.Info}");
                return null;
            }
            return result;
        }
    }
}
