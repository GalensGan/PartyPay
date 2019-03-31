using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PayPartyMemberDues
{
    public partial class PayForm : Form
    {
        PartyBranchInformations _partyBranchDuesInfos = null;
        public PayForm(PartyBranchInformations info)
        {
            InitializeComponent();

            //赋予初值
            _partyBranchDuesInfos = info;
            pictureBox1.Image = info.GetPayQRCode(PayDuesInformation.PayQRCodeType.Wechat);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;

            //写入基本信息
            InitializeListView(listView1);
        }

        //写入党员的基本信息
        private void InitializeListView(ListView lv)
        {
            //写入欢迎信息
            WelcomLabel.Text = _partyBranchDuesInfos.CurrentPartyInfo.WelcomeString;

            //设置属性
            lv.GridLines = true;  //显示网格线
            lv.FullRowSelect = true;  //显示全行
            lv.MultiSelect = false;  //设置只能单选
            lv.View = View.Details;  //设置显示模式为详细
            lv.HoverSelection = true;  //当鼠标停留数秒后自动选择

            //把列名添加到listview中
            lv.Columns.Add("姓  名",51,HorizontalAlignment.Center);
            lv.Columns.Add("身份证", 153, HorizontalAlignment.Center);
            lv.Columns.Add("党费基数", 68, HorizontalAlignment.Center);
            lv.Columns.Add("党费月份", 120, HorizontalAlignment.Center);
            lv.Columns.Add("合计(元)", 60, HorizontalAlignment.Center);

            APartyMemberInfo info = _partyBranchDuesInfos.CurrentPartyInfo;
            ListViewItem liRow = new ListViewItem(info.Name);
            liRow.SubItems.Add(info.Id);
            liRow.SubItems.Add(info.DuesPerMonth.ToString());
            liRow.SubItems.Add(info.Date);
            liRow.SubItems.Add(info.TotalMoney.ToString());
            lv.Items.Add(liRow);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0) pictureBox1.Image = _partyBranchDuesInfos.GetPayQRCode(PayDuesInformation.PayQRCodeType.AliPay);
            if (comboBox1.SelectedIndex == 1) pictureBox1.Image = _partyBranchDuesInfos.GetPayQRCode(PayDuesInformation.PayQRCodeType.Wechat);
        }
    }
}
