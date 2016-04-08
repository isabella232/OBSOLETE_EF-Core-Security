﻿using EFCoreSecurityODataService.Models;
using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using DevExpress.EntityFramework.SecurityDataStore.Security.BaseSecurityEntity;

namespace EFCoreSecurityODataService
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapODataServiceRoute(
                routeName: "ODataRoute",
                routePrefix: null,
                model: GetEdmModel());
        }
        private static IEdmModel GetEdmModel() {
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<Contact>("Contacts");
            builder.EntitySet<DemoTask>("Tasks");
            builder.EntitySet<ContactTask>("ContactTasks");
            builder.EntitySet<Department>("Departments");

            foreach(var type in builder.StructuralTypes) {
                if(typeof(ISecurityEntity).IsAssignableFrom(type.ClrType)) {
                    type.AddCollectionProperty(typeof(ISecurityEntity).GetProperty("BlockedMembers"));
                }
            }

            IEdmModel edmModel = builder.GetEdmModel();
            return edmModel;
        }
    }
}
