using System;
using System.Collections.Generic;
using System.Data;
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

        private  bool IsSystemActive = false;
        //配置文件位置
        private string _configurationUrl = @"https://nj02cm01.baidupcs.com/file/017012abfc3741d1e9c2fee89f3063de?bkt=p3-1400017012abfc3741d1e9c2fee89f3063de7ede14b50000000008bd&fid=3291265560-250528-940630852061810&time=1553874588&sign=FDTAXGERLQBHSKfW-DCb740ccc5511e5e8fedcff06b081203-KAQCurvoewUBG67glHqnxu1bg5Q%3D&to=88&size=2237&sta_dx=2237&sta_cs=1&sta_ft=xml&sta_ct=0&sta_mt=0&fm2=MH%2CQingdao%2CAnywhere%2C%2Cbeijing%2Ccmnet&ctime=1553873391&mtime=1553873391&resv0=cdnback&resv1=0&vuk=3291265560&iv=0&htype=&newver=1&newfm=1&secfm=1&flow_ver=3&pkey=1400017012abfc3741d1e9c2fee89f3063de7ede14b50000000008bd&sl=68616270&expires=8h&rt=sh&r=757174239&mlogid=2040804003527599748&vbdid=3132365251&fin=PartyPayconfiguration.xml&fn=PartyPayconfiguration.xml&rtype=1&dp-logid=2040804003527599748&dp-callid=0.1.1&hps=1&tsl=200&csl=200&csign=MeYL%2B%2FVzaV5EHCvsuS%2BuVUH5eFU%3D&so=0&ut=6&uter=4&serv=0&uc=2548494708&ti=76168191086d6f29c506b09cb16030a575dd39cf8651408d&by=themis";

        //信息文件
        private string _infoUrl = null;
        //二维码位置文件
        private string _weChatCode = null;
        private string _aliPayCode = null;

        //配置信息
        private XmlDocument _configDoc = new XmlDocument();
        //信息文件
        private XmlDocument _partyInfoDoc = new XmlDocument();


        /// <summary>
        /// 开始下载主要文件
        /// </summary>
        public void BeginDownLoadInfo()
        {
            string str = DownLoadText(_infoUrl);
            Image wechatImage = DownLoadImage(_weChatCode);
            _payCodeDic.Add("weChat", wechatImage);
        }

        /// <summary>
        /// 下载配置文件
        /// </summary>
        public void DownLoadConfigurationFile()
        {
            //不同的支部名称对应不同的地址
            //下载配置文件
            _configurationUrl = @"https://raw.githubusercontent.com/GalensGan/ArticleSourceCode/master/ArticleSourceCode/ArticleSourceCode.csproj";
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
            XmlNodeList xnl = _configDoc.SelectNodes("/PartyBranches");
            foreach (XmlNode node in xnl)
            {
                if (node.Attributes["Name"].Value == branchNmae) return true;
            }
            return false;
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
            WebClient webClient = new WebClient();
            webClient.Encoding = Encoding.UTF8;
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
