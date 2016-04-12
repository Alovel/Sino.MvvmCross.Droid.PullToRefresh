using MvvmCross.Plugins.Messenger;

namespace PullToRefresharpDroid.Core
{
	public class LoadMoreMessager : MvxMessage
    {
        public LoadMoreMessager(object sender)
            : base(sender) { }

        public bool IsClose { get; set; }
    }
}
