/************************************************************************
* Copyright (c) 2019 All Rights Reserved.
*命名空间：Lucky.Proect.Core.Extensions
*文件名： Extensions
*创建人： Lxsh
*创建时间：2019/3/26 13:47:43
*描述
*=======================================================================
*修改标记
*修改时间：2019/3/26 13:47:43
*修改人：Lxsh
*描述：
************************************************************************/
using Lucky.Proect.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lucky.Proect.Core.Extensions
{
  public static partial  class Extensions
    {
        /// <summary>
        /// 列表生成树形节点 (只支持2级)
        /// </summary>
        /// <typeparam name="T">集合对象的类型</typeparam>
        /// <typeparam name="K">父节点的类型</typeparam>
        /// <param name="collection">集合</param>
        /// <param name="idSelector">主键ID</param>
        /// <param name="parentIdSelector">父节点</param>
        /// <param name="rootId">根节点</param>
        /// <returns>列表生成树形节点</returns>
        public static IEnumerable<TreeItem<T>> GenerateTree<T, K>(
            this IEnumerable<T> collection,
            Func<T, K> idSelector,
            Func<T, K> parentIdSelector,
            K rootId = default(K))
        {
            foreach (var c in collection.Where(u =>
            {
                var selector = parentIdSelector(u);
                return (rootId == null && selector == null)|| (rootId != null && rootId.Equals(selector));
            }))
            {
                yield return new TreeItem<T>
                {
                    Item = c,
                    Children = collection.GenerateTree(idSelector, parentIdSelector, idSelector(c))
                };
            }
        }
    }
}