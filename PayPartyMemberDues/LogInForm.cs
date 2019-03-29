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
    public partial class LogInForm : Form
    {
        DownLoadInformation _partyInfos = null;
        public LogInForm(DownLoadInformation info)
        {            
            InitializeComponent();
            _partyInfos = info;
        }
    }
}
