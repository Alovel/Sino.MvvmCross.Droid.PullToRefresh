using System.Collections.Generic;
using Android.Content;
using Android.Views;
using PullToRefresharpDroid.Core;
using MvvmCross.Droid.Platform;
using MvvmCross.Core.ViewModels;
using System.Linq;

namespace PullToRefresharpDroid.Sample
{
	public class Setup : MvxAndroidSetup
    {
        public Setup(Context context)
            : base(context) { }

        protected override IMvxApplication CreateApp()
        {
            return new App();
        }

        protected override IEnumerable<System.Reflection.Assembly> AndroidViewAssemblies
        {
            get
            {
                var toReturn = base.AndroidViewAssemblies;
				var list = toReturn.ToList();
				list.Add(typeof(Sino.MvvmCross.Droid.Weight.MvxRefreshLayout).Assembly);
				return list;
            }
        }

        protected override IDictionary<string, string> ViewNamespaceAbbreviations
        {
            get
            {
                var toReturn = base.ViewNamespaceAbbreviations;
                toReturn["MC"] = "Sino.MvvmCross.Droid.Weight";
                return toReturn;
            }
        }
    }
}