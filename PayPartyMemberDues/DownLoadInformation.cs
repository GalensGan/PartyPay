using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;

namespace PayPartyMemberDues
{
    public class DownLoadInformation
    {
        //支付二维码
        private Dictionary<string, Image> _payCodeDic = new Dictionary<string, Image>();
        //指示是否下载完成
        private readonly bool _isEndDownLoad = false;
        public bool IsEndDownLoad => _isEndDownLoad;

        public string InfoUrl { set => _infoUrl = value; }
        public string WeChatCode { set => _weChatCode = value; }
        public string AliPayCode { set => _aliPayCode = value; }
        public string BranchName { set => _branchName = value; }

        //配置文件位置
        private readonly string _configurationUrl = @"https://raw.githubusercontent.com/GalensGan/PartyPay/master/PayPartyMemberDues/ConfigurationFile/PartyPayconfiguration.xml";

        //信息文件
        private string _infoUrl = null;
        //二维码位置文件
        private string _weChatCode = null;
        private string _aliPayCode = null;
        //支部名称
        private string _branchName = null;

        //配置信息
        private XmlDocument _configDoc = new XmlDocument();
        //信息文件
        private  XmlDocument _partyInfoDoc = new XmlDocument();


        /// <summary>
        /// 开始下载主要文件
        /// </summary>
        public void DownLoadMainInfo()
        {
            GetPartyInfoDirectory(_branchName);
            //下载信息表
            string str = DownLoadText(_infoUrl);
            _partyInfoDoc.LoadXml(str);
            //下载微信支付
            Image wechatImage = DownLoadImage(_weChatCode);
            _payCodeDic.Add("WeChat", wechatImage);
            //下载支付宝支付
            Image aliPayImage = DownLoadImage(_aliPayCode);
            _payCodeDic.Add("AliPay", aliPayImage);
        }

        /// <summary>
        /// 获取支部的各个路径
        /// </summary>
        /// <param name="partyBranchName"></param>
        private void GetPartyInfoDirectory(string branchName)
        {
            //获取所有的节点数据
            XmlNodeList xnl = _configDoc.SelectNodes("/PartyBranches/PartyBranch[@name='" + branchName + "']");
            foreach (XmlNode node in xnl)
            {
                if (node.Attributes["type"].Value == "InfoAddress") _infoUrl = node.Value;
                if (node.Attributes["type"].Value == "WeChatQRCodeAddress") _weChatCode= node.Value;
                if (node.Attributes["type"].Value == "AliPayQRCodeAddress") _aliPayCode = node.Value;
            }
        }

        /// <summary>
        /// 下载配置文件
        /// </summary>
        public void DownLoadConfigurationFile()
        {
            //不同的支部名称对应不同的地址
            //下载配置文件           
            string config = DownLoadText(_configurationUrl);
            _configDoc.LoadXml(config);
        }

        /// <summary>
        /// 检查本地状态
        /// </summary>
        /// <returns></returns>
        public bool CheckLocalEnvironment()
        {
            //获取本地文件
            string localPath = System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"PartyPay\userConfig.txt";
            //不存在时，直接返回false
            if (!File.Exists(localPath)) return false;

            StreamReader reader = new StreamReader(localPath);
            string branchNmae = reader.ReadLine();
            //获取所有的节点数据
            XmlNodeList xnl = _configDoc.SelectNodes("/PartyBranches/PartyBranch");
            foreach (XmlNode node in xnl)
            {
                if (node.Attributes["name"].Value == branchNmae)
                {
                    _branchName = branchNmae;
                    return true;
                } 
            }
            return false;
        }

        /// <summary>
        /// 获取所有支部的名称
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllPartyNames()
        {
            List<string> returnList = new List<string>();
            //获取所有的节点数据
            XmlNodeList xnl = _configDoc.SelectNodes("/PartyBranches/PartyBranch");
            foreach (XmlNode node in xnl)
            {
                returnList.Add(node.Attributes["name"].Value);
            }
            return returnList;
        }

        /// <summary>
        /// 获取支付二维码
        /// </summary>
        /// <param name="qrName"></param>
        /// <returns></returns>
        public Image GetPayQRCode(PayQRCodeType qrName)
        {
            if (_payCodeDic.ContainsKey(qrName.ToString())) return _payCodeDic[qrName.ToString()];
            else return null;
        }

        /// <summary>
        /// 二维码类型
        /// </summary>
        public enum PayQRCodeType
        {
            Wechat,
            AliPay
        }

        //下载文本文件
        private string DownLoadText(string url)
        {
            WebClient webClient = new WebClient
            {
                Encoding = Encoding.UTF8
            };
            //这里使用DownloadString方法，如果是不需要对文件的文本内容做处理，直接保存，那么可以直接使用功能DownloadFile(url,savepath)直接进行文件保存。
            string outText = webClient.DownloadString(url);
            return outText;
        }

        //下载图片
        private Image DownLoadImage(string imgurl)
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
