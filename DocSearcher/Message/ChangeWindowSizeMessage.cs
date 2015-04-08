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