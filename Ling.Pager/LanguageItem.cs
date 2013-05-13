// Github: http://www.github.com/laufin
// Licensed: LGPL-2.1 <http://opensource.org/licenses/lgpl-2.1.php>
// Author: Laufin <laufin@qq.com>
// Copyright (c) 2012-2013 Laufin,all rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ling.Pager
{
    /// <summary>
    /// 语言项
    /// </summary>
    public enum LanguageItem
    {
        /// <summary>
        /// 提示文本：{PageIndex}=当前记录，{PageSize}=每页记录数，{Records}=总记录数，{PageTotal}=总页数
        /// 如：当前第{PageIndex}页，一共{PageTotal}页
        /// </summary>
        Tips = 1,

        /// <summary>
        /// 第一页文本，如：首页
        /// </summary>
        First,

        /// <summary>
        /// 上一页文本,如：上一页
        /// </summary>
        Pre,

        /// <summary>
        /// 下一页文本，如：下一页
        /// </summary>
        Next,

        /// <summary>
        /// 尾页文本，如：尾页
        /// </summary>
        Last,

        /// <summary>
        /// 跳转按钮文本，如：跳转
        /// </summary>
        Go
    }
}
