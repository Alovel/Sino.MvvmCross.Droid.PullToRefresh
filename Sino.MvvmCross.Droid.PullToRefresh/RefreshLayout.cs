using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V4.Widget;
using Android.Util;
using System.Windows.Input;

namespace Sino.MvvmCross.Droid.Weight
{
    [Register("sino.mvvmcross.droid.weight.MvxRefreshLayout")]
    public class MvxRefreshLayout : SwipeRefreshLayout , Android.Widget.AbsListView.IOnScrollListener
    {
        private int _touchSlop;
        private ListView _listView;
        private int _yDown;
        private int _lasttY;
        private bool _isLoading;

        /// <summary>
        /// 下拉加载更多
        /// </summary>
        public event EventHandler LoadMoreNative;
        
        public MvxRefreshLayout(Context context)
            : this(context, null) { }

        public MvxRefreshLayout(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
            _touchSlop = ViewConfiguration.Get(context).ScaledTouchSlop;
        }

        protected override void OnLayout(bool changed, int left, int top, int right, int bottom)
        {
            base.OnLayout(changed, left, top, right, bottom);
            if(_listView == null)
            {
                GetListView();
            }
        }

        private void GetListView()
        {
            for (int i = 0; i < ChildCount; i++)
            {
                View childView = GetChildAt(i);
                if (childView is ListView)
                {
                    _listView = childView as ListView;
                    _listView.SetOnScrollListener(this);
                    break;
                }
            }
        }

        public override bool DispatchTouchEvent(MotionEvent e)
        {
            switch (e.Action)
            {
                case MotionEventActions.Down:
                    {
                        _yDown = (int)e.RawY;
                    }
                    break;
                case MotionEventActions.Move:
                    {
                        _lasttY = (int)e.RawY;
                    }
                    break;
                case MotionEventActions.Up:
                    {
                        if (CanLoad)
                        {
                            LoadData();
                        }
                    }
                    break;
                default:
                    break;
            }
            return base.DispatchTouchEvent(e);
        }

        private bool CanLoad
        {
            get
            {
                return IsBottom && !_isLoading && IsPullUp;
            }
        }

        private bool IsBottom
        {
            get
            {
                if (_listView != null && _listView.Adapter != null)
                {
                    return _listView.LastVisiblePosition == _listView.Adapter.Count - 1;
                }
                return false;
            }
        }

        private bool IsPullUp
        {
            get
            {
                return (_yDown - _lasttY) >= _touchSlop;
            }
        }

        private void LoadData()
        {
            if(LoadMoreNative != null)
            {
                IsLoading = true;
                LoadMoreNative(this, null);
            }
        }

        #region IOnScrollListener

        public void OnScroll(AbsListView view, int firstVisibleItem, int visibleItemCount, int totalItemCount)
        {
            if (CanLoad)
            {
                LoadData();
            }
        }

        public void OnScrollStateChanged(AbsListView view, ScrollState scrollState)
        {

        }

        #endregion

        #region SupportForMvvmCross

        public bool IsLoading
        {
            get
            {
                return _isLoading;
            }
            set
            {
                _isLoading = value;
                if (_isLoading)
                {
                    _yDown = 0;
                    _lasttY = 0;
                }
            }
        }

        private ICommand _refreshActivated;
        public ICommand RefreshActivated
        {
            get
            {
                return _refreshActivated;
            }
            set
            {
                _refreshActivated = value;
                if (_refreshActivated != null)
                    EnsureRefreshActivatedOverloaded();
            }
        }

        private bool _refreshActivatedOverLoaded = false;
        private void EnsureRefreshActivatedOverloaded()
        {
            if (_refreshActivatedOverLoaded)
                return;

            _refreshActivatedOverLoaded = true;
            Refresh += (e, s) => ExecuteCommand(RefreshActivated);
        }

        private ICommand _loadMore;
        public ICommand LoadMore
        {
            get
            {
                return _loadMore;
            }
            set
            {
                _loadMore = value;
                if (_loadMore != null)
                    EnsureLoadMoreOverloaded();
            }
        }

        private bool _loadMoreOverLoaded = false;
        private void EnsureLoadMoreOverloaded()
        {
            if (_loadMoreOverLoaded)
                return;

            _loadMoreOverLoaded = true;
            LoadMoreNative += (snerder, args) => ExecuteCommand(LoadMore);
        }

        protected virtual void ExecuteCommand(ICommand command)
        {
            if (command == null)
                return;

            if (!command.CanExecute(null))
                return;

            command.Execute(null);
        }

        #endregion
    }
}