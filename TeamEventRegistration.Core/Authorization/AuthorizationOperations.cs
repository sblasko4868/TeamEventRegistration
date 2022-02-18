using Microsoft.AspNetCore.Authorization.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using TeamEventRegistration.Core.Enumerations;

namespace TeamEventRegistration.Core.Authorization
{
    public class AuthorizationOperations
    {
        public static OperationAuthorizationRequirement Create =
          new OperationAuthorizationRequirement { Name = AuthorizationEnumerations.Permissions.Create.ToString() };
        public static OperationAuthorizationRequirement Read =
          new OperationAuthorizationRequirement { Name = AuthorizationEnumerations.Permissions.Read.ToString() };
        public static OperationAuthorizationRequirement Update =
          new OperationAuthorizationRequirement { Name = AuthorizationEnumerations.Permissions.Update.ToString() };
        public static OperationAuthorizationRequirement Delete =
          new OperationAuthorizationRequirement { Name = AuthorizationEnumerations.Permissions.Delete.ToString() };
        public static OperationAuthorizationRequirement Join =
          new OperationAuthorizationRequirement { Name = AuthorizationEnumerations.Permissions.Join.ToString() };
    }
}
