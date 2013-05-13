// Github: http://www.github.com/laufin
// Licensed: LGPL-2.1 <http://opensource.org/licenses/lgpl-2.1.php>
// Author: Laufin <laufin@qq.com>
// Copyright (c) 2012-2013 Laufin,all rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Ling.Pager
{
    public partial class Pager
    {
        protected int vPageSize;   //每页记录数
        protected int vPageIndex;  //当前页码
        protected int vPageTotal;  //总页数
        protected int vRecords;    //总记录数

        protected string vCssClass;    // Css样式
        protected bool vShowPreNext;   //是否显示上一页下一页
        protected bool vShowPageNum;   //是否显示页码连接
        protected int vPageSplitNum;   //页码隔多少数字
        protected bool vShowPageTips;  //是否显示当前页、记录数等信息
        protected bool vShowPageJump;  //是否显示下来框跳转
        protected bool vShowPageGo;    //是否显示输入页码跳转


        //Lan   语言包
        protected Hashtable vHashLanguage = null;
        protected int vPageLanguage;   //显示风格： 0 - 默认， 1 - 中文， 2 - 英语 ， 其它默认
        protected string vLanguageTipsText; //提示文本
        protected string vLanguagePreText;  //上一页文本
        protected string vLanguageNextText; //下一页文本
        protected string vLanguageFirstText;    //首页文本
        protected string vLanguageLastText; //尾页文本
        protected string vLanguageGoText; //跳转按钮文本



    }
}
