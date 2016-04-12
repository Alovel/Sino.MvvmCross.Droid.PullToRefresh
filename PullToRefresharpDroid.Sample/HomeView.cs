using Android.App;
using PullToRefresharpDroid.Core;
using Sino.Droid.MaterialDialogs;
using MvvmCross.Droid.Views;
using MvvmCross.Plugins.Messenger;
using MvvmCross.Platform;

namespace PullToRefresharpDroid.Sample
{
	[Activity(Label = "PullToRefresharpDroid.Sample", MainLauncher = true, Icon = "@drawable/icon")]
    public class HomeView : MvxActivity
    {
        private IMvxMessenger _messenger;
        public IMvxMessenger Messenger
        {
            get
            {
                if (_messenger == null)
                {
                    _messenger = Mvx.Resolve<IMvxMessenger>();
                }
                return _messenger;
            }
        }

        protected override void OnViewModelSet()
        {
            base.OnViewModelSet();
            SetContentView(Resource.Layout.Main);

            var dialog = new MaterialDialog.Builder(this).SetContent("正在加载更多").SetCancelable(false).SetProgress(true, 0).Build();

            Messenger.Subscribe<LoadMoreMessager>(x =>
            {
                if(x.IsClose)
                {
                    dialog.Dismiss();
                }
                else
                {
                    dialog.Show();
                }
            });
        }
    }
}

