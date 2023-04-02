using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.SwaggerConfig.Permissions
{
    //با این کلاس ، DefaultAuthorizationPolicyProvider را یک بار باز تعریف میکنیم  
    //public class AuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
    //{
    //    public AuthorizationPolicyProvider(IOptions<AuthorizationOptions> options)
    //        : base(options)
    //    {
    //    }

    //    public override Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
    //    {
    //        if (!policyName.StartsWith(PermissionAuthorizeAttribute.PolicyPrefix, StringComparison.OrdinalIgnoreCase))
    //        {
    //            return base.GetPolicyAsync(policyName);
    //        }

    //        var permissionNames = policyName.Substring(PermissionAuthorizeAttribute.PolicyPrefix.Length).Split(',');

    //        var policy = new AuthorizationPolicyBuilder()
    //            .RequireClaim(Permissions.Permission, permissionNames)
    //            .Build();

    //        return Task.FromResult(policy);
    //    }
    //}
}
