using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using CrimeRecordsManager.Models;

namespace CrimeRecordsManager
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            ODataModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<PoliceStation>("PoliceStations");
            builder.EntitySet<PoliceOfficer>("PoliceOfficers");

            config.MapODataServiceRoute(
                routeName: "ODataRoute",
                routePrefix: "api",
                model: builder.GetEdmModel()
            );
            config.Count().Filter().OrderBy().Expand().Select().MaxTop(100);
        }
    }
}
