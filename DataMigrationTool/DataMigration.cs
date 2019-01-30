using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
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
        private Dictionary<string, Tuple<List<string>, string, string>> _cardTypeRelation = new Dictionary<string, Tuple<List<string>, string, string>>();//旧小微卡类型id集合，最大id，总数
        private List<CardTypeOld> _cardList = new List<CardTypeOld>();
        private string _tokenOld = Common.GetConfigValue("tokenold");
        private string _tokenNew = Common.GetConfigValue("tokennew");
        private static string _APIAddressOld = Common.GetConfigValue("apiaddressold");
        private static string _APIAddressNew = Common.GetConfigValue("apiaddressnew");

        private string _urlGetCardType = $"{_APIAddressOld}/Api/Member/getCardType/addon/MemberCard";
        private string _urlGetCardTypeCount = $"{_APIAddressOld}/Api/Member/getMemberTotal/addon/MemberCard";
        private string _urlGetCard = $"{_APIAddressOld}/Api/Member/getMember/addon/MemberCard";
        private string _urlGetCardTypeNew = $"{_APIAddressNew}/Api/Brand/getCardType/addon/Dining";
        private string _urlImportCard = $"{_APIAddressNew}/Api/Brand/batchImportMember/addon/Dining";
        private string _cardTypeId = "";
        private string _cardTypeName = "";
        private string _cardTypeType = "";


        private void DataMigration_Load(object sender, EventArgs e)
        {
            ///加载窗体时，
            ///1.获取新旧小微卡类型数据
            ///2.同时将对应类型的卡券总数和当前最大卡券Id(默认当前是0)写入配置文件
            ///3.绑定下拉框的数据源为新旧小微共有的卡类型数据(以名称相同判断为同一条数据)
            try
            {
                progressBar1.Visible = false;//不显示进度条
                var cardTypeOldData = GetCardTypesOld(_tokenOld);
                var cardTypeNewData = GetCardTypesNew(_tokenNew);
                if (cardTypeOldData == null || cardTypeNewData == null)
                {
                    if (MessageBox.Show(cardTypeOldData == null ? "获取旧小微卡类型数据失败，请检查API地址是否正确" : "获取新小微卡类型数据失败，请检查API地址是否正确") == DialogResult.OK)
                    {
                        Dispose();
                        return;
                    }
                }
                //var cardTypeList = new List<CardTypeNew>();
                foreach (var newCard in cardTypeNewData)
                {
                    //var cardType = cardTypeNewData.LastOrDefault(t => t.Card_Name?.Trim() == newCard.Card_Name?.Trim());
                    //cardTypeList.Add(cardType);
                    if (IsNullOrEmpty(Common.GetConfigValue(newCard.Id)))
                    {
                        Common.SetConfigValueByKey(newCard.Id, "0");//卡券最大ID不能每次加载窗体都改变，只能在更新数据处才可以修改改值
                    }

                    var idListOld = cardTypeOldData.Where(t => t.Card_Name?.Trim() == newCard.Card_Name?.Trim() && t.Type == newCard.Type).Select(x => x.Id).ToList();
                    var cardCount = 0;
                    foreach (var idOld in idListOld)
                    {
                        var cardCountTemp = GetCardCount(_tokenOld, idOld);
                        if (cardCountTemp == -1)
                        {
                            if (MessageBox.Show("获取旧小微卡券总数数据失败，请检查API地址是否正确") == DialogResult.OK)
                            {
                                Dispose();
                                return;
                            }
                        }
                        else
                        {
                            cardCount += cardCountTemp;
                        }
                    }
                    Common.SetConfigValueByKey(newCard.Id + "_total", cardCount.ToString());//最大数量每次加载时都会刷新
                    if (_cardTypeRelation.All(t => t.Key != newCard.Id))
                    {
                        _cardTypeRelation.Add(newCard.Id, new Tuple<List<string>, string, string>(idListOld, Common.GetConfigValue(newCard.Id), cardCount.ToString()));//将卡类型Id对应的最大卡券Id记录下来，以便后面使用
                    }
                    else
                    {
                        _cardTypeRelation[newCard.Id] = new Tuple<List<string>, string, string>(idListOld, Common.GetConfigValue(newCard.Id), cardCount.ToString());
                    }
                }
                comboBox1.DisplayMember = "Card_Name";
                comboBox1.ValueMember = "Id";
                comboBox1.DataSource = cardTypeNewData;
                //comboBox1.SelectedIndex = -1;//默认不选择任何卡类型
                comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;//不可编辑的下拉框
            }
            catch (Exception exception)
            {
                if (MessageBox.Show(exception.Message) == DialogResult.OK)
                {
                    Dispose();
                }
            }

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
        /// 根据选择的卡类型进行展示数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ///根据下拉框的值，展示数据
            ///这个的功能用处不大

            var cardTypeId = comboBox1.SelectedValue.ToString();
            if (_cardTypeRelation.All(t => t.Key != cardTypeId)) return;
            var cardsOld = new List<CardOld>();
            var idList = _cardTypeRelation[cardTypeId].Item1;
            var maxId = _cardTypeRelation[cardTypeId].Item2;
            var count = _cardTypeRelation[cardTypeId].Item3;
            foreach (var id in idList)
            {
                var cards = GetCardsOld(_tokenOld, id, maxId);
                if (cards == null)
                {
                    return;//获取数据失败直接返回
                }
                cardsOld.AddRange(cards);
            }

            var converter = new IsoDateTimeConverter
            {
                DateTimeFormat = "yyyy-MM-dd HH:mm:ss"
            };
            rtxt_data.Invoke(new Action(() => rtxt_data.Text = $"{Common.ConvertJsonString(JsonConvert.SerializeObject(cardsOld, converter))}\r\n"));
            txtMaxId.Invoke(new Action(() => txtMaxId.Text = maxId));
            txtCount.Invoke(new Action(() => txtCount.Text = count));

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
            ///界面选择一个卡类型然后点击导入即可导入该类型的数据，异步执行
            ///前一个为执行完毕也可以执行其他的卡类型的导入
            ///当前存在的导入将提示不可以重复导入
            ///判断当前卡类型Id是否在集合中，若在集合中则提示并返回
            ///将对应的卡类型Id加入集合即可执行导入函数
            ///


            var cardTypeId = comboBox1.SelectedValue.ToString();
            if (keyValuePairs.Any(t => t.Key == cardTypeId && t.Value))
            {
                if (MessageBox.Show("当前卡类型正在导入数据中，请勿重复操作") == DialogResult.OK)
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
                ///另起线程各自卡类型跑各自的
                ///1.获取数据
                ///2.导入数据
                ///
                var cardTypeId1 = cardTypeId;
                int count = 0, countSuccess = 0;
                var watcher = new Stopwatch();
                watcher.Start();//计时器
                var timeTotal = 0L;
                try
                {
                    var max = Convert.ToInt32(Common.GetConfigValue(cardTypeId1 + "_total"));
                    while (count <= max)
                    {
                        if (!keyValuePairs[cardTypeId1])
                        {
                            //导入被取消了，给个提示并返回
                            watcher.Stop();
                            var timeStamp = watcher.ElapsedMilliseconds;

                            rtxtInfo.Invoke(new Action(() => rtxtInfo.Text += $"新小微卡类型[ID:{cardTypeId1}]导入已被取消。已经导入的数据量为:{count}/{max}.本次用时{timeTotal + timeStamp}ms.\r\n"));
                            rtxt_data.Invoke(new Action(() => rtxt_data.Clear()));
                            return;
                        }
                        #region 获取数据

                        if (_cardTypeRelation.All(t => t.Key != cardTypeId1))
                        {
                            rtxtInfo.Invoke(new Action(() => rtxtInfo.Text += $"新小微卡类型[ID:{cardTypeId1}]导入失败(未知数据)，详情见日志\r\n"));
                            rtxt_data.Invoke(new Action(() => rtxt_data.Clear()));
                            keyValuePairs.Remove(cardTypeId1);
                            return;
                        }

                        var cardsOld = new List<CardOld>();
                        var idList = _cardTypeRelation[cardTypeId1].Item1;
                        var maxIdOld = _cardTypeRelation[cardTypeId1].Item2;
                        foreach (var id in idList)
                        {
                            var cards = GetCardsOld(_tokenOld, id, maxIdOld);
                            if (cards == null)
                            {
                                rtxtInfo.Invoke(new Action(() => rtxtInfo.Text += $"新小微卡类型[ID:{cardTypeId1}]导入失败(获取旧小微数据失败)，详情见日志\r\n"));
                                rtxt_data.Invoke(new Action(() => rtxt_data.Clear()));
                                keyValuePairs.Remove(cardTypeId1);
                                return;//获取数据失败直接返回
                            }
                            cardsOld.AddRange(cards);
                        }
                        #endregion

                        if (cardsOld.Count <= 0)
                        {
                            if (count > 0)
                            {
                                //循环出口
                                watcher.Stop();
                                var timeStamp1 = watcher.ElapsedMilliseconds;
                                rtxtInfo.Invoke(new Action(() => rtxtInfo.Text += $"新小微卡类型[ID:{cardTypeId1}]导入完成.总用时{timeTotal + timeStamp1}ms.\r\n"));
                                rtxt_data.Invoke(new Action(() => rtxt_data.Clear()));
                                keyValuePairs.Remove(cardTypeId1);
                                return;
                            }
                            //没数据，给个提示
                            watcher.Stop();
                            var timeStamp2 = watcher.ElapsedMilliseconds;
                            rtxtInfo.Invoke(new Action(() => rtxtInfo.Text += $"新小微卡类型[ID:{cardTypeId1}]没有数据可导入.本次用时{timeStamp2}ms.\r\n"));
                            rtxt_data.Invoke(new Action(() => rtxt_data.Clear()));
                            keyValuePairs.Remove(cardTypeId1);
                            return;
                        }
                        var cardsImport = AutoMapper.Mapper.Map<List<CardOld>, List<CardNew>>(cardsOld);
                        var resultImport = Import(_tokenNew, cardTypeId1, cardsImport);
                        if (resultImport == null)
                        {
                            rtxtInfo.Invoke(new Action(() => rtxtInfo.Text += $"新小微卡类型[ID:{cardTypeId1}]导入失败，详情见日志\r\n"));
                            rtxt_data.Invoke(new Action(() => rtxt_data.Clear()));
                            keyValuePairs.Remove(cardTypeId1);
                            return;//插入失败直接退出
                        }
                        var sb = new StringBuilder();
                        var sbStr = new StringBuilder();
                        foreach (var item in resultImport.Fail)
                        {
                            sb.Append($"id:{item.Id} failReason:{item.Reason}\r\n");
                            if (IsNullOrEmpty(sbStr.ToString()))
                            {
                                sbStr.Append(item.Id);
                                continue;
                            }
                            sbStr.Append(";" + item.Id);
                        }
                        if (sb.Length > 0)
                        {
                            Common.WriteLog(DateTime.Now.ToString("yyyyMMdd") + "ImportFailed.txt", sb.ToString());
                            if (IsNullOrEmpty(Common.GetConfigValue(cardTypeId1 + "_failids")))
                            {
                                Common.SetConfigValueByKey(cardTypeId1 + "_failids", sbStr.ToString());
                            }
                            else
                            {
                                var ddd = Common.GetConfigValue(cardTypeId1 + "_failids");
                                Common.SetConfigValueByKey(cardTypeId1 + "_failids", ddd + sbStr);
                            }
                        }

                        var sb1 = new StringBuilder();
                        foreach (var item in resultImport.Success)
                        {
                            sb1.Append($"id:{item}\r\n");
                        }
                        if (sb1.Length > 0)
                        {
                            Common.WriteLog(DateTime.Now.ToString("yyyyMMdd") + "ImportSuccess.txt", sb1.ToString());
                        }
                        //更新最大Id
                        var maxId = resultImport.Success.Max() > resultImport.Fail.Select(t => t.Id).Max() ? resultImport.Success.Max() : resultImport.Fail.Select(t => t.Id).Max();
                        Common.SetConfigValueByKey(cardTypeId1, maxId.ToString());
                        var newValue = new Tuple<List<string>, string, string>(_cardTypeRelation[cardTypeId1].Item1, maxId.ToString(), _cardTypeRelation[cardTypeId1].Item3);
                        _cardTypeRelation[cardTypeId1] = newValue;
                        count += resultImport.Fail.Count + resultImport.Success.Count;
                        countSuccess += resultImport.Success.Count;
                        var timeStamp3 = watcher.ElapsedMilliseconds;
                        rtxtInfo.Invoke(new Action(() => rtxtInfo.Text += $"新小微卡类型[ID:{cardTypeId1}已导入数据量为:{countSuccess}/{max}.本次用时{ timeStamp3}ms.\r\n"));//给界面提示，不做进度条
                        timeTotal += timeStamp3;
                        watcher.Restart();
                    }
                    //导入完成，删除键值对里对应的数据
                    watcher.Stop();
                    var timeStamp4 = watcher.ElapsedMilliseconds;
                    rtxtInfo.Invoke(new Action(() => rtxtInfo.Text += $"新小微卡类型[ID:{cardTypeId1}]导入完成.总用时{timeStamp4 + timeTotal}ms.\r\n"));
                    rtxt_data.Invoke(new Action(() => rtxt_data.Clear()));
                    keyValuePairs.Remove(cardTypeId1);
                }
                catch (Exception ex)
                {
                    Common.WriteLog(DateTime.Now.ToString("yyyyMMdd") + "ExceptionError.txt", $"导入新小微卡券异常错误.ErrorMessage:{ex.Message}");
                    keyValuePairs.Remove(cardTypeId1);
                    rtxtInfo.Invoke(new Action(() => rtxtInfo.Text += $"导入新小微发生了未知错误,详情见日志.\r\n"));
                }
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
        /// 取消对应卡类型的导入操作
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
        /// 获取旧小微卡类型
        /// </summary>
        /// <param name="token">旧小微token</param>
        /// <returns>返回旧小微卡类型集合，返回null表示请求失败并记录错误日志</returns>
        List<CardTypeOld> GetCardTypesOld(string token)
        {
            try
            {
                if (IsNullOrEmpty(token))
                {
                    Common.WriteLog(DateTime.Now.ToString("yyyyMMdd") + "Error.txt", $"获取旧小微卡类型参数错误.token:{token}");
                    return null;
                }
                var param = JsonConvert.SerializeObject(new RECardTypeOld { token = token });
                var resultStr = Common.Post(_urlGetCardType, param);
                var result = JsonConvert.DeserializeObject<ResponseCardTypeOld>(resultStr);
                if (result == null || result.Status != 1 || result.List == null)
                {
                    Common.WriteLog(DateTime.Now.ToString("yyyyMMdd") + "Error.txt", $"获取旧小微卡类型返回失败.ErrorMessage:{result?.Info}");
                    return null;
                }
                return result.List;
            }
            catch (Exception ex)
            {
                Common.WriteLog(DateTime.Now.ToString("yyyyMMdd") + "ExceptionError.txt", $"获取旧小微卡类型异常错误.ErrorMessage:{ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 获取旧小微指定卡类型下卡券的总数
        /// </summary>
        /// <param name="token">旧小微token</param>
        /// <param name="cardTypeId">旧小微卡类型ID</param>
        /// <returns>返回旧小微指定卡类型的卡券总数，返回-1表示请求失败并记录错误日志</returns>
        int GetCardCount(string token, string cardTypeId)
        {
            try
            {
                if (IsNullOrEmpty(token) || IsNullOrEmpty(cardTypeId))
                {
                    Common.WriteLog(DateTime.Now.ToString("yyyyMMdd") + "Error.txt", $"获取旧小微卡类型总数参数错误.token:{token} card_id:{cardTypeId}");
                    return -1;
                }
                var param = JsonConvert.SerializeObject(new RECardCountOld { token = token, card_id = cardTypeId });
                var resultStr = Common.Post(_urlGetCardTypeCount, param);
                var result = JsonConvert.DeserializeObject<ResponseCardCountOld>(resultStr);
                if (result == null || result.Status != 1)
                {
                    Common.WriteLog(DateTime.Now.ToString("yyyyMMdd") + "Error.txt", $"获取旧小微卡类型总数返回错误.ErrorMessage:{result?.Info}");
                    return -1;
                }
                return result.Total;
            }
            catch (Exception ex)
            {
                Common.WriteLog(DateTime.Now.ToString("yyyyMMdd") + "ExceptionError.txt", $"获取旧小微卡类型总数异常错误.ExceptionMessage:{ex.Message}");
                return -1;
            }
        }

        /// <summary>
        /// 获取旧小微卡券数据
        /// </summary>
        /// <param name="token">旧小微token</param>
        /// <param name="cardTypeId">旧小微卡类型ID</param>
        /// <param name="maxId">记录在本地的旧小微最大卡券ID</param>
        /// <returns>返回旧小微指定卡类型的卡券信息，返回null表示请求失败并记录错误日志</returns>
        List<CardOld> GetCardsOld(string token, string cardTypeId, string maxId, List<string> appoint_ids = null)
        {
            try
            {
                if (IsNullOrEmpty(token) || IsNullOrEmpty(cardTypeId) || (IsNullOrEmpty(maxId) && appoint_ids == null))
                {
                    Common.WriteLog(DateTime.Now.ToString("yyyyMMdd") + "Error.txt", $"获取旧小微卡券参数错误.token:{token} card_id:{cardTypeId} maxId:{maxId}");
                    return null;
                }

                var paramEntity = new RECardOld { token = token, card_id = cardTypeId, id = maxId };
                if (appoint_ids != null && appoint_ids.Count > 0)
                {
                    paramEntity.appoint_ids = appoint_ids;
                }
                var param = JsonConvert.SerializeObject(paramEntity);
                var resultStr = Common.Post(_urlGetCard, param);
                var result = JsonConvert.DeserializeObject<ResponseCardOld>(resultStr);
                if (result == null || result.Status != 1 || result.List == null)
                {
                    Common.WriteLog(DateTime.Now.ToString("yyyyMMdd") + "Error.txt", $"获取旧小微卡券返回错误.ErrorMessage:{result?.Info}");
                    return null;
                }
                return result.List;
            }
            catch (Exception ex)
            {
                Common.WriteLog(DateTime.Now.ToString("yyyyMMdd") + "ExceptionError.txt", $"获取旧小微卡券异常错误.ErrorMessage:{ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 获取新小微卡类型
        /// </summary>
        /// <param name="token">新小微token</param>
        /// <returns>返回新小微卡类型集合，返回null表示请求失败并记录错误日志</returns>
        List<CardTypeNew> GetCardTypesNew(string token)
        {
            try
            {
                if (IsNullOrEmpty(token))
                {
                    Common.WriteLog(DateTime.Now.ToString("yyyyMMdd") + "Error.txt", $"获取新小微卡类型参数错误.token:{token}");
                    return null;
                }
                var param = JsonConvert.SerializeObject(new RECardTypeNew { tokenKey = token });
                var resultStr = Common.Post(_urlGetCardTypeNew, param);
                var result = JsonConvert.DeserializeObject<ResponseCardTypeNew>(resultStr);
                if (result == null || result.Status != 1 || result.List == null)
                {
                    Common.WriteLog(DateTime.Now.ToString("yyyyMMdd") + "Error.txt", $"获取新小微卡类型返回错误.ErrorMessage:{result?.Info}");
                    return null;
                }
                return result.List;
            }
            catch (Exception ex)
            {
                Common.WriteLog(DateTime.Now.ToString("yyyyMMdd") + "ExceptionError.txt", $"获取新小微卡类型异常错误.ErrorMessage:{ex.Message}");
                return null;
            }
        }

        //获取当前卡类型导入失败的数据,没有则不做任何动作
        private void failDataMenuItem_Click(object sender, EventArgs e)
        {
            var cardTypeId = comboBox1.SelectedValue.ToString();
            if (IsNullOrEmpty(Common.GetConfigValue(cardTypeId + "_failids")))
            {
                return;
            }
            var idListString = Common.GetConfigValue(cardTypeId + "_failids");
            var idList = idListString.Split(';').ToList();
            idList.RemoveAll(IsNullOrEmpty);
            if (_cardTypeRelation.All(t => t.Key != cardTypeId)) return;
            var cardsOld = new List<CardOld>();
            var typeIdList = _cardTypeRelation[cardTypeId].Item1;
            foreach (var typeId in typeIdList)
            {
                var cards = GetCardsOld(_tokenOld, typeId, "", idList);
                if (cards == null) return;//获取数据失败直接返回
                if (cards.Count <= 0)
                {
                    continue;
                }
                cardsOld.AddRange(cards);
            }
            var converter = new IsoDateTimeConverter
            {
                DateTimeFormat = "yyyy-MM-dd"
            };
            rtxt_data.Invoke(new Action(() => rtxt_data.Text = $"{Common.ConvertJsonString(JsonConvert.SerializeObject(cardsOld, converter))}\r\n"));
            txtMaxId.Invoke(new Action(() => txtMaxId.Text = ""));
            txtCount.Invoke(new Action(() => txtCount.Text = cardsOld.Count.ToString()));
        }

        private Dictionary<string, bool> keyValuePairs1 = new Dictionary<string, bool>();

        private void importFailDataMenuItem_Click(object sender, EventArgs e)
        {
            var cardTypeId = comboBox1.SelectedValue.ToString();
            if (keyValuePairs1.Any(t => t.Key == cardTypeId && t.Value))
            {
                if (MessageBox.Show("当前卡类型正在导入失败数据中，请勿重复操作") == DialogResult.OK)
                {
                    return;
                }
            }
            else if (keyValuePairs1.Any(t => t.Key == cardTypeId && t.Value == false))
            {
                keyValuePairs1[cardTypeId] = true;
            }
            else
            {
                keyValuePairs1.Add(cardTypeId, true);
            }

            Task.Run(() =>
            {
                ///另起线程各自卡类型跑各自的
                ///1.获取数据
                ///2.导入数据
                ///
                var cardTypeId1 = cardTypeId;
                int count = 0, countSuccess = 0, total = 0;
                var watcher = new Stopwatch();
                watcher.Start();//计时器
                var timeTotal = 0L;
                #region 获取失败数据id
                if (IsNullOrEmpty(Common.GetConfigValue(cardTypeId1 + "_failids")))
                {
                    return;
                }
                var idListString = Common.GetConfigValue(cardTypeId1 + "_failids");
                var idList = idListString.Split(';').ToList();
                idList.RemoveAll(IsNullOrEmpty);
                total = idList.Count;
                if (_cardTypeRelation.All(t => t.Key != cardTypeId1)) return;
                var cardsOld = new List<CardOld>();
                var typeIdList = _cardTypeRelation[cardTypeId1].Item1;
                #endregion
                //直接删除原先失败的id
                Common.SetConfigValueByKey(cardTypeId1 + "_failids", "");
                try
                {
                    while (count <= total)
                    {
                        if (!keyValuePairs1[cardTypeId1])
                        {
                            //导入被取消了，给个提示并返回
                            watcher.Stop();
                            var timeStamp = watcher.ElapsedMilliseconds;

                            rtxtInfo.Invoke(new Action(() => rtxtInfo.Text += $"新小微卡类型[ID:{cardTypeId1}]导入已被取消。已经导入的数据量为:{count}/{total}.本次用时{timeTotal + timeStamp}ms.\r\n"));
                            rtxt_data.Invoke(new Action(() => rtxt_data.Clear()));
                            Common.SetConfigValueByKey(cardTypeId1+"_failids",idListString);
                            return;
                        }
                        #region 获取数据
                        foreach (var typeId in typeIdList)
                        {
                            var cards = GetCardsOld(_tokenOld, typeId, "", idList.Count > 1000 ? idList.GetRange(0, 999) : idList);
                            if (cards == null) return;//获取数据失败直接返回
                            if (cards.Count <= 0)
                            {
                                continue;
                            }
                            cardsOld.AddRange(cards);
                        }
                        #endregion

                        if (cardsOld.Count <= 0)
                        {
                            if (count > 0)
                            {
                                //循环出口
                                watcher.Stop();
                                var timeStamp1 = watcher.ElapsedMilliseconds;
                                rtxtInfo.Invoke(new Action(() => rtxtInfo.Text += $"新小微卡类型[ID:{cardTypeId1}]导入完成.总用时{timeTotal + timeStamp1}ms.\r\n"));
                                rtxt_data.Invoke(new Action(() => rtxt_data.Clear()));
                                keyValuePairs1.Remove(cardTypeId1);
                                return;
                            }
                            //没数据，给个提示
                            watcher.Stop();
                            var timeStamp2 = watcher.ElapsedMilliseconds;
                            rtxtInfo.Invoke(new Action(() => rtxtInfo.Text += $"新小微卡类型[ID:{cardTypeId1}]没有数据可导入.本次用时{timeStamp2}ms.\r\n"));
                            rtxt_data.Invoke(new Action(() => rtxt_data.Clear()));
                            keyValuePairs1.Remove(cardTypeId1);
                            return;
                        }
                        var cardsImport = AutoMapper.Mapper.Map<List<CardOld>, List<CardNew>>(cardsOld);
                        var resultImport = Import(_tokenNew, cardTypeId1, cardsImport);
                        if (resultImport == null)
                        {
                            rtxtInfo.Invoke(new Action(() => rtxtInfo.Text += $"新小微卡类型[ID:{cardTypeId1}]导入失败，详情见日志\r\n"));
                            rtxt_data.Invoke(new Action(() => rtxt_data.Clear()));
                            keyValuePairs1.Remove(cardTypeId1);
                            Common.SetConfigValueByKey(cardTypeId1 + "_failids", idListString);
                            return;//插入失败直接退出
                        }
                        var sb = new StringBuilder();
                        var sbStr = new StringBuilder();
                        idListString = Common.UpdateIdListStr(idListString, idList.Count > 1000 ? idList.GetRange(0, 999) : idList);
                        idList.RemoveRange(0, 1000);
                        foreach (var item in resultImport.Fail)
                        {
                            sb.Append($"id:{item.Id} failReason:{item.Reason}\r\n");
                            if (IsNullOrEmpty(sbStr.ToString()))
                            {
                                sbStr.Append(item.Id);
                                continue;
                            }
                            sbStr.Append(";" + item.Id);
                        }
                        if (sb.Length > 0)
                        {
                            Common.WriteLog(DateTime.Now.ToString("yyyyMMdd") + "ImportFailed.txt", sb.ToString());
                            if (IsNullOrEmpty(Common.GetConfigValue(cardTypeId1 + "_failids")))
                            {
                                Common.SetConfigValueByKey(cardTypeId1 + "_failids", sbStr.ToString());
                            }
                            else
                            {
                                var ddd = Common.GetConfigValue(cardTypeId1 + "_failids");
                                Common.SetConfigValueByKey(cardTypeId1 + "_failids", ddd + sbStr);
                            }
                        }

                        var sb1 = new StringBuilder();
                        foreach (var item in resultImport.Success)
                        {
                            sb1.Append($"id:{item}\r\n");
                        }
                        if (sb1.Length > 0)
                        {
                            Common.WriteLog(DateTime.Now.ToString("yyyyMMdd") + "ImportSuccess.txt", sb1.ToString());
                        }
                        count += resultImport.Fail.Count + resultImport.Success.Count;
                        countSuccess += resultImport.Success.Count;
                        var timeStamp3 = watcher.ElapsedMilliseconds;
                        rtxtInfo.Invoke(new Action(() => rtxtInfo.Text += $"新小微卡类型[ID:{cardTypeId1}已导入数据量为:{countSuccess}/{total}.本次用时{ timeStamp3}ms.\r\n"));//给界面提示，不做进度条
                        timeTotal += timeStamp3;
                        watcher.Restart();
                    }
                    //导入完成，删除键值对里对应的数据
                    watcher.Stop();
                    var timeStamp4 = watcher.ElapsedMilliseconds;
                    rtxtInfo.Invoke(new Action(() => rtxtInfo.Text += $"新小微卡类型[ID:{cardTypeId1}]导入完成.总用时{timeStamp4 + timeTotal}ms.\r\n"));
                    rtxt_data.Invoke(new Action(() => rtxt_data.Clear()));
                    keyValuePairs1.Remove(cardTypeId1);
                }
                catch (Exception ex)
                {
                    Common.WriteLog(DateTime.Now.ToString("yyyyMMdd") + "ExceptionError.txt", $"导入新小微卡券异常错误.ErrorMessage:{ex.Message}");
                    keyValuePairs1.Remove(cardTypeId1);
                    rtxtInfo.Invoke(new Action(() => rtxtInfo.Text += $"导入新小微发生了未知错误,详情见日志.\r\n"));
                }
            });
        }

        /// <summary>
        /// 导入新小微卡券信息
        /// </summary>
        /// <param name="token">新小微token</param>
        /// <param name="cardTypeId">卡类型ID</param>
        /// <param name="data">待导入的卡券数据</param>
        /// <returns>返回导入的信息，包括成功表示、成功和失败卡券的ID集合，返回null表示请求失败并记录错误日志</returns>
        ResponseImportCard Import(string token, string cardTypeId, List<CardNew> data)
        {
            try
            {
                if (IsNullOrEmpty(token) || IsNullOrEmpty(cardTypeId) || data == null)
                {
                    Common.WriteLog(DateTime.Now.ToString("yyyyMMdd") + "Error.txt", $"导入参数错误.token:{token} card_id:{cardTypeId} data:{data}");
                    return null;
                }

                var convert = new IsoDateTimeConverter()
                {
                    DateTimeFormat = "yyyy-MM-dd"
                };
                var param = JsonConvert.SerializeObject(new REImportCard { tokenKey = token, card_id = cardTypeId, data = data }, convert);
                var resultStr = Common.Post(_urlImportCard, param);
                var result = JsonConvert.DeserializeObject<ResponseImportCard>(resultStr);
                if (result == null || result.Status != 1)
                {
                    Common.WriteLog(DateTime.Now.ToString("yyyyMMdd") + "Error.txt", $"导入新小微卡券返回错误.ErrorMessage:{result?.Info}");
                    return null;
                }
                return result;
            }
            catch (Exception ex)
            {
                Common.WriteLog(DateTime.Now.ToString("yyyyMMdd") + "ExceptionError.txt", $"导入新小微卡券返回错误.ErrorMessage:{ex.Message}");
                return null;
            }
        }
    }
}
