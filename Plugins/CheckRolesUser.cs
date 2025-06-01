using Microsoft.Xrm.Sdk;
using System.Collections.Generic;
using System;
using DataverseModel;
using DanielCabrasCrmProject.Services;
using DanielCabrasCrmProject.Interfaces;

namespace DanielCabrasCrmProject
{
    public class CheckRolesUser : IPlugin
    {
        #region Secure/Unsecure Configuration Setup
        private string _secureConfig = null;
        private string _unsecureConfig = null;

        public CheckRolesUser(string unsecureConfig, string secureConfig)
        {
            _secureConfig = secureConfig;
            _unsecureConfig = unsecureConfig;
        }
        #endregion
        public void Execute(IServiceProvider serviceProvider)
        {
            ITracingService tracer = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = factory.CreateOrganizationService(context.UserId);

            IHeaderService headerService = new HeaderService();
            ISecurityRoleInterface securityRole = new SecurityRoleService();
            
            //this plugin check the security role of the user before allowing him to change the status code of a dataset that is inactive and closed
            //only users with sys admin and "daniel cabras - manager" can change it
            try
            {
                tracer.Trace("Plugin started successfully");

                Entity target = (Entity)context.InputParameters["Target"];

                string[] headerColumnSet =
                {
                    DACa_Header.Fields.StateCode, 
                    DACa_Header.Fields.StatusCode
                };
                DACa_Header header = headerService.GetHeader(service, target, headerColumnSet, tracer);

                if (header.StateCode != null && header.StateCode.Value == DACa_Header_StateCode.Active)
                {
                    return;
                }

                if (header.StateCode == null || header.StateCode.Value != DACa_Header_StateCode.Inactive ||
                    header.StatusCode == null || header.StatusCode.Value != DACa_Header_StatusCode.Closed)
                {
                    return;
                }

                tracer.Trace("Checking security roles for status reason operation");
                
                List<string> allowedRoles = new List<string>
                {
                    "Daniel Cabras - Manager",
                    "System Administrator"
                };

                bool hasAnyRole = securityRole.UserHasSecurityRole(service, context.UserId, allowedRoles, tracer);

                if (!hasAnyRole)
                {
                    tracer.Trace("User lacks required roles, throwing exception");
                    throw new InvalidPluginExecutionException(
                        $"You do not have permission to change the status reason for this record. " +
                        $"One of the following roles is required: {string.Join(", ", allowedRoles)}");
                }
            }
            catch (Exception e)
            {
                tracer.Trace($"Error in plugin: {e.Message}");
                throw new InvalidPluginExecutionException(e.Message);
            }
        }
    }
}

