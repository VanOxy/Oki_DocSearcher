using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocSearcher.Message
{
    internal class ChangeWindowSizeMessage
    {
        public string Stage { get; set; }

        public ChangeWindowSizeMessage(string str)
        {
            Stage = str;
        }
    }
}