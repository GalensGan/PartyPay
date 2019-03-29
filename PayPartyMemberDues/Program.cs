using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PayPartyMemberDues
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            //判断联网
            if (!CommonSDK.IsInternetConnect()) return;

            DownLoadInformation info = new DownLoadInformation();
            info.DownLoadConfigurationFile();

            //读取本地支部数据，如果没有，则从服务器上下载
            //如果有，则对比服务器上的支部

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LogInForm(info));



        }
    }
}
