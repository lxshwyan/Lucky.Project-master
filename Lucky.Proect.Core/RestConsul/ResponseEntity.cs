using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace Lucky.Proect.Core.RestConsul
{
    public  class ResponseEntity<T>
    {
        public HttpStatusCode StatusCode { get; set; }
        public T Body { get; set; }//返回的json反序列化出来的对象
        public HttpResponseHeaders Headers { get; set; }//响应的报文头
    }
}
