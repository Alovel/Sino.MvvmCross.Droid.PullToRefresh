using MvvmCross.Core.ViewModels;

namespace PullToRefresharpDroid.Core
{
	public class App : MvxApplication
    {
        public override void Initialize()
        {
            base.Initialize();
            RegisterAppStart<HomeViewModel>();
        }
    }
}
