using Microsoft.Xrm.Sdk;
using System;
using DanielCabrasCrmProject.Interfaces;
using DanielCabrasCrmProject.Services;
using DataverseModel;

namespace DanielCabrasCrmProject.Plugins
{
    public class CopyHeaderButton : IPlugin
    {
        #region Secure/Unsecure Configuration Setup
        private string _secureConfig = null;
        private string _unsecureConfig = null;

        public CopyHeaderButton(string unsecureConfig, string secureConfig)
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

            //this plugin copies the data from one header and make a new on with a new unique identifier, it does not copy the associates lines
            try
            {
                tracer.Trace("Plugin started successfully");

                string entityId = (string)context.InputParameters["entityid"];
                string entityName = (string)context.InputParameters["entityname"];


                string[] headerColumnSet =
                {
                    DACa_Header.Fields.StateCode,
                    DACa_Header.Fields.StatusCode,
                    DACa_Header.Fields.DACa_Last_Check_DT,
                    DACa_Header.Fields.DACa_Next_Check_DT,
                    DACa_Line.Fields.DACa_Account_Id
                };
                DACa_Header header = headerService.GetHeaderFromPara(service, entityName, entityId, headerColumnSet, tracer);

                headerService.CreateNewHeaderRecord(header, service, tracer);
                tracer.Trace("plugin Exited");
            }
            catch (Exception e)
            {
                throw new InvalidPluginExecutionException(e.Message);
            }
        }
    }
}