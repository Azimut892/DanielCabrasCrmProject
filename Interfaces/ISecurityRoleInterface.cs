using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;

namespace DanielCabrasCrmProject.Interfaces
{
    public interface ISecurityRoleInterface
    {
        bool UserHasSecurityRole(IOrganizationService service, Guid userId, List<string> roleNames, ITracingService tracer);
        FilterExpression BuildRoleNameFilter(List<string> roleNames, ITracingService tracer);
        QueryExpression BuildUserRoleQuery(Guid userId, FilterExpression roleNameFilter, ITracingService tracer);
    }
}