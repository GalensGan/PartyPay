using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Xml;

namespace PayPartyMemberDues
{
    public class PartyBranchInformations
    {
        //是否允许登陆
        private readonly bool _isAllowLogin = false;
        //信息文件
        private readonly string _infoUrl = null;
        //二维码位置文件
        private readonly string _weChatCode = null;
        private readonly string _aliPayCode = null;

        //支付二维码
        private Dictionary<PayDuesInformation.PayQRCodeType, Image> _payCodeDic = new Dictionary<PayDuesInformation.PayQRCodeType, Image>();
        //党员信息表
        private XmlDocument _partyInfosDoc = new XmlDocument();

        //当前党员信息
        private APartyMemberInfo _currentPartyInfo = null;

        public bool IsAllowLogin => _isAllowLogin;

        public APartyMemberInfo CurrentPartyInfo => _currentPartyInfo;

        /// <summary>
        /// 用ID激活当前党员的信息
        /// </summary>
        /// <param name="id"></param>
        public void ActiveCurrentPartyInfo(string id)
        {
            _currentPartyInfo = new APartyMemberInfo
            {
                Id = id
            };

            //获取所有的节点数据
            XmlNodeList xmlNodeList = _partyInfosDoc.SelectNodes("/PartyInfos/PartyMember[@id='" + id + "']");
            if (xmlNodeList.Count == 0) return;
            xmlNodeList = xmlNodeList[0].ChildNodes;
            foreach (XmlNode node in xmlNodeList)
            {
                if (node.Name == "Name")
                {
                    _currentPartyInfo.Name = node.InnerText;
                    continue;
                }
                if (node.Name == "Gender")
                {
                    _currentPartyInfo.Gender = node.InnerText;
                    continue;
                }
                if (node.Name == "DuesPerMonth")
                {
                    _currentPartyInfo.DuesPerMonth = double.Parse(node.InnerText);
                    continue;
                }
                if (node.Name == "AppPhone")
                {
                    _currentPartyInfo.AppPhoneNumber = node.InnerText;
                    continue;
                }
                if (node.Name == "Department")
                {
                    _currentPartyInfo.Department = node.InnerText;
                    continue;
                }
            }

            //获取月份信息
            XmlNodeList xmlNodeList2 = _partyInfosDoc.SelectNodes("/PartyInfos/MonthInfo");
            if (xmlNodeList2.Count == 0) return;
            xmlNodeList2 = xmlNodeList2[0].ChildNodes;
            foreach (XmlNode node in xmlNodeList)
            {
                if (node.Name == "Count")
                {
                    _currentPartyInfo.MonthCount =double.Parse(node.InnerText);
                    continue;
                }
                if (node.Name == "Start")
                {
                    _currentPartyInfo.StartMonth = node.InnerText;
                    continue;
                }
                if (node.Name == "End")
                {
                    _currentPartyInfo.EndMonth = node.InnerText;
                    continue;
                }
            }
        }

        public PartyBranchInformations(XmlDocument branchConfig)
        {
            //获取所有的节点数据
            XmlNodeList xmlNodeList = branchConfig.SelectNodes("/PartyBranches/PartyBranch");
            xmlNodeList = xmlNodeList[0].ChildNodes;
            //获取所有的地址信息
            foreach (XmlNode node in xmlNodeList)
            {
                if (node.Name == "IsAllowLogin")
                {
                    if (xmlNodeList[0].InnerText == "true") _isAllowLogin = true;
                    else _isAllowLogin = false;
                    continue;
                }
                if (node.Name == "InfoAddress")
                {
                    _infoUrl = node.InnerText;
                    continue;
                }
                if (node.Name == "WeChatQRCodeAddress")
                {
                    _weChatCode = node.InnerText;
                    continue;
                }
                if (node.Name == "AliPayQRCodeAddress")
                {
                    _aliPayCode = node.InnerText;
                    continue;
                }
            }
            //加载信息文件
            LoadPartyInfo();
        }

        //加载信息文件
        private void LoadPartyInfo()
        {
            //下载信息表
            string str = CommonSDK.DownLoadText(_infoUrl);
            _partyInfosDoc.LoadXml(str);

            //下载微信支付
            if (_weChatCode.Contains("http"))
            {
                Image wechatImage = CommonSDK.DownLoadImage(_weChatCode);
                _payCodeDic.Add(PayDuesInformation.PayQRCodeType.Wechat, wechatImage);
            }

            //下载支付宝支付
            if (_aliPayCode.Contains("http"))
            {
                Image aliPayImage = CommonSDK.DownLoadImage(_aliPayCode);
                _payCodeDic.Add(PayDuesInformation.PayQRCodeType.AliPay, aliPayImage);
            }
        }

        /// <summary>
        /// 获取支付二维码
        /// </summary>
        /// <param name="qrName"></param>
        /// <returns></returns>
        public Image GetPayQRCode(PayDuesInformation.PayQRCodeType qrName)
        {
            if (_payCodeDic.ContainsKey(qrName)) return _payCodeDic[qrName];
            else if (_payCodeDic.Count > 0) return _payCodeDic.FirstOrDefault().Value;
            else return null;
        }

        /// <summary>
        /// 获取二维码的个数
        /// </summary>
        public int PayQRCodeCount
        {
            get
            {
              return _payCodeDic.Count;
            }
        }

        /// <summary>
        /// 输入身份证检查是否在数据库中
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CheckId(string id)
        {
            //获取所有的节点数据
            XmlNodeList xmlNodeList = _partyInfosDoc.SelectNodes("/PartyInfos/PartyMember[@id='" + id + "']");
            if (xmlNodeList.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("在当前党支部中未能查询到您的信息，登陆失败。");
                return false;
            }
            else return true;
        }

    }
}
