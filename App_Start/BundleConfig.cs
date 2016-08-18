using System.Web.Optimization;

namespace NewIspNL
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.Add(new ScriptBundle("~/bundles/Mainjquery").Include(

                 "~/Content/ace-assest/js/jquery-2.0.3.min.js",
                 "~/Content/ace-assest/js/bootstrap.min.js",
                 "~/Content/LoginJS/login.js"
              ));

            bundles.Add(new ScriptBundle("~/bundles/HomeJs").Include(

                "~/Content/ace-assest/js/jquery-2.0.3.min.js",
                "~/Content/ace-assest/js/ace.min.js",
                "~/Content/ace-assest/js/jquery-ui-1.10.3.full.min.js",
                "~/Content/ace-assest/js/bootstrap.min.js",
                "~/Content/ace-assest/js/ace-extra.min.js",
                "~/Content/ace-assest/js/chosen.jquery.min.js",
                "~/Content/ace-assest/js/fuelux/fuelux.spinner.min.js",
                "~/Content/ace-assest/js/jquery.maskedinput.min.js",
                "~/Content/ace-assest/js/ace-elements.min.js",
                "~/Content/ace-assest/js/jquery.ui.touch-punch.min.js",
                "~/Content/ace-assest/js/jquery.hotkeys.min.js",
                "~/Content/ace-assest/js/bootstrap-wysiwyg.min.js",
                "~/Content/ace-assest/js/typeahead-bs2.min.js",
                "~/Content/ace-assest/js/bootstrap-tag.min.js",
                "~/Content/ace-assest/css/Nossair/Home.js",
                "~/font/GESSTwoMedium-Medium",
                "~/Content/ace-assest/js/jquery.slimscroll.min.js",
                 "~/Content/ace-assest/js/jquery.easy-pie-chart.min.js",
                "~/Content/ace-assest/js/jquery.sparkline.min.js",
                "~/Content/ace-assest/js/flot/jquery.flot.min.js",
                "~/Content/ace-assest/js/flot/jquery.flot.pie.min.js",
                "~/Content/ace-assest/js/flot/jquery.flot.resize.min.js"

               

                ));

            //"~/Content/ace-assest/js/ace.min.js",

            //bundles.Add(new ScriptBundle("~/bundles/ChartJs").Include(
            //    "~/Content/ace-assest/js/jquery.easy-pie-chart.min.js",
            //    "~/Content/ace-assest/js/jquery.sparkline.min.js",
            //    "~/Content/ace-assest/js/flot/jquery.flot.min.js",
            //    "~/Content/ace-assest/js/flot/jquery.flot.pie.min.js",
            //    "~/Content/ace-assest/js/flot/jquery.flot.resize.min.js"

            //    ));
            bundles.Add(new ScriptBundle("~/bundles/tablesorter").Include(
                "~/Content/app/jquery.tablesorter.min.js" 
               ));
            bundles.Add(new ScriptBundle("~/bundles/numberOnly").Include(
                "~/Content/app/numberOnly.js" 
               ));
            bundles.Add(new ScriptBundle("~/bundles/paymentBs").Include(
                "~/Content/app/paymentBs.js" 
               ));
            bundles.Add(new StyleBundle("~/bundles/HomecSS").Include(

                "~/Content/ace-assest/css/bootstrap.min.css",
                "~/Content/ace-assest/css/ace.min.css",
                "~/Content/ace-assest/css/jquery-ui-1.10.3.full.min.css",
                "~/Content/ace-assest/css/ace-rtl.min.css",
                "~/Content/ace-assest/css/ace-fonts.css",
                "~/Content/ace-assest/css/font-awesome.min.css",
                "~/Content/ace-assest/css/ace-skins.min.css",
                "~/Content/ace-assest/css/Nossair/Login.css"
               

                ));


            bundles.Add(new StyleBundle("~/bundles/LogincSS").Include(

                 "~/Content/ace-assest/css/bootstrap.min.css",
                   "~/Content/ace-assest/css/ace.min.css",
                   "~/Content/ace-assest/css/Nossair/Login.css"

                            ));

            //bundles.Add(new StyleBundle("~/bundles/CSSAr").Include(
            //    "~/CSS/jquery/jquery*",

            //                ));

            //bundles.Add(new ScriptBundle("~/bundles/MainJSEN").Include(

            //     "~/JS/jquery/jquery*",


            //    ));
            //bundles.Add(new ScriptBundle("~/bundles/JSEN").Include(
            //            "~/JS/jquery/jquery*",


            //                ));




            BundleTable.EnableOptimizations = true;
        }

    }
}