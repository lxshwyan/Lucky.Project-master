using Consul;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Lucky.Proect.Core.Extensions;

namespace Lucky.Proect.Core.RestConsul
{
    public class RestTemplate
    {
        private String consulServerUrl;

        public RestTemplate(String consulServerUrl = "http://127.0.0.1:8500")
        {
            this.consulServerUrl = consulServerUrl;
        }
        /// <summary>
        /// 获取服务的第一个实现地址
        /// </summary>
        /// <param name="consulClient"></param>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        private async Task<String> ResolveRootUrlAsync(String serviceName)
        {
            using (var consulClient = new ConsulClient(c => c.Address = new Uri(consulServerUrl)))
            {
                var services = (await consulClient.Agent.Services()).Response;
                var agentServices = services.Where(s => s.Value.Service.Equals(serviceName, StringComparison.CurrentCultureIgnoreCase))
                    .Select(s => s.Value);
                //TODO:注入负载均衡策略
                var agentService = agentServices.ElementAt(Environment.TickCount % agentServices.Count());
                //根据当前TickCount对服务器个数取模，“随机”取一个机器出来，避免“轮询”的负载均衡策略需要计数加锁问题
                return agentService.Address + ":" + agentService.Port;
            }
        }

        //把http://apiservice1/api/values转换为http://192.168.1.1:5000/api/values
        private async Task<String> ResolveUrlAsync(String url)
        {
            Uri uri = new Uri(url);
            String serviceName = uri.Host;//apiservice1
            String realRootUrl = await ResolveRootUrlAsync(serviceName);//查询出来apiservice1对应的服务器地址192.168.1.1:5000
                                                                        //uri.Scheme=http,realRootUrl =192.168.1.1:5000,PathAndQuery=/api/values
            return uri.Scheme + "://" + realRootUrl + uri.PathAndQuery;
        }

        public async Task<ResponseEntity<T>> GetForEntityAsync<T>(String url, HttpRequestHeaders requestHeaders = null)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                HttpRequestMessage requestMsg = new HttpRequestMessage();
                if (requestHeaders != null)
                {
                    foreach (var header in requestHeaders)
                    {
                        requestHeaders.Add(header.Key, header.Value);
                    }
                }
                requestMsg.Method = HttpMethod.Get;
                //http://apiservice1/api/values转换为http://192.168.1.1:5000/api/values
                requestMsg.RequestUri = new Uri(await ResolveUrlAsync(url));

                var result = await httpClient.SendAsync(requestMsg);
                ResponseEntity<T> respEntity = new ResponseEntity<T>();
                respEntity.StatusCode = result.StatusCode;
                string bodyStr = await result.Content.ReadAsStringAsync();
                string st = bodyStr;
                respEntity.Body =bodyStr.FromJson<T>();
                respEntity.Headers = respEntity.Headers;
                return respEntity;
            }
        }

    }
}
