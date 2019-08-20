using System.Web.Optimization;
using Utility;
using Utility.Components;

namespace Web
{
    public class BundleConfig
    {
        // 有关 Bundling 的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            BundleConfigHelper.Init(bundles);
        }
    }
}
