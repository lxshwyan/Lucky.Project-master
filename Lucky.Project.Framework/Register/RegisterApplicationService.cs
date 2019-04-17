/************************************************************************
* Copyright (c) 2019 All Rights Reserved.
*命名空间：Lucky.Project.Framework.Register
*文件名： RegisterApplicationService
*创建人： Lxsh
*创建时间：2019/3/27 9:44:10
*描述
*=======================================================================
*修改标记
*修改时间：2019/3/27 9:44:10
*修改人：Lxsh
*描述：
************************************************************************/
using Microsoft.AspNetCore.SignalR;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Lucky.Project.Framework.Register
{
    public class RegisterApplicationService: IRegisterApplicationService  
    {
     
        /// 初始化
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void initRegister()
        {   
        }
    }
}