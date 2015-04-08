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