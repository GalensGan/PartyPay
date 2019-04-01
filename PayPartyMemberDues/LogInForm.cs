using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace PayPartyMemberDues
{
    public partial class LogInForm : Form
    {
        private PayDuesInformation _payDuesInfos = null;
        private List<string> _branchList = new List<string>();
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
            //获取所有的分支名称
            List<string> branchesNameList = _payDuesInfos.GetAllBranchNames();
            //存在时，直接调用用户的数据
            if (File.Exists(localPath))
            {
                StreamReader reader = new StreamReader(localPath);
                string branchNmae = reader.ReadLine();
                if (branchesNameList.Contains(branchNmae))
                {
                    _branchList.Add(branchNmae);
                    string idStr = reader.ReadLine();
                    comboBox1.Items.Clear();
                    comboBox1.Items.Add(branchNmae);
                    comboBox1.SelectedIndex = 0;
                    textBox1.Text = idStr;
                    reader.Close();
                    _isAllowSearch = true;
                    return;
                }
                reader.Close();
            }
            //获取所有的节点数据
            comboBox1.Items.Clear();
            _branchList.AddRange(_payDuesInfos.GetAllBranchNames());
            comboBox1.Items.AddRange(_branchList.ToArray());
            _isAllowSearch = true;
        }

        //登陆操作
        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex < 0) MessageBox.Show("请选择支部名称");
            else
            {
                //先进行用户验证
                if (!_payDuesInfos.CheckId(textBox1.Text)) return;

                //激活当前的党员信息
                _payDuesInfos.CurrentPartyBranchInfos.ActiveCurrentPartyInfo(textBox1.Text);

                //激活主窗体
                PayForm form = new PayForm(_payDuesInfos.CurrentPartyBranchInfos);

                form.FormClosed += CloseForm;
                Hide();
                form.Show();
                //覆盖本地的基本信息文件
                SaveUserData();
            }
        }

        //保存个人基本信息
        private void SaveUserData()
        {
            string folderPath = System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            // 创建文件夹
            string sPath = folderPath + "\\PartyPay";
            if (!Directory.Exists(sPath))
            {
                Directory.CreateDirectory(sPath);
            }
            string localPath = sPath + "\\userConfig.txt";
            FileStream fs = new FileStream(localPath, FileMode.Create, FileAccess.ReadWrite); //可以指定盘符，也可以指定任意文件名，还可以为word等文件
            StreamWriter sw = new StreamWriter(fs); // 创建写入流
            sw.Write(comboBox1.Text + "\n" + textBox1.Text);
            sw.Close(); //关闭文件
        }

        //实现子窗体关闭，父窗体也关闭
        private void CloseForm(object sender, EventArgs e)
        {
            Close();
        }

        //根据不同的支部名称，加载不同的党员数据文件
        private int lastIndex = -1;
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lastIndex != comboBox1.SelectedIndex)
            {
                _payDuesInfos.LoadBranchConfig(comboBox1.Text);
                lastIndex = comboBox1.SelectedIndex;
                if (_payDuesInfos.IsAllowLogin)
                {
                    button1.Enabled = true;
                    button1.Text = "登陆";
                    button1.ForeColor = Color.Black;
                }
                else
                {
                    button1.Enabled = false;
                    button1.Text = "暂停服务";
                    button1.ForeColor = Color.Red;
                }
            }
        }

        //实现搜索功能
        private bool _isAllowSearch = false;
        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            //if (!_isAllowSearch) return;
            //List<string> newItems = new List<string>();
            //foreach (string item in _branchList)
            //{
            //    if (item.Contains(textBox1.Text)) newItems.Add(item);
            //}
            //comboBox1.Items.Clear();
            //comboBox1.Items.AddRange(newItems.ToArray());
            //Cursor = Cursors.Default;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private const int WM_NCHITTEST = 0x84;
        private const int HTCLIENT = 0x1;
        private const int HTCAPTION = 0x2;
        //重写窗体，使窗体可以不通过自带标题栏实现移动
        protected override void WndProc(ref Message m)
        {
            //当重载窗体的 WndProc 方法时，可以截获 WM_NCHITTEST 消息并改些该消息， 
            //当判断鼠标事件发生在客户区时，改写改消息，发送 HTCAPTION 给窗体， 
            //这样，窗体收到的消息就时 HTCAPTION ，在客户区通过鼠标来拖动窗体就如同通过标题栏来拖动一样。 
            //注意：当你重载 WndProc 并改写鼠标事件后，整个窗体的鼠标事件也就随之改变了。 
            switch (m.Msg)
            {
                case WM_NCHITTEST:
                    base.WndProc(ref m);
                    if ((int)m.Result == HTCLIENT)
                        m.Result = (IntPtr)HTCAPTION;
                    return;
            }
            base.WndProc(ref m);
        }
    }
}
