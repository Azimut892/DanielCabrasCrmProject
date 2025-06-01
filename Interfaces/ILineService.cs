using DataverseModel;
using Microsoft.Xrm.Sdk;

namespace DanielCabrasCrmProject.Interfaces
{
    public interface ILineService
    {
        Entity SetupLineToUpdate(EntityCollection childLine, DACa_Header header, IOrganizationService service, ITracingService tracer);
        void SetStateAndStatusLine(IOrganizationService service, Entity toUpdate, ITracingService tracer);
        void SetLineDefaultsFromHeader(DACa_Line line, DACa_Header header, ITracingService tracer);
        EntityCollection GetEntityCollectionLine(DACa_Header header, IOrganizationService service, ITracingService tracer);
    }
}