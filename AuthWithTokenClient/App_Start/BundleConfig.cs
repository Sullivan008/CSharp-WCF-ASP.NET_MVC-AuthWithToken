using System.Web.Optimization;

namespace AuthWithTokenClient
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            #region JavaScripts

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            #region Common

            #region Operations

            bundles.Add(new ScriptBundle("~/bundles/Scripts/app/common/operations/modal/modal-operations")
                .Include("~/Scripts/app/common/operations/modal/modal-operations.js"));

            #endregion

            #region Serialize

            bundles.Add(new ScriptBundle("~/bundles/Scripts/app/common/serialize/json/form-serialize-to-json")
                .Include("~/Scripts/app/common/serialize/json/jquery.serializeToJSON.js"));

            #endregion

            #endregion

            #region Views

            #region Home

            bundles.Add(new ScriptBundle("~/bundles/Scripts/app/views/home/index-view")
                .Include("~/Scripts/app/views/home/index-view.js"));

            #endregion

            #endregion

            #endregion

            #region Styles

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            #endregion
        }
    }
}