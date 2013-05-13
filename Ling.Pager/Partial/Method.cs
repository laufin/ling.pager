// Github: http://www.github.com/laufin
// Licensed: LGPL-2.1 <http://opensource.org/licenses/lgpl-2.1.php>
// Author: Laufin <laufin@qq.com>
// Copyright (c) 2012-2013 Laufin,all rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Ling.Pager
{
    public partial class Pager
    {

        /// <summary>
        /// 重写文件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {

            if (!this.DesignMode)
            {
                //const string key = "PagerCssPage";
                //判断是否采用默认属性
                if (CssClass == "ling_pager")
                {
                    //if (!this.Page.ClientScript.IsClientScriptBlockRegistered(key))
                    //{
                    //    string strCssFile = Page.ClientScript.GetWebResourceUrl(this.GetType(), "Sxmobi.Utility.Web.Pager.css");
                    //    //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), key, "<link href='" + strCssFile + "' rel='stylesheet' type='text/css' />");
                    //    LiteralControl include = new LiteralControl("<link href='" + strCssFile + "' rel='stylesheet' type='text/css' />");
                    //    this.Page.Header.Controls.Add(include);
                    //}

                    //判断是否第一次添加CSS，避免重复添加
                    if (Context.Items["IsHasCss"] == null)
                    {
                        string strCssFile = Page.ClientScript.GetWebResourceUrl(this.GetType(), "Ling.Pager.Pager.css");
                        //加载script方法
                        this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "PagerCss", @"
                        <script type=""text/javascript"">
                            function load" + this.Page.UniqueID + @"Css(cssfile){
　　                              var cssTag = document.getElementById('PagerCss');
　　                              var head = document.getElementsByTagName('head').item(0);
　　                              if(cssTag) head.removeChild(cssTag);
　　                              css = document.createElement('link');
　　                              css.href = cssfile;
　　                              css.rel  ='stylesheet';
　　                              css.type = 'text/css';
　　                              css.id = 'PagerCss';
　　                              head.appendChild(css);
                            }
                            load" + this.Page.UniqueID + @"Css('" + strCssFile + @"');
                        </script>
                            ");
                        Context.Items["IsHasCss"] = "true";
                    }

                }
            }

            base.OnInit(e);
        }



        /// <summary>
        /// 重写方法
        /// </summary>
        /// <param name="writer"></param>
        protected override void Render(HtmlTextWriter writer)
        {
            if (Context == null)
            {
                //模拟数据，仅供VS设计视图查看效果
                this.PageIndex = 6;
                this.Records = 500;
            }
            InitLan();  //初始化语言
            //如果未设置ID或者ID开头为数字，则赋值默认ID
            if (string.IsNullOrEmpty(this.ID))
            {
                writer.Write("<div style='height:20px;padding:5px;background:#ccc; border:solid 2px #00f; color:red;'>分页控件（Pager Control）出错啦，你未设置分页控件ID</div>");
            }
            else
            {
                if (this.Records > 0)
                {
                    WritePager(writer);
                }
            }

        }


        /// <summary>
        /// 分页导航条
        /// </summary>
        /// <param name="writer"></param>
        private void WritePager(HtmlTextWriter writer)
        {
            StringBuilder strPage = new StringBuilder();
            if (CssClass != "")
            {
                strPage.Append("<div class=\"" + CssClass + "\">");
            }
            else
            {
                strPage.Append("<div>");
            }



            //设置分页
            strPage.Append(PageStyle());

            strPage.Append("</div>");
            writer.Write(strPage);

        }



        /// <summary>
        /// 获取当前网址前缀
        /// </summary>
        /// <returns></returns>
        private string GetPageUrlPrefix()
        {
            string url = Context == null ? "" : Context.Request.Url.PathAndQuery;
            return Regex.Replace(url, ("&{0,1}" + this.ID + "=\\d*").ToString(), "", RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 规格化数字
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        private int ParsePageNum(int num)
        {
            if (num < 1)
            {
                return 1;
            }
            else if (num > PageTotal)
            {
                return PageTotal;
            }
            return num;
        }

        /// <summary>
        /// 规格化网址
        /// </summary>
        /// <param name="pageNum"></param>
        /// <param name="pageTitle"></param>
        /// <returns></returns>
        private string ParsePageUrl(int pageNum, string pageTitle)
        {
            if (PageChange != null)
            {
                return string.Format("<a href=\"{0}\">{1}</a>", Page.ClientScript.GetPostBackClientHyperlink(this, pageNum.ToString()), pageTitle);
            }
            else
            {
                string strUrlPrefix = GetPageUrlPrefix();
                //判断是否有参数
                if (strUrlPrefix.IndexOf('?') != -1)
                {
                    return (" <a href=\"" + strUrlPrefix + "&" + this.ID + "=" + ParsePageNum(pageNum) + "\">" + pageTitle + "</a> ").Replace("?&", "?");
                }

                return " <a href=\"" + strUrlPrefix + "?" + this.ID + "=" + ParsePageNum(pageNum) + "\">" + pageTitle + "</a> ";
            }
        }


        /// <summary>
        /// 获取当前的索引页
        /// </summary>
        /// <returns></returns>
        private int GetRequestPageIndex()
        {
            int index = 1;
            if (Context != null && !int.TryParse(Context.Request[this.ID], out index))
            {
                index = 1;
            }
            return index;
        }

        /// <summary>
        /// 默认的分页样式
        /// </summary>
        /// <returns></returns>
        private string PageStyle()
        {
            StringBuilder strPage = new StringBuilder();
            strPage.Append(Style_PageTips());
            strPage.Append(Style_PagePre());
            strPage.Append(Style_PageNum());
            strPage.Append(Style_PageNext());
            strPage.Append(Style_PageJump());
            strPage.Append(Style_PageGo());
            return strPage.ToString();
        }


        /// <summary>
        /// 显示当前页，总记录等
        /// </summary>
        /// <returns></returns>
        private string Style_PageTips()
        {
            if (ShowPageTips)
            {
                //return "第" + PageIndex + "页/共" + PageTotal + "页-共" + Records + "条记录 &nbsp ";
                return "<span class=\"pagetips\">" + vHashLanguage[LanguageItem.Tips.ToString()].ToString() + "</span>";
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 显示：首页 上一页
        /// </summary>
        /// <returns></returns>
        private string Style_PagePre()
        {
            StringBuilder strPage = new StringBuilder();
            if (ShowPreNext)
            {
                if (PageIndex > 1)
                {
                    //strPage.Append(ParsePageUrl(1, "首页 "));
                    //strPage.Append(ParsePageUrl(PageIndex - 1, " 上一页 "));
                    strPage.Append(ParsePageUrl(1, vHashLanguage[LanguageItem.First.ToString()].ToString()));
                    strPage.Append(ParsePageUrl(PageIndex - 1, vHashLanguage[LanguageItem.Pre.ToString()].ToString()));
                }
                //else
                //{
                //    //strPage.Append(" 首页 ");
                //    //strPage.Append(" 上一页 ");
                //    strPage.Append("<span>" + vHashLanguage["First"].ToString() + "</span>");
                //    strPage.Append("<span>" + vHashLanguage["Pre"].ToString() + "</span>");
                //}
            }
            return strPage.ToString();
        }

        /// <summary>
        /// 显示：尾页 下一页
        /// </summary>
        /// <returns></returns>
        private string Style_PageNext()
        {
            StringBuilder strPage = new StringBuilder();
            if (ShowPreNext)
            {
                if (PageIndex < PageTotal)
                {
                    //strPage.Append(ParsePageUrl(PageIndex + 1, " 下一页 "));
                    //strPage.Append(ParsePageUrl(PageTotal, " 尾页 "));
                    strPage.Append(ParsePageUrl(PageIndex + 1, vHashLanguage[LanguageItem.Next.ToString()].ToString()));
                    strPage.Append(ParsePageUrl(PageTotal, vHashLanguage[LanguageItem.Last.ToString()].ToString()));
                }
                //else
                //{
                //    //strPage.Append(" 下一页 ");
                //    //strPage.Append(" 尾页 ");
                //    strPage.Append("<span>" + vHashLanguage["Next"].ToString() + "</span>");
                //    strPage.Append("<span>" + vHashLanguage["Last"].ToString() + "</span>");
                //}
            }
            return strPage.ToString();
        }

        /// <summary>
        /// 显示页数字如： 1 2 3 4 ...6
        /// </summary>
        /// <returns></returns>
        private string Style_PageNum()
        {
            StringBuilder strPage = new StringBuilder();
            if (ShowPageNum)
            {
                //int iSplitNum = (int)(PageSplitNum / 2) + 1;
                int iSplitNum = PageSplitNum;
                for (int i = PageIndex - 1; (i > 0) && ((PageIndex - i) < iSplitNum); i--)
                {
                    strPage.Insert(0, ParsePageUrl(i, i.ToString()));
                }
                if ((PageIndex - iSplitNum) > 0)
                {
                    if ((PageIndex - iSplitNum) != 1)
                    {
                        //strPage.Insert(0, " ... ");
                        strPage.Insert(0, ParsePageUrl(PageIndex - iSplitNum, "..."));
                    }
                    strPage.Insert(0, ParsePageUrl(1, "1"));
                }
                strPage.Append(" <span class=\"current\">" + PageIndex + "</span> ");
                for (int j = PageIndex + 1; (j <= PageTotal) && ((j - PageIndex) < iSplitNum); j++)
                {
                    strPage.Append(ParsePageUrl(j, j.ToString()));
                }
                if ((PageIndex + iSplitNum) <= PageTotal)
                {
                    if ((PageIndex + iSplitNum) != PageTotal)
                    {
                        //strPage.Append(" ... ");
                        strPage.Append(ParsePageUrl(PageIndex + iSplitNum, "..."));
                    }
                    strPage.Append(ParsePageUrl(PageTotal, PageTotal.ToString()));
                }
            }
            return strPage.ToString();
        }

        /// <summary>
        /// 显示：跳转操控
        /// </summary>
        /// <returns></returns>
        private string Style_PageJump()
        {
            string strUrlPrefix = GetPageUrlPrefix();
            StringBuilder strPage = new StringBuilder();

            if (ShowPageJump)
            {
                strPage.Append("<span class=\"pagejump\">");
                if (PageChange == null)
                {
                    if (strUrlPrefix.IndexOf('?') != -1)
                    {
                        strPage.Append("<select name=\"sel" + this.UniqueID + "\" onchange=\"self.location.href='"
                                        + strUrlPrefix + "&" + this.ID + "=' + this.options[this.selectedIndex].value;\">").Replace("?&", "?");
                    }
                    else
                    {
                        strPage.Append("<select name=\"sel" + this.UniqueID + "\" onchange=\"self.location.href='"
                            + strUrlPrefix + "?" + this.ID + "=' + this.options[this.selectedIndex].value;\">");
                    }
                }
                else
                {
                    strPage.Append(string.Format("<select name=\"sel{0}\" onchange=\"javascript:__doPostBack('{0}',this.options[this.selectedIndex].value);\">", this.UniqueID));
                }
                for (int i = 1; i <= PageTotal; i++)
                {
                    if (i == PageIndex)
                    {
                        strPage.Append("<option value=\"" + i + "\" selected=\"selected\">" + i + "</option>");
                    }
                    else
                    {
                        strPage.Append("<option value=\"" + i + "\">" + i + "</option>");
                    }
                }
                strPage.Append("</select> ");
                strPage.Append("</span> ");
            }
            else
            {
                strPage.Append("");
            }
            return strPage.ToString();
        }

        /// <summary>
        /// 直接输入页数进行跳转
        /// </summary>
        /// <returns></returns>
        private string Style_PageGo()
        {
            StringBuilder strPage = new StringBuilder();
            if (ShowPageGo)
            {
                strPage.Append("<span class=\"pagego\">");
                strPage.Append("<input name=\"txt" + this.UniqueID + "\"  id=\"txt" + this.UniqueID + "\" type=\"text\" value=\"" + PageIndex + "\" style=\"width:50px;\" />");
                strPage.Append("<input name=\"btn" + this.UniqueID + "\" type=\"button\" value=\"" + vHashLanguage[LanguageItem.Go.ToString()].ToString() + "\" ");
                if (PageChange == null)
                {
                    string strUrlPrefix = GetPageUrlPrefix();
                    if (strUrlPrefix.IndexOf('?') != -1)
                    {
                        strPage.Append((" onclick=\"self.location.href='" + strUrlPrefix
                            + "&" + this.ID + "=' + document.getElementById('txt" + this.UniqueID + "').value;\" />").Replace("?&", "?"));
                    }
                    else
                    {
                        strPage.Append(" onclick=\"self.location.href='" + strUrlPrefix
                            + "?" + this.ID + "=' + document.getElementById('txt" + this.UniqueID + "').value;\" />");
                    }
                }
                else
                {
                    strPage.Append(string.Format(" onclick=\"javascript:__doPostBack('{0}',document.getElementById('txt{0}').value)\" />", this.UniqueID));
                }
                strPage.Append("</span>");
            }
            return strPage.ToString();
        }



        /// <summary>
        /// 初始化语言栏
        /// </summary>
        private void InitLan()
        {
            if (vHashLanguage == null)
            {
                vHashLanguage = new System.Collections.Hashtable();
            }
            switch (PageLanguage)
            {
                case 1:
                    _setDefaultLanauge(LanguageItem.Tips, "第{PageIndex}页/共{PageTotal}页-共{Records}条记录");
                    _setDefaultLanauge(LanguageItem.First, "首页");
                    _setDefaultLanauge(LanguageItem.Pre, "上一页");
                    _setDefaultLanauge(LanguageItem.Next, "下一页");
                    _setDefaultLanauge(LanguageItem.Last, "尾页");
                    _setDefaultLanauge(LanguageItem.Go, "跳转");
                    break;
                case 2:
                    _setDefaultLanauge(LanguageItem.Tips, "Page:{PageIndex}/Pages:{PageTotal}-Records:{Records}");
                    _setDefaultLanauge(LanguageItem.First, "First");
                    _setDefaultLanauge(LanguageItem.Pre, "Pre");
                    _setDefaultLanauge(LanguageItem.Next, "Next");
                    _setDefaultLanauge(LanguageItem.Last, "Last");
                    _setDefaultLanauge(LanguageItem.Go, "Go");
                    break;
                default:
                    _setDefaultLanauge(LanguageItem.Tips, "{PageIndex}/{PageTotal}-{Records}");
                    _setDefaultLanauge(LanguageItem.First, "|<");
                    _setDefaultLanauge(LanguageItem.Pre, "<");
                    _setDefaultLanauge(LanguageItem.Next, ">");
                    _setDefaultLanauge(LanguageItem.Last, ">|");
                    _setDefaultLanauge(LanguageItem.Go, "Go");
                    break;
            }

        }

        /// <summary>
        /// 设置默认语言
        /// </summary>
        /// <param name="item"></param>
        /// <param name="text"></param>
        private void _setDefaultLanauge(LanguageItem item, string text)
        {
            if (!vHashLanguage.ContainsKey(item.ToString()))
            {
                string languageText = string.Empty;
                switch (item)
                {
                    case LanguageItem.Tips:
                        languageText = this.LanguageTipsText;
                        break;
                    case LanguageItem.Pre:
                        languageText = this.LanguagePreText;
                        break;
                    case LanguageItem.Next:
                        languageText = this.LanguageNextText;
                        break;
                    case LanguageItem.First:
                        languageText = this.LanguageFirstText;
                        break;
                    case LanguageItem.Last:
                        languageText = this.LanguageLastText;
                        break;
                    case LanguageItem.Go:
                        languageText = this.LanguageGoText;
                        break;
                }
                if (!string.IsNullOrEmpty(languageText))
                {
                    text = languageText;
                }
            }
            else
            {
                text = vHashLanguage[item.ToString()].ToString();
            }
            vHashLanguage[item.ToString()] = text.Replace("{PageIndex}", this.PageIndex.ToString())
                    .Replace("{PageSize}", this.PageSize.ToString())
                    .Replace("{PageTotal}", this.PageTotal.ToString())
                    .Replace("{Records}", this.Records.ToString());
        }

    }
}
