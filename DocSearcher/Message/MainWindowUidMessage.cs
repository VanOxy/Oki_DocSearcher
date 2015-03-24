using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace DocSearcher.Message
{
    internal class MainWindowUidMessage
    {
        public Thread ThreadUid { get; set; }

        public MainWindowUidMessage(Thread p)
        {
            ThreadUid = p;
        }
    }
}