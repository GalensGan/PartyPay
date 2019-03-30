using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace PayPartyMemberDues
{
    public partial class LogInForm : Form
    {
        PayDuesInformation _payDuesInfos = null;
        public LogInForm(PayDuesInformation info)
        {            
            InitializeComponent();
            _payDuesInfos = info;
            LoadConfiguration();
        }

        //根据程序的配置文件，下载所有的党支部名称
        public void LoadConfiguration()
        {
            //获取本地文件
            string localPath = System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\PartyPay\userConfig.txt";
            //存在时，直接调用用户的数据
            if (File.Exists(localPath))
            {
                StreamReader reader = new StreamReader(localPath);
                string branchNmae = reader.ReadLine();
                string idStr = reader.ReadLine();
                comboBox1.Items.Clear();
                comboBox1.Items.Add(branchNmae);
                comboBox1.SelectedIndex = 0;
                textBox1.Text = idStr;
                reader.Close();
            }
            else
            {
                //获取所有的节点数据
                comboBox1.Items.Clear();
                comboBox1.Items.AddRange(_payDuesInfos.GetAllBranchNames().ToArray());
            }            
        }

        //登陆操作
        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex < 0) MessageBox.Show("请选择支部名称");
            else
            {                
                //先进行用户验证
                if (!_payDuesInfos.CheckId(textBox1.Text)) return;

                //激活主窗体
                PayForm form = new PayForm(_payDuesInfos);
                form.FormClosed += CloseForm;
                this.Hide();                
                form.Show();
                //覆盖本地的基本信息文件
                SaveUserData();
            }
        }

        //保存个人基本信息
        private void SaveUserData()
        {
            string localPath = System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\PartyPay\userConfig.txt";
            System.IO.File.WriteAllText(localPath, comboBox1.Text+"\n"+textBox1.Text);
        }

        //实现子窗体关闭，父窗体也关闭
        private void CloseForm(object sender, EventArgs e)
        {
            this.Close();
        }

        //根据不同的支部名称，加载不同的党员数据文件
        private int lastIndex = -1;
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lastIndex != comboBox1.SelectedIndex)
            {
                _payDuesInfos.LoadBranchConfig(comboBox1.Text);
                lastIndex = comboBox1.SelectedIndex;
            }
        }
    }
}
