using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocSearcher.Message
{
    internal class ExtensionManagementFileTypeSelectedMessage
    {
        public string SelectedFileType;

        public ExtensionManagementFileTypeSelectedMessage(string p)
        {
            SelectedFileType = p;
        }
    }
}