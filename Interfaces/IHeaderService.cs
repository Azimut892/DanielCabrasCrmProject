using DataverseModel;
using Microsoft.Xrm.Sdk;

namespace DanielCabrasCrmProject.Interfaces
{
    public interface IHeaderService
    {
        DACa_Header GetHeader(IOrganizationService service, Entity target, string[] headerColumnSet, ITracingService tracer);
        DACa_Header GetHeaderRef(IOrganizationService service, DACa_Line line, string[] headerColumnSet, ITracingService tracer);

        DACa_Header GetHeaderFromPara(IOrganizationService service, string entityName, string entityId, string[] headerColumnSet, ITracingService tracer);
        void CreateNewHeaderRecord(DACa_Header header, IOrganizationService service, ITracingService tracer);
    }
}