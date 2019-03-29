using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace PayPartyMemberDues
{
    public partial class LogInForm : Form
    {
        DownLoadInformation _partyInfos = null;
        public LogInForm(DownLoadInformation info)
        {            
            InitializeComponent();
            _partyInfos = info;
            //获取所有的节点数据
            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(_partyInfos.GetAllPartyNames().ToArray());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex < 0) MessageBox.Show("请选择支部名称");
            else
            {
                PayForm form = new PayForm(_partyInfos);
                form.FormClosed += CloseForm;
                this.Hide();
                form.Show();
            }
        }

        private void CloseForm(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
