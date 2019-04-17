/************************************************************************
* Copyright (c) 2019 All Rights Reserved.
*命名空间：Lucky.Project.Models.Base
*文件名： Base_Menu
*创建人： Lxsh
*创建时间：2019/3/26 10:59:09
*描述
*=======================================================================
*修改标记
*修改时间：2019/3/26 10:59:09
*修改人：Lxsh
*描述：
************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Lucky.Project.Models
{
    /// <summary>   
    /// 后台管理菜单
    /// </summary>
    [Table("Base_Menu")]
    public class Base_Menu
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        public string Id { get; set; }

        /// <summary>
        /// 父菜单ID
        /// </summary>
        [Required]     
        public string ParentId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [MaxLength(32)]
        public String Name { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        [MaxLength(128)]
        public String DisplayName { get; set; }

        /// <summary>
        /// 图标地址
        /// </summary>
        [MaxLength(128)]
        public String IconUrl { get; set; }

        /// <summary>
        /// 链接地址
        /// </summary>
        [MaxLength(128)]
        public String LinkUrl { get; set; }

        /// <summary>
        /// 排序数字
        /// </summary>
        [MaxLength(10)]
        public Int32? Sort { get; set; }

        /// <summary>
        /// 操作权限（按钮权限时使用）
        /// </summary>
        [MaxLength(256)]
        public String Permission { get; set; }

        /// <summary>
        /// 是否显示
        /// </summary>
        [Required]
        [MaxLength(1)]
        public Boolean IsDisplay { get; set; }

        /// <summary>
        /// 是否系统默认
        /// </summary>
        [Required]
        [MaxLength(1)]
        public Boolean IsSystem { get; set; }

        /// <summary>
        /// 添加人
        /// </summary>
        [Required]
        [MaxLength(10)]
        public string CreatorId { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        [Required]
        [MaxLength(23)]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        [MaxLength(10)]
        public string ModifyId { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        [MaxLength(23)]
        public DateTime? ModifyTime { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        [Required]
        [MaxLength(1)]
        public Boolean IsDelete { get; set; }

         public bool IsMenu { get; set; }
        public string Controller { get; set; }

        public string Action { get; set; }

        public string RouteName { get; set; }



    }
}