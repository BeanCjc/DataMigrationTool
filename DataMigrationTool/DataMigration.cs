using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataMigrationTool.Old;

namespace DataMigrationTool
{
    public partial class DataMigration : Form
    {
        public DataMigration()
        {
            InitializeComponent();
        }

        private bool _flag;
        private Dictionary<string, string> _cardTypeAndMaxCardId=new Dictionary<string, string>();

        private void rtxt_data_TextChanged(object sender, EventArgs e)
        {
            rtxt_data.SelectionStart = rtxt_data.Text.Length;
            rtxt_data.ScrollToCaret();
        }

        private void DataMigration_Load(object sender, EventArgs e)
        {
            var dataSource = new List<CardTypeOld>
            {
                new CardTypeOld {Card_Name = "A类", Id = "0", Type = "0"},
                new CardTypeOld {Card_Name = "B类", Id = "1", Type = "1"},
                new CardTypeOld {Card_Name = "C类", Id = "2", Type = "1"}
            };
            comboBox1.DataSource = dataSource;
            comboBox1.DisplayMember = "Card_Name";
            comboBox1.ValueMember = "Id";
            comboBox1.SelectedValue = "2";
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            _flag = true;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_flag)
            {
                var cardTypeId = comboBox1.SelectedValue.ToString();

            }
        }
    }
}
