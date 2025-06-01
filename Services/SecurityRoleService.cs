using DataverseModel;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using DanielCabrasCrmProject.Interfaces;

namespace DanielCabrasCrmProject.Services
{
    public class SecurityRoleService : ISecurityRoleInterface
    {
        /// <summary>
        /// this method checks if the systemuser has the security role from a given list
        /// </summary>
        /// <param name="service"></param>
        /// <param name="userId"></param>
        /// <param name="roleNames"></param>
        /// <param name="traces"></param>
        /// <returns></returns>
        public bool UserHasSecurityRole(IOrganizationService service, Guid userId, List<string> roleNames, ITracingService tracer)
        {
            tracer.Trace("Entered UserHasSecurityRole Method");

            var roleNameFilter = BuildRoleNameFilter(roleNames, tracer);
            var query = BuildUserRoleQuery(userId, roleNameFilter, tracer);

            EntityCollection result = service.RetrieveMultiple(query);

            return result.Entities.Count > 0;
        }

        /// <summary>
        /// builds the filter for the security role
        /// </summary>
        /// <param name="roleNames"></param>
        /// <returns></returns>
        public FilterExpression BuildRoleNameFilter(List<string> roleNames, ITracingService tracer)
        {
            tracer.Trace("Entered BuildRoleNameFilter Method");
            var roleNameFilter = new FilterExpression(LogicalOperator.Or);

            foreach (string roleName in roleNames)
            {
                var condition = new ConditionExpression(Role.Fields.Name, ConditionOperator.Equal, roleName);
                roleNameFilter.Conditions.Add(condition);
            }
            return roleNameFilter;
        }

        /// <summary>
        /// builds the querry to link the security role to the systemuser
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleNameFilter"></param>
        /// <returns></returns>
        public QueryExpression BuildUserRoleQuery(Guid userId, FilterExpression roleNameFilter, ITracingService tracer)
        {
            tracer.Trace("Entered BuildUserRoleQuery Method");
            return new QueryExpression(SystemUserRoles.EntityLogicalName)
            {
                ColumnSet = new ColumnSet(SystemUserRoles.Fields.RoleId),
                Criteria = new FilterExpression
                {
                    FilterOperator = LogicalOperator.And,
                    Conditions =
                    {
                        new ConditionExpression(SystemUserRoles.Fields.SystemUserId, ConditionOperator.Equal, userId)
                    }
                },
                LinkEntities =
                {
                    new LinkEntity
                    {
                        LinkFromEntityName = SystemUserRoles.EntityLogicalName,
                        LinkFromAttributeName = SystemUserRoles.Fields.RoleId,
                        LinkToEntityName = Role.EntityLogicalName,
                        LinkToAttributeName = Role.Fields.RoleId,
                        EntityAlias = Role.EntityLogicalName,
                        Columns = new ColumnSet(Role.Fields.Name),
                        JoinOperator = JoinOperator.Inner,
                        LinkCriteria = roleNameFilter
                    }
                }
            };
        }
    }
}
