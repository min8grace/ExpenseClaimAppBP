using System.Collections.Generic;

namespace StoreManager.Application.Constants
{
    public static class Permissions
    {
        public static List<string> GeneratePermissionsForModule(string module)
        {
            return new List<string>()
            {
                $"Permissions.{module}.Create",
                $"Permissions.{module}.View",
                $"Permissions.{module}.Edit",
                $"Permissions.{module}.Delete",
            };
        }

        public static class Dashboard
        {
            public const string View = "Permissions.Dashboard.View";
            public const string Create = "Permissions.Dashboard.Create";
            public const string Edit = "Permissions.Dashboard.Edit";
            public const string Delete = "Permissions.Dashboard.Delete";
        }

        public static class Products
        {
            public const string View = "Permissions.Products.View";
            public const string Create = "Permissions.Products.Create";
            public const string Edit = "Permissions.Products.Edit";
            public const string Delete = "Permissions.Products.Delete";
        }

        public static class Users
        {
            public const string View = "Permissions.Users.View";
            public const string Create = "Permissions.Users.Create";
            public const string Edit = "Permissions.Users.Edit";
            public const string Delete = "Permissions.Users.Delete";
        }

        public static class Brands
        {
            public const string View = "Permissions.Brands.View";
            public const string Create = "Permissions.Brands.Create";
            public const string Edit = "Permissions.Brands.Edit";
            public const string Delete = "Permissions.Brands.Delete";
        }
        public static class Request
        {
            public const string View = "Permissions.Request.View";
            public const string Create = "Permissions.Request.Create";
            public const string Edit = "Permissions.Request.Edit";
            public const string Delete = "Permissions.Request.Delete";
        }
        public static class Approved
        {
            public const string View = "Permissions.Approved.View";
            public const string Create = "Permissions.Approved.Create";
            public const string Edit = "Permissions.Approved.Edit";
            public const string Delete = "Permissions.Approved.Delete";
        }
        public static class Finance
        {
            public const string View = "Permissions.Finance.View";
            public const string Create = "Permissions.Finance.Create";
            public const string Edit = "Permissions.Finance.Edit";
            public const string Delete = "Permissions.Finance.Delete";
        }
        public static class Claim
        {
            public const string View = "Permissions.Claim.View";
            public const string Create = "Permissions.Claim.Create";
            public const string Edit = "Permissions.Claim.Edit";
            public const string Delete = "Permissions.Claim.Delete";
        }
    }
}