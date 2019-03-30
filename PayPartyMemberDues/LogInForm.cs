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
        DownLoadInformation _partyInfos = null;
        public LogInForm(DownLoadInformation info)
        {            
            InitializeComponent();
            _partyInfos = info;
            CheckLocalEnvironment();
        }

        // 检查本地状态
        public void CheckLocalEnvironment()
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
                comboBox1.Items.AddRange(_partyInfos.GetAllPartyNames().ToArray());
            }            
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex < 0) MessageBox.Show("请选择支部名称");
            else
            {
                _partyInfos.BranchName = comboBox1.Text;
                
                //先进行用户验证
                if (!UserAuthentication()) return;
                //激活主窗体
               
                //覆盖本地的基本信息文件
                PayForm form = new PayForm(_partyInfos);
                form.FormClosed += CloseForm;
                this.Hide();                
                form.Show();
            }
        }

        //保存个人基本信息
        private void SaveUserData()
        {
            string localPath = System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\PartyPay\userConfig.txt";
            System.IO.File.WriteAllText(localPath, comboBox1.Text);
        }

        //用户验证
        private bool UserAuthentication()
        {
            List<string> namesList = _partyInfos.GetAllIds(comboBox1.Text);
            if (namesList == null)
            {
                MessageBox.Show("不存在当前党支部名称");
                return false;
            }
            foreach (string str in namesList)
            {
                if (str == textBox1.Text) return true;                   
            }
            MessageBox.Show("您不属于当前党支部");
            return false;
        }

        private void CloseForm(object sender, EventArgs e)
        {
            this.Close();
        }

        private int lastIndex = -1;
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lastIndex != comboBox1.SelectedIndex)
            {
                _partyInfos.DownLoadMainInfo();
                lastIndex = comboBox1.SelectedIndex;
            }
        }
    }
}
