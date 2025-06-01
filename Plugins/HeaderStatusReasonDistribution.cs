using Microsoft.Xrm.Sdk;
using System;
using DanielCabrasCrmProject.Interfaces;
using DanielCabrasCrmProject.Services;
using DataverseModel;

namespace DanielCabrasCrmProject
{
    public class HeaderStatusReasonDistribution : IPlugin
    {
        #region Secure/Unsecure Configuration Setup
        private string _secureConfig = null;
        private string _unsecureConfig = null;

        public HeaderStatusReasonDistribution(string unsecureConfig, string secureConfig)
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
            ILineService lineService = new LineService();

            //this plugin distributes the state and status code from parent header to its associated child lines setting them the same status and state
            try
            {
                tracer.Trace("Plugin started successfully");
                Entity target = (Entity)context.InputParameters["Target"];

                string[] headerColumnSet =
                {
                    DACa_Header.Fields.StateCode,
                    DACa_Header.Fields.StatusCode
                };

                if (context.MessageName.Equals("create"))
                {
                    tracer.Trace("Context Message 'Create'");

                    DACa_Line line = target.ToEntity<DACa_Line>();
                    DACa_Header header = headerService.GetHeaderRef(service, line, headerColumnSet, tracer);

                    lineService.SetLineDefaultsFromHeader(line, header, tracer);
                } 
                else if(context.MessageName.Equals("update"))
                {
                    tracer.Trace("Context Message 'Update'");

                    DACa_Header header = headerService.GetHeader(service, target, headerColumnSet, tracer);
                    EntityCollection childLine = lineService.GetEntityCollectionLine(header, service, tracer);

                    lineService.SetupLineToUpdate(childLine, header, service, tracer);
                }
                tracer.Trace("plugin Exited");
            }
            catch (Exception e)
            {
                throw new InvalidPluginExecutionException(e.Message);
            }
        }
    }
}
    