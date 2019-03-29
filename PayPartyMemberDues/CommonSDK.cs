using System.Runtime.InteropServices;
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
    }
}
