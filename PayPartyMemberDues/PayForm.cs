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
        PayDuesInformation _partyDuesInfos = null;
        public PayForm(PayDuesInformation info)
        {
            InitializeComponent();

            //赋予初值
            _partyDuesInfos = info;
            pictureBox1.Image = info.GetPayQRCode(PayDuesInformation.PayQRCodeType.Wechat);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;

            //写入基本信息
            InitializeListView(listView1);
        }

        //写入党员的基本信息
        private void InitializeListView(ListView lv)
        {
            //添加列名
            ColumnHeader c1 = new ColumnHeader
            {
                Width = 40,
                Text = "姓名"
            };
            ColumnHeader c2 = new ColumnHeader
            {
                Width = 100,
                Text = "身份证"
            };
            ColumnHeader c3 = new ColumnHeader
            {
                Width = 100,
                Text = "每月缴费基数"
            };
            ColumnHeader c4 = new ColumnHeader
            {
                Width = 100,
                Text = "上缴月份"
            };
            ColumnHeader c5 = new ColumnHeader
            {
                Width = 100,
                Text = "合计"
            };
            //设置属性
            lv.GridLines = true;  //显示网格线
            lv.FullRowSelect = true;  //显示全行
            lv.MultiSelect = false;  //设置只能单选
            lv.View = View.Details;  //设置显示模式为详细
            lv.HoverSelection = true;  //当鼠标停留数秒后自动选择
            //把列名添加到listview中
            lv.Columns.Add(c1);
            lv.Columns.Add(c2);
            lv.Columns.Add(c3);
            lv.Columns.Add(c4);
            lv.Columns.Add(c5);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0) pictureBox1.Image = _partyDuesInfos.GetPayQRCode(PayDuesInformation.PayQRCodeType.AliPay);
            if (comboBox1.SelectedIndex == 1) pictureBox1.Image = _partyDuesInfos.GetPayQRCode(PayDuesInformation.PayQRCodeType.Wechat);
        }
    }
}
