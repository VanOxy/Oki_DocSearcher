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