using DanielCabrasCrmProject.Interfaces;
using DataverseModel;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk;
using System.Collections.Generic;
using System;
using Microsoft.Crm.Sdk.Messages;

namespace DanielCabrasCrmProject.Services
{
    public class HeaderService : IHeaderService
    {

        /// <summary>
        /// retrieve the header
        /// </summary>
        /// <param name="service"></param>
        /// <param name="target"></param>
        /// <param name="headerColumnSet"></param>
        /// <param name="traces"></param>
        /// <returns></returns>
        public DACa_Header GetHeader(IOrganizationService service, Entity target, string[] headerColumnSet, ITracingService tracer)
        {
            tracer.Trace($"Entered GetHeader Method");

            DACa_Header header = service.Retrieve(DACa_Header.EntityLogicalName, target.Id, new ColumnSet(headerColumnSet)).ToEntity<DACa_Header>();

            return header;
        }

        /// <summary>
        /// retrieve the headerReference
        /// </summary>
        /// <param name="service"></param>
        /// <param name="line"></param>
        /// <param name="headerColumnSet"></param>
        /// <param name="traces"></param>
        /// <returns></returns>
        public DACa_Header GetHeaderRef(IOrganizationService service, DACa_Line line, string[] headerColumnSet, ITracingService tracer)
        {
            tracer.Trace("Entered GetHeaderRef Method");

            EntityReference headerRef = line.DACa_Header_Id;

            DACa_Header header = service.Retrieve(DACa_Header.EntityLogicalName, headerRef.Id, new ColumnSet(headerColumnSet)).ToEntity<DACa_Header>();

            return header;
        }

        /// <summary>
        /// retrive header from parameters given by action
        /// </summary>
        /// <param name="service"></param>
        /// <param name="entityName"></param>
        /// <param name="entityId"></param>
        /// <param name="headerColumnSet"></param>
        /// <param name="traces"></param>
        /// <returns></returns>
        public DACa_Header GetHeaderFromPara(IOrganizationService service, string entityName, string entityId, string[] headerColumnSet, ITracingService tracer)
        {
            tracer.Trace("Entered GetHeaderFromPara Method");

            DACa_Header header = service.Retrieve(entityName, new Guid(entityId), new ColumnSet(headerColumnSet)).ToEntity<DACa_Header>();

            return header;
        }

        /// <summary>
        /// creates a new header dataset based on the base header
        /// </summary>
        /// <param name="header"></param>
        /// <param name="service"></param>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public void CreateNewHeaderRecord(DACa_Header header, IOrganizationService service, ITracingService tracer)
        {
            tracer.Trace("Entered CreateNewHeaderRecord Method");

            DACa_Header newHeader = new DACa_Header();

            List<string> excludedAttributes = new List<string>
            {
                DACa_Header.Fields.DACa_Header_Id,
                DACa_Header.Fields.StateCode,
                DACa_Header.Fields.StatusCode
            };

            foreach (KeyValuePair<string, object> attribute in header.Attributes)
            {
                if (!excludedAttributes.Contains(attribute.Key))
                {
                    newHeader.Attributes.Add(attribute);
                }
            }
            newHeader.Id = service.Create(newHeader);

            if (header.StateCode == null || header.StatusCode == null)
            {
                return;
            }
            SetStateRequest setStateRequest = new SetStateRequest
            {
                EntityMoniker = newHeader.ToEntityReference(),
                State = new OptionSetValue((int)header.StateCode),
                Status = new OptionSetValue((int)header.StatusCode)
            };
            service.Execute(setStateRequest);
        }
    }
}