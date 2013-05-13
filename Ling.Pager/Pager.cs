// Github: http://www.github.com/laufin
// Licensed: LGPL-2.1 <http://opensource.org/licenses/lgpl-2.1.php>
// Author: Laufin <laufin@qq.com>
// Copyright (c) 2012-2013 Laufin,all rights reserved.

using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Web;
using System.Web.Caching;
using System.Web.UI;
using System.Text.RegularExpressions;
using System.Collections;
using System.Data;
using System.Data.SqlClient;

namespace Ling.Pager
{
    /// <summary>
    /// ASP.NET分页控件2.0
    /// </summary>
    [ToolboxData("<{0}:Pager runat=\"server\"></{0}:Pager>")]
    [DefaultEvent("PageChange")] 
    public partial class Pager : Control, IPostBackEventHandler 
    {


        #region ________不可显示设置属性________
        /****************************************/
        /*                                      */
        /*         不可显示设置属性             */
        /*                                      */
        /****************************************/

        /// <summary>
        /// 当前页码
        /// </summary>
        [Browsable(false)]
        [Category("Appearance")]
        [PersistenceMode(PersistenceMode.Attribute)]
        public int PageIndex
        {
            get
            {
                if (this.ViewState["pageindex"] != null)
                {
                    vPageIndex = int.Parse(this.ViewState["pageindex"].ToString());
                }
                if (vPageIndex == 0 && PageChange == null)
                {
                    vPageIndex = GetRequestPageIndex();
                }
                if (vPageIndex <= 0)
                {
                    vPageIndex = 1;
                }
                return vPageIndex;
            }
            set
            {
                vPageIndex = value;
                this.ViewState["pageindex"] = vPageIndex;
            }
        }

        /// <summary>
        /// 总页数
        /// </summary>
        [Browsable(false)]
        [Category("Appearance")]
        [PersistenceMode(PersistenceMode.Attribute)]
        public int PageTotal
        {
            get
            {
                double count = this.Records;
                vPageTotal = int.Parse(Math.Ceiling(count / this.PageSize).ToString());
                if (vPageTotal == 0 && this.Records > 0) vPageTotal = 1;
                return vPageTotal;
            }
        }

        /// <summary>
        /// 总记录数
        /// </summary>
        [Browsable(false)]
        [Category("Appearance")]
        [PersistenceMode(PersistenceMode.Attribute)]
        public int Records
        {
            get
            {
                if (this.ViewState["records"] != null)
                {
                    vRecords = int.Parse(this.ViewState["records"].ToString());
                }
                return vRecords;
            }
            set
            {
                this.vRecords = value;
                this.ViewState["records"] = vRecords;
                this.EnableViewState = this.Visible = vRecords > this.PageSize;
                if (this.PageIndex > this.PageTotal) this.PageIndex = 1;
            }
        }

        #endregion  //不可显示设置属性end



        #region ________可以显示设置属性________
        /****************************************/
        /*                                      */
        /*         可以显示设置属性             */
        /*                                      */
        /****************************************/

        /// <summary>
        /// Css样式
        /// </summary>
        [DefaultValue("ling_pager")]
        [Category("Appearance")]
        [PersistenceMode(PersistenceMode.Attribute)]
        public string CssClass
        {
            get
            {
                vCssClass = "ling_pager";
                if (this.ViewState["CssClass"] != null)
                {
                    vCssClass = this.ViewState["CssClass"].ToString();
                }
                return vCssClass;
            }
            set
            {
                this.ViewState["CssClass"] = value;
                vCssClass = value;
            }
        }


        /// <summary>
        /// 每页记录数
        /// </summary>
        [DefaultValue(20)]
        [Category("Appearance")]
        [PersistenceMode(PersistenceMode.Attribute)]
        public int PageSize
        {
            get
            {
                vPageSize = 20;
                if (this.ViewState["PageSize"] != null)
                {
                    vPageSize = int.Parse(this.ViewState["PageSize"].ToString());
                }
                return vPageSize;
            }
            set
            {
                this.ViewState["PageSize"] = value;
                vPageSize = value;
            }
        }

        /// <summary>
        /// 是否显示当前页、记录数等信息
        /// </summary>
        [DefaultValue(false)]
        [Category("Appearance")]
        [PersistenceMode(PersistenceMode.Attribute)]
        public bool ShowPageTips
        {
            get
            {
                vShowPageTips = false;
                if (this.ViewState["ShowPageTips"] != null)
                {
                    vShowPageTips = bool.Parse(this.ViewState["ShowPageTips"].ToString());
                }
                return vShowPageTips;
            }
            set
            {
                this.ViewState["ShowPageTips"] = value;
                vShowPageTips = value;
            }
        }

        /// <summary>
        /// 是否显示数字页码，如：1... 4 5 6 7 8 ... 10
        /// </summary>
        [DefaultValue(true)]
        [Category("Appearance")]
        [PersistenceMode(PersistenceMode.Attribute)]
        public bool ShowPageNum
        {
            get
            {
                vShowPageNum = true;
                if (this.ViewState["ShowPageNum"] != null)
                {
                    vShowPageNum = bool.Parse(this.ViewState["ShowPageNum"].ToString());
                }
                return vShowPageNum;
            }
            set
            {
                this.ViewState["ShowPageNum"] = value;
                vShowPageNum = value;
            }
        }

        /// <summary>
        /// 数字页码左右两边个数
        /// </summary>
        [DefaultValue(5)]
        [Category("Appearance")]
        [PersistenceMode(PersistenceMode.Attribute)]
        public int PageSplitNum
        {
            get
            {
                vPageSplitNum = 5;
                if (this.ViewState["PageSplitNum"] != null)
                {
                    vPageSplitNum = int.Parse(this.ViewState["PageSplitNum"].ToString());
                }
                return vPageSplitNum;
            }
            set
            {
                this.ViewState["PageSplitNum"] = value;
                vPageSplitNum = value;
            }
        }

        /// <summary>
        /// 是否显示：上一页下一页
        /// </summary>
        [DefaultValue(true)]
        [Category("Appearance")]
        [PersistenceMode(PersistenceMode.Attribute)]
        public bool ShowPreNext
        {
            get
            {
                vShowPreNext = true;
                if (this.ViewState["ShowPreNext"] != null)
                {
                    vShowPreNext = bool.Parse(this.ViewState["ShowPreNext"].ToString());
                }
                return vShowPreNext;
            }
            set
            {
                this.ViewState["ShowPreNext"] = value;
                vShowPreNext = value;
            }
        }

        /// <summary>
        /// 是否显示下来框跳转
        /// </summary>
        [DefaultValue(false)]
        [Category("Appearance")]
        [PersistenceMode(PersistenceMode.Attribute)]
        public bool ShowPageJump
        {
            get
            {
                vShowPageJump = false;
                if (this.ViewState["ShowPageJump"] != null)
                {
                    vShowPageJump = bool.Parse(this.ViewState["ShowPageJump"].ToString());
                }
                return vShowPageJump;
            }
            set
            {
                this.ViewState["ShowPageJump"] = value;
                vShowPageJump = value;
            }
        }

        /// <summary>
        /// 是否显示输入页码点击跳转
        /// </summary>
        [DefaultValue(false)]
        [Category("Appearance")]
        [PersistenceMode(PersistenceMode.Attribute)]
        public bool ShowPageGo
        {
            get
            {
                vShowPageGo = false;
                if (this.ViewState["ShowPageGo"] != null)
                {
                    vShowPageGo = bool.Parse(this.ViewState["ShowPageGo"].ToString());
                }
                return vShowPageGo;
            }
            set
            {
                this.ViewState["ShowPageGo"] = value;
                vShowPageGo = value;
            }
        }

        /// <summary>
        /// 导航条语言:0=默认，1=中文，2=英语
        /// </summary>
        [DefaultValue(0)]
        [Category("Appearance")]
        [PersistenceMode(PersistenceMode.Attribute)]
        public int PageLanguage
        {
            get
            {
                vPageLanguage = 0;
                if (this.ViewState["PageLanguage"] != null)
                {
                    vPageLanguage = int.Parse(this.ViewState["PageLanguage"].ToString());
                }
                return vPageLanguage;
            }
            set
            {
                this.ViewState["PageLanguage"] = value;
                vPageLanguage = value;
            }
        }


        /// <summary>
        /// 提示文本：{PageIndex}=当前记录，{PageSize}=每页记录数，{Records}=总记录数，{PageTotal}=总页数
        /// 如：当前第{PageIndex}页，一共{PageTotal}页
        /// </summary>
        [DefaultValue("")]
        [Category("Appearance")]
        [PersistenceMode(PersistenceMode.Attribute)]
        public string LanguageTipsText
        {
            get
            {
                vLanguageTipsText = string.Empty;
                if (this.ViewState["LanguageTipsText"] != null)
                {
                    vLanguageTipsText = this.ViewState["LanguageTipsText"].ToString();
                }
                return vLanguageTipsText;
            }
            set
            {
                this.ViewState["LanguageTipsText"] = value;
                vLanguageTipsText = value;
            }
        }

        /// <summary>
        /// 第一页文本，如：首页
        /// </summary>
        [DefaultValue("")]
        [Category("Appearance")]
        [PersistenceMode(PersistenceMode.Attribute)]
        public string LanguageFirstText
        {
            get
            {
                vLanguageFirstText = string.Empty;
                if (this.ViewState["LanguageFirstText"] != null)
                {
                    vLanguageFirstText = this.ViewState["LanguageFirstText"].ToString();
                }
                return vLanguageFirstText;
            }
            set
            {
                this.ViewState["LanguageFirstText"] = value;
                vLanguageFirstText = value;
            }
        }


        /// <summary>
        /// 上一页文本,如：上一页
        /// </summary>
        [DefaultValue("")]
        [Category("Appearance")]
        [PersistenceMode(PersistenceMode.Attribute)]
        public string LanguagePreText
        {
            get
            {
                vLanguagePreText = string.Empty;
                if (this.ViewState["LanguagePreText"] != null)
                {
                    vLanguagePreText = this.ViewState["LanguagePreText"].ToString();
                }
                return vLanguagePreText;
            }
            set
            {
                this.ViewState["LanguagePreText"] = value;
                vLanguagePreText = value;
            }
        }

        /// <summary>
        /// 下一页文本，如：下一页
        /// </summary>
        [DefaultValue("")]
        [Category("Appearance")]
        [PersistenceMode(PersistenceMode.Attribute)]
        public string LanguageNextText
        {
            get
            {
                vLanguageNextText = string.Empty;
                if (this.ViewState["LanguageNextText"] != null)
                {
                    vLanguageNextText = this.ViewState["LanguageNextText"].ToString();
                }
                return vLanguageNextText;
            }
            set
            {
                this.ViewState["LanguageNextText"] = value;
                vLanguageNextText = value;
            }
        }

        /// <summary>
        /// 尾页文本，如：尾页
        /// </summary>
        [DefaultValue("")]
        [Category("Appearance")]
        [PersistenceMode(PersistenceMode.Attribute)]
        public string LanguageLastText
        {
            get
            {
                vLanguageLastText = string.Empty;
                if (this.ViewState["LanguageLastText"] != null)
                {
                    vLanguageLastText = this.ViewState["LanguageLastText"].ToString();
                }
                return vLanguageLastText;
            }
            set
            {
                this.ViewState["LanguageLastText"] = value;
                vLanguageLastText = value;
            }
        }


        /// <summary>
        /// 跳转按钮文本，如：跳转
        /// </summary>
        [DefaultValue("")]
        [Category("Appearance")]
        [PersistenceMode(PersistenceMode.Attribute)]
        public string LanguageGoText
        {
            get
            {
                vLanguageGoText = string.Empty;
                if (this.ViewState["LanguageGoText"] != null)
                {
                    vLanguageGoText = this.ViewState["LanguageGoText"].ToString();
                }
                return vLanguageGoText;
            }
            set
            {
                this.ViewState["LanguageGoText"] = value;
                vLanguageGoText = value;
            }
        }

        #endregion  //可显示设置属性end



        /// <summary>
        /// 取分页列表数据
        /// </summary>
        /// <param name="conectionString">连接字符串</param>
        /// <param name="sql">Sql语句</param>
        /// <param name="sqlParameters">Sql参数</param>
        /// <param name="order">排序</param>
        /// <param name="timeout">超时时间</param>
        /// <returns></returns>
        public DataTable PageList(string conectionString, string sql, SqlParameter[] sqlParameters, string order, int timeout)
        {
            int records = 0;
            DataTable dtList = DbHelper.PageList(conectionString, sql, sqlParameters, order, this.PageIndex, this.PageSize, out records);
            this.Records = records;
            return dtList;
        }


        /// <summary>
        /// 取分页列表数据
        /// </summary>
        /// <param name="conectionString">连接字符串</param>
        /// <param name="sql">Sql语句</param>
        /// <param name="sqlParameters">Sql参数</param>
        /// <param name="order">排序</param>
        /// <returns></returns>
        public DataTable PageList(string conectionString, string sql, SqlParameter[] sqlParameters, string order)
        {
            return PageList(conectionString, sql, sqlParameters, order, 0);
        }

        /// <summary>
        /// 取分页列表数据
        /// </summary>
        /// <param name="conectionString">连接字符串</param>
        /// <param name="sql">连接Sql</param>
        /// <param name="order">排序</param>
        /// <returns></returns>
        public DataTable PageList(string conectionString, string sql, string order)
        {
            return PageList(conectionString, sql, null, order, 0);
        }


        /// <summary>
        /// 设置语言
        /// </summary>
        /// <param name="item">设置文本项</param>
        /// <param name="text">文本</param>
        /// <returns></returns>
        public bool SetLanguageText(LanguageItem item, string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                string key = item.ToString();
                if (vHashLanguage == null)
                {
                    vHashLanguage = new Hashtable();
                }
                vHashLanguage[item.ToString()] = text;
                return true;
            }
            return false;
        }

    }


}

