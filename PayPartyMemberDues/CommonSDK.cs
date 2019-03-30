using System.Drawing;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace PayPartyMemberDues
{
    internal class CommonSDK
    {
        [DllImport("wininet")]
        //判断网络状况的方法,返回值true为连接，false为未连接  
        public static extern bool InternetGetConnectedState(out int conState, int reder);

        public static bool IsInternetConnect()
        {
            if ((InternetGetConnectedState(out int i, 0) == true))
            {
                //MessageBox.Show("设备联网正常！");
                return true;
            }
            else
            {
                MessageBox.Show("您的电脑未连接网络！");
                return false;
            }
        }

        /// <summary>
        /// 下载文本文件
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string DownLoadText(string url)
        {
            WebClient webClient = new WebClient
            {
                Encoding = Encoding.UTF8
            };
            //这里使用DownloadString方法，如果是不需要对文件的文本内容做处理，直接保存，那么可以直接使用功能DownloadFile(url,savepath)直接进行文件保存。
            string outText = webClient.DownloadString(url);
            return outText;
        }

        /// <summary>
        /// 下载图片
        /// </summary>
        /// <param name="imgurl"></param>
        /// <returns></returns>
        public static Image DownLoadImage(string imgurl)
        {
            WebClient webClient = new WebClient();
            byte[] imgData = webClient.DownloadData(imgurl);
            Stream ms = new MemoryStream(imgData)
            {
                Position = 0
            };
            Image img = Image.FromStream(ms);
            return img;
        }
    }
}
