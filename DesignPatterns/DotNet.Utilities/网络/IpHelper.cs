/*
 源码己托管:https://github.com/DomoYao
 */

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Web;

namespace DotNet.Utilities
{
    /// <summary>
    /// 共用工具类
    /// </summary>
    public static class IpHelper
    {
        #region 获得用户IP
        /// <summary>
        /// 获得用户IP
        /// </summary>
        public static string GetUserIp()
        {
            string ip;
            string[] temp;
            bool isErr = false;
            if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_ForWARDED_For"] == null)
                ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            else
                ip = HttpContext.Current.Request.ServerVariables["HTTP_X_ForWARDED_For"];
            if (ip.Length > 15)
                isErr = true;
            else
            {
                temp = ip.Split('.');
                if (temp.Length == 4)
                {
                    for (int i = 0; i < temp.Length; i++)
                    {
                        if (temp[i].Length > 3) isErr = true;
                    }
                }
                else
                    isErr = true;
            }

            if (isErr)
                return "1.1.1.1";
            else
                return ip;
        }
        #endregion

        public static string GetClientIp()
        {
            if (HttpContext.Current == null)
            {
                return IPAddress.Loopback.ToString();
            }
            string ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(ip))
            {
                ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            if (string.IsNullOrEmpty(ip))
            {
                ip = HttpContext.Current.Request.UserHostAddress;
            }
            if (ip == "::1")
            {
                ip = "127.0.0.1";
            }

            return ip;
        }

        #region GetLocalIP
        /// <summary>
        /// 得到本机IP
        /// </summary>
        public static string GetLocalIp()
        {
            string strLocalIp = "";
            string strPcName = Dns.GetHostName();
            IPHostEntry ipEntry = Dns.GetHostEntry(strPcName);
            foreach (var ip in ipEntry.AddressList)
            {
                if (IsRightIP(ip.ToString()))
                {
                    strLocalIp = ip.ToString();
                    break;
                }
            }

            return strLocalIp;
        }
        #endregion

        public static HashSet<string> GetLocalIPs()
        {
            var ips = new HashSet<string>(System.StringComparer.OrdinalIgnoreCase);
            string strPcName = Dns.GetHostName();
            IPHostEntry ipEntry = Dns.GetHostEntry(strPcName);
            foreach (var ip in ipEntry.AddressList)
            {
                if (IsRightIP(ip.ToString()))
                {
                    ips.Add(ip.ToString());
                }
            }

            return ips;
        }

        #region GetGateway
        /// <summary>
        /// 得到网关地址
        /// </summary>
        /// <returns></returns>
        public static string GetGateway()
        {
            string strGateway = "";
            //获取所有网卡
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            foreach (var netWork in nics)
            {
                IPInterfaceProperties ip = netWork.GetIPProperties();
                GatewayIPAddressInformationCollection gateways = ip.GatewayAddresses;
                foreach (var gateWay in gateways)
                {
                    if (IsPingIp(gateWay.Address.ToString()))
                    {
                        strGateway = gateWay.Address.ToString();
                        break;
                    }
                }

                if (strGateway.Length > 0)
                {
                    break;
                }
            }

            return strGateway;
        }
        #endregion

        #region IsRightIP
        /// <summary>
        /// 判断是否为正确的IP地址
        /// </summary>
        /// <param name="strIPadd">需要判断的字符串</param>
        /// <returns>true = 是 false = 否</returns>
        public static bool IsRightIP(string strIPadd)
        {
            if (Regex.IsMatch(strIPadd, "[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}"))
            {
                //根据小数点分拆字符串
                string[] ips = strIPadd.Split('.');
                if (ips.Length == 4 || ips.Length == 6)
                {
                    if (Int32.Parse(ips[0]) < 256 && Int32.Parse(ips[1]) < 256 & Int32.Parse(ips[2]) < 256 & Int32.Parse(ips[3]) < 256)
                        return true;
                    return false;
                }

                return false;
            }

            return false;
        }

        #endregion

        #region IsPingIP
        /// <summary>
        /// 尝试Ping指定IP是否能够Ping通
        /// </summary>
        /// <param name="strIp">指定IP</param>
        /// <returns>true 是 false 否</returns>
        public static bool IsPingIp(string strIp)
        {
            try
            {
                var ping = new Ping();
                PingReply reply = ping.Send(strIp, 1000);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion
       
    }
}
