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
    public partial class Pager
    {

        public delegate void PagerHandler(object sender, int currPageIndex, int prePageIndex);

        /// <summary>
        /// 点击事件
        /// </summary>
        public event PagerHandler PageChange;


        //Invoke   delegates   registered   with   the   Click   event. 
        protected virtual void _DoPageChange(object sender, int currPageIndex, int prePageIndex)
        {
            if (PageChange != null)
            {
                PageChange(sender, currPageIndex, prePageIndex);
            }
        }

        public void RaisePostBackEvent(string eventArgument)
        {
            //DoPageChange(new EventArgs());
            if (PageChange != null)
            {
                int prePageIndex = this.PageIndex;
                int currPageIndex = 0;
                int.TryParse(eventArgument, out currPageIndex);
                this.PageIndex = currPageIndex;
                _DoPageChange(this, currPageIndex, prePageIndex);
            }
        }
    }
}
