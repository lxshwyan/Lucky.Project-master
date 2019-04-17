/************************************************************************
* Copyright (c) 2019 All Rights Reserved.
*命名空间：Lucky.Proect.Core.Models
*文件名： TreeItem
*创建人： Lxsh
*创建时间：2019/3/26 13:48:45
*描述
*=======================================================================
*修改标记
*修改时间：2019/3/26 13:48:45
*修改人：Lxsh
*描述：
************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace Lucky.Proect.Core.Models
{
    public class TreeItem<T>
    {
        public T Item { get; set; }
        public IEnumerable<TreeItem<T>> Children { get; set; }
    }
}