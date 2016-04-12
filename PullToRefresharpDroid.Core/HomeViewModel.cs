using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace PullToRefresharpDroid.Core
{
	public class HomeViewModel : MvxViewModel
    {
        private IMvxMessenger Messenger;

        public HomeViewModel(IMvxMessenger messenger)
        {
            var items = Enumerable.Range(0, 13).Select(i => new ListItemViewModel
            {
                Title = "标题" + i.ToString(),
                Notes = "笔记" + i.ToString(),
                When = "索引" + i.ToString()
            });
            Items = new ObservableCollection<ListItemViewModel>(items);
            Messenger = messenger;
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get
            {
                return _isLoading;
            }
            set
            {
                _isLoading = value;
                RaisePropertyChanged(() => IsLoading);
            }
        }

        private bool _refreshing;
        public bool Refreshing
        {
            get
            {
                return _refreshing;
            }
            set
            {
                _refreshing = value;
                RaisePropertyChanged(() => Refreshing);
            }
        }

        public bool HasItems
        {
            get
            {
                return Items != null && Items.Count > 0;
            }
        }

        private ObservableCollection<ListItemViewModel> _items;
        public ObservableCollection<ListItemViewModel> Items
        {
            get 
            {
                return _items;
            }
            set
            {
                SetProperty(ref _items, value);
                value.CollectionChanged += (e, s) =>
                {
                    RaisePropertyChanged(() => HasItems);
                };
                RaisePropertyChanged(() => HasItems);
            }
        }

        public IMvxCommand ItemClick
        {
            get
            {
                return new MvxCommand<ListItemViewModel>(x =>
                {
                    Mvx.Trace("{0}-{1}-{2}", x.Title, x.Notes, x.When);
                });
            }
        }

        public IMvxCommand RefreshActivated
        {
            get
            {
                return new MvxCommand(async () =>
                {
                    Mvx.Trace("Refresh");
                    await Task.Delay(2000);
                    Refreshing = false;
                });
            }
        }

        public IMvxCommand LoadMore
        {
            get
            {
                return new MvxCommand(async () =>
                {
                    Mvx.Trace("LoadMore");
                    Messenger.Publish<LoadMoreMessager>(new LoadMoreMessager(this)
                    {
                        IsClose = false
                    });
                    await Task.Delay(2000);
                    IsLoading = false;
                    Messenger.Publish<LoadMoreMessager>(new LoadMoreMessager(this)
                    {
                        IsClose = true
                    });
                });
            }
        }
    }
}
