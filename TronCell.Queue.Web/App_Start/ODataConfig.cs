using System.Web.Http;
using System.Web.Http.OData.Builder;
using Queue.Entities.Models;

namespace TronCell.Queue.Web
{
    public static class ODataConfig
    {
        public static void Register(HttpConfiguration config)
        {
            ODataModelBuilder builder = new ODataConventionModelBuilder();

            builder.EntitySet<Fitting>(typeof(Fitting).Name);
            builder.EntitySet<FittingRoom>(typeof(Fitting).Name);
            var model = builder.GetEdmModel();
            config.Routes.MapODataRoute("odata", "odata", model);
            config.EnableQuerySupport();
        }
    }
}