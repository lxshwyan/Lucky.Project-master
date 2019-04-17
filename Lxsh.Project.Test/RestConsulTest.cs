/************************************************************************
* Copyright (c) 2019 All Rights Reserved.
*命名空间：Lxsh.Project.Test
*文件名： RestConsulTest
*创建人： Lxsh
*创建时间：2019/4/3 14:51:20
*描述
*=======================================================================
*修改标记
*修改时间：2019/4/3 14:51:20
*修改人：Lxsh
*描述：
************************************************************************/
using Lucky.Proect.Core.Extensions;
using Lucky.Proect.Core.RestConsul;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Lxsh.Project.Test
{
    public class RestConsulTest
    {
        [Fact]
        public void CheckConsul()
        {   
            RestTemplate restTemplate = new RestTemplate();
            var result= restTemplate.GetForEntityAsync<string[]>("http://Lucky.Project.API.Service/api/Values").Result;
             Assert.Equal(2, result.Body.Length);

        }
    }
}