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
        DownLoadInformation _partyInfos = null;
        public PayForm(DownLoadInformation info)
        {
            InitializeComponent();
            _partyInfos = info;
            info.BeginDownLoadInfo();
            pictureBox1.Image = info.GetPayQRCode(DownLoadInformation.PayQRCodeType.Wechat);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        }
        
    }
}
