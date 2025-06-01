using Microsoft.Xrm.Sdk;
using DanielCabrasCrmProject.Interfaces;
using DataverseModel;
using Microsoft.Xrm.Sdk.Query;

namespace DanielCabrasCrmProject.Services
{
    public class LineService : ILineService
    {
        /// <summary>
        /// checks if a line is actually to update and updates it if needed
        /// </summary>
        /// <param name="childLine"></param>
        /// <param name="header"></param>
        /// <param name="service"></param>
        public Entity SetupLineToUpdate(EntityCollection childLine, DACa_Header header, IOrganizationService service, ITracingService tracer)
        {
            tracer.Trace("Entered SetupLineToUpdate Method");

            foreach (Entity child in childLine.Entities)
            {
                DACa_Line line = child.ToEntity<DACa_Line>();

                DACa_Line toUpdate = new DACa_Line
                {
                    LogicalName = line.LogicalName,
                    Id = line.Id
                };

                if (header.StateCode != null && line.StateCode != null && (int)header.StateCode != (int)line.StateCode)
                {
                    toUpdate.StateCode = (DACa_Line_StateCode)header.StateCode;
                }

                if (header.StatusCode != null && line.StatusCode != null && (int)header.StatusCode != (int)line.StatusCode)
                {
                    toUpdate.StatusCode = (DACa_Line_StatusCode)header.StatusCode;
                }

                SetStateAndStatusLine(service, toUpdate, tracer);
            }

            return null;
        }

        /// <summary>
        /// updates the lines that need to be updated
        /// </summary>
        /// <param name="service"></param>
        /// <param name="toUpdate"></param>
        public void SetStateAndStatusLine(IOrganizationService service, Entity toUpdate, ITracingService tracer)
        {
            tracer.Trace("Entered SetStateAndStatusLine Method");
            service.Update(toUpdate);
            tracer.Trace("Lines updated successfully");
        }

        /// <summary>
        /// this methods only gets triggered for the create of target to set status and state code
        /// </summary>
        /// <param name="line"></param>
        /// <param name="header"></param>
        /// <param name="traces"></param>
        public void SetLineDefaultsFromHeader(DACa_Line line, DACa_Header header, ITracingService tracer)
        {
            tracer.Trace("Entered SetLineDefaultsFromHeader Method");

            if (header.StateCode != null)
            {
                line.StateCode = (DACa_Line_StateCode)header.StateCode;
            }

            if (header.StatusCode != null)
            {
                line.StatusCode = (DACa_Line_StatusCode)header.StatusCode;
            }
        }

        /// <summary>
        /// gets the collection of lines associated with header that are then used in "CheckLineToUpdate"
        /// </summary>
        /// <param name="header"></param>
        /// <param name="service"></param>
        /// <returns></returns>
        public EntityCollection GetEntityCollectionLine(DACa_Header header, IOrganizationService service, ITracingService tracer)
        {
            tracer.Trace("Entered GetEntityCollectionLine Method");
            string[] lineColumnSet = 
            {
                DACa_Line.Fields.StatusCode,
                DACa_Line.Fields.StateCode

            };

            QueryExpression queryLine = new QueryExpression(DACa_Line.EntityLogicalName)
            {
                ColumnSet = new ColumnSet(lineColumnSet),
                Criteria = new FilterExpression
                {
                    Conditions =
                    {
                        new ConditionExpression(DACa_Header.Fields.DACa_HeaderId, ConditionOperator.Equal, header.Id)
                    }
                }
            };
            EntityCollection childLine = service.RetrieveMultiple(queryLine);

            tracer.Trace("Line entity collection retrieved");
            return childLine;
        }

    }
}