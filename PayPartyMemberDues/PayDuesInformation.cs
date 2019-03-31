using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;

namespace PayPartyMemberDues
{
    public class PayDuesInformation
    {        
        public string BranchName { set => _branchName = value; }
        public bool IsAllowLogin => _currentPartyBranchInfos.IsAllowLogin;

        public PartyBranchInformations CurrentPartyBranchInfos => _currentPartyBranchInfos;

        //配置文件位置
        private readonly string _configurationUrl = @"https://raw.githubusercontent.com/GalensGan/PartyPay/master/PayPartyMemberDues/ConfigurationFile/PartyPayconfiguration.xml";

        
        //支部名称
        private string _branchName = null;

        //程序配置信息
        private XmlDocument _configDoc = new XmlDocument();
        //支部配置文件
        private XmlDocument _branchConfigDoc = new XmlDocument();
        //支部信息文件
        private PartyBranchInformations _currentPartyBranchInfos = null;

        /// <summary>
        /// 根据支部名称加载各个支部的数据地址以及党员信息
        /// </summary>
        /// <param name="branchName"></param>
        public void LoadBranchConfig(string branchName)
        {
            //保存支部名称
            _branchName = branchName;

            //获取所有的节点数据
            XmlNodeList xmlNodeList = _configDoc.SelectNodes("/PartyBranches/PartyBranch[@name='"+branchName+"']");
            if (xmlNodeList.Count == 0) return;
            xmlNodeList = xmlNodeList[0].ChildNodes;
            string config = null;
            foreach (XmlNode node in xmlNodeList)
            {
                if (node.Name == "clientAddress")
                {
                    config = node.InnerText;
                    break;
                }
            }
            config = CommonSDK.DownLoadText(config);
            
            //从程序配置文件加载支部配置文件
            _branchConfigDoc.LoadXml(config);
            
            //加载党员信息
            LoadBranchInformations(branchName);
        }

        //支部党员信息
        private Dictionary<string, PartyBranchInformations> _allPartyInfoDoc = new Dictionary<string, PartyBranchInformations>();
        /// <summary>
        /// 根据支部名称，加载各个支部的党员信息，采用享元模式，避免多次加载
        /// </summary>
        /// <param name="branchName"></param>
        private void LoadBranchInformations(string branchName)
        {
            if (_allPartyInfoDoc.ContainsKey(branchName)) _currentPartyBranchInfos = _allPartyInfoDoc[branchName];
            else
            {
                //加载
                _currentPartyBranchInfos = new PartyBranchInformations(_branchConfigDoc);
                _allPartyInfoDoc.Add(branchName, _currentPartyBranchInfos);
            }
        }    

        /// <summary>
        /// 下载配置文件
        /// </summary>
        public void LoadConfigurationFile()
        {
            //不同的支部名称对应不同的地址
            //下载配置文件           
            string config = CommonSDK.DownLoadText(_configurationUrl);
            _configDoc.LoadXml(config);
        }
        

        /// <summary>
        /// 获取所有支部的名称
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllBranchNames()
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
        /// 验证id是否能通过
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CheckId(string id)
        {
            return _currentPartyBranchInfos.CheckId(id);
        }

        /// <summary>
        /// 二维码类型
        /// </summary>
        public enum PayQRCodeType
        {
            Wechat,
            AliPay
        }

        /// <summary>
        /// 获取支付二维码
        /// </summary>
        /// <param name="qRCodeType"></param>
        /// <returns></returns>
        public Image GetPayQRCode(PayQRCodeType qRCodeType)
        {
          return  _currentPartyBranchInfos.GetPayQRCode(qRCodeType);
        }
    }
}
