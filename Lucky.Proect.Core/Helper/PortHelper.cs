﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using System.Net;

namespace Lucky.Proect.Core.Helper
{
    public class PortHelper
    {
        /// <summary>
        /// 产生一个介于minPort-maxPort之间的随机可用端口
        /// </summary>
        /// <param name="minPort"></param>
        /// <param name="maxPort"></param>
        /// <returns></returns>
        public static int GetRandAvailablePort(int minPort = 1024, int maxPort = 65535)
        {
            Random rand = new Random();
            while (true)
            {
                int port = rand.Next(minPort, maxPort);
                if (!IsPortInUsed(port))
                {
                    return port;
                }
            }
        }

        /// <summary>
        /// 判断port端口是否在使用中
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public static bool IsPortInUsed(int port)
        {
            //using System.Net.NetworkInformation;
            IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] ipsTCP = ipGlobalProperties.GetActiveTcpListeners();
            if (ipsTCP.Any(p => p.Port == port))
            {
                return true;
            }

            IPEndPoint[] ipsUDP = ipGlobalProperties.GetActiveUdpListeners();
            if (ipsUDP.Any(p => p.Port == port))
            {
                return true;
            }
            TcpConnectionInformation[] tcpConnInfoArray = ipGlobalProperties.GetActiveTcpConnections();
            if (tcpConnInfoArray.Any(conn => conn.LocalEndPoint.Port == port))
            {
                return true;
            }
            return false;
        }
    }
}
