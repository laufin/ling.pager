
LingPager
=================================================

LingPager2.0是ASP.NET分页控件，支持事件、非事件，附带CSS样式等等。  

## 主要特色

内置分页类库，完美支持复杂Sql分页  
支持事件分页和Url分页（非事件）  
支持默认、中文、英文语言模式。  
默认漂亮且简洁的Css样式  
支持微软Ajax模式下使用  
可自定义Css样式。  
可随意设置语言或者分页格式。  
支持输入页码进行跳转分页  
支持下拉框进行分页  
支持拖动使用控件及属性编辑、双击生成分页事件等方便、直观操作  
完全免费使用  
更多特性等待您的体验。  


## 下载

https://github.com/kindsoft/kindeditor

## 使用

-- 必须赋值 --  
Records: 总记录数（必须赋值）  

-- 可设置 --  
CssClass： css样式，默认是class="ling_pager"样式  
PageLanguage: 显示风格： 0 - 默认， 1 - 中文， 2 - 英语 ， 其它默认  
PageSplitNum: 数字页码显示多少数字，即当前页左右两边个数  
PageSize: 每页显示多少条记录  
ShowPageNum: 是否显示数字导航：true-显示(默认), false=不显示  
ShowPreNext: 是否显示上一页下一页：true-显示(默认), false=不显示  
ShowPageGo: 是否显示输入页码跳转：true-显示, false=不显示(默认)  
ShowPageJump: 是否显示下来框跳转：true-显示, false=不显示(默认)  
ShowPageTips: 是否显示当前页、记录数等信息  
LanguageFirstText: “首页”的文本  
LanguageLastText: “尾页”的文本  
LanguagePreText: “上一页”的文本  
LanguageNextText: “下一页”的文本  
LanguageGoText: “跳转”按钮的文本  
LanguageTipsText: 提示文本，可使用以下几个变量：{PageIndex}表示当前页，{PageTotal}表示总页数，{PageSize}表示每页记录数，{Records}表示总记录数  


-- 可读 --  
PageIndex: 当前页数  
PageTotal: 总页数  


控件的方法如下：  
public DataTable PageList() //封装好的读取分页列表，多个重载函数  
public bool SetLanguageText(LanguageItem item, string text) //设置语言文本，与LanguageTipsText、LanguageFirstText等设置效果相同（同时设置，方法设置优先级高）。  



扩展类库方法：  
Ling.Pager.DbHelper.PageList()	//扩展类库分页方法，完美支持复杂Sql分页，多个函数重载  


## 关于作者

Author: laufin  
QQ:	250047953  
Email:	laufin(at)qq.com  
主页:	http://www.liufuling.cn  
博客园:	http://foolin.cnblogs.com/  
github: https://github.com/laufin/ling.pager  



