
LingPager
=================================================

LingPager��ASP.NET��ҳ�ؼ�2.0��

## Features

���÷�ҳ��⣬����֧�ָ���Sql��ҳ
֧���¼���ҳ��Url��ҳ�����¼���
֧��Ĭ�ϡ����ġ�Ӣ������ģʽ��
Ĭ��Ư���Ҽ���Css��ʽ
֧��΢��Ajaxģʽ��ʹ��
���Զ���Css��ʽ��
�������������Ի��߷�ҳ��ʽ��
֧������ҳ�������ת��ҳ
֧����������з�ҳ
֧���϶�ʹ�ÿؼ������Ա༭��˫�����ɷ�ҳ�¼��ȷ��㡢ֱ�۲���
��ȫ���ʹ��
�������Եȴ��������顣


## ����

https://github.com/kindsoft/kindeditor

## ʹ��

-- ���븳ֵ --
Records: �ܼ�¼�������븳ֵ��

-- ������ --
CssClass�� css��ʽ��Ĭ����class="ling_pager"��ʽ
PageLanguage: ��ʾ��� 0 - Ĭ�ϣ� 1 - ���ģ� 2 - Ӣ�� �� ����Ĭ��
PageSplitNum: ����ҳ����ʾ�������֣�����ǰҳ�������߸���
PageSize: ÿҳ��ʾ��������¼
ShowPageNum: �Ƿ���ʾ���ֵ�����true-��ʾ(Ĭ��), false=����ʾ
ShowPreNext: �Ƿ���ʾ��һҳ��һҳ��true-��ʾ(Ĭ��), false=����ʾ
ShowPageGo: �Ƿ���ʾ����ҳ����ת��true-��ʾ, false=����ʾ(Ĭ��)
ShowPageJump: �Ƿ���ʾ��������ת��true-��ʾ, false=����ʾ(Ĭ��)
ShowPageTips: �Ƿ���ʾ��ǰҳ����¼������Ϣ
LanguageFirstText: ����ҳ�����ı�
LanguageLastText: ��βҳ�����ı�
LanguagePreText: ����һҳ�����ı�
LanguageNextText: ����һҳ�����ı�
LanguageGoText: ����ת����ť���ı�
LanguageTipsText: ��ʾ�ı�����ʹ�����¼���������{PageIndex}��ʾ��ǰҳ��{PageTotal}��ʾ��ҳ����{PageSize}��ʾÿҳ��¼����{Records}��ʾ�ܼ�¼��

-- �ɶ� --
PageIndex: ��ǰҳ��
PageTotal: ��ҳ��




�ؼ��ķ������£�

public DataTable PageList() //��װ�õĶ�ȡ��ҳ�б�������غ���
public bool SetLanguageText(LanguageItem item, string text) //���������ı�����LanguageTipsText��LanguageFirstText������Ч����ͬ��ͬʱ���ã������������ȼ��ߣ���




��չ��ⷽ����

Ling.Pager.DbHelper.PageList()	//��չ����ҳ����������֧�ָ���Sql��ҳ�������������

## ��������

Author: laufin
QQ:	250047953
Email:	laufin@qq.com
��ҳ:	http://www.liufuling.cn
����԰:	http://foolin.cnblogs.com/


