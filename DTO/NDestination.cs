using SAP.Middleware.Connector;

namespace SAP.DataAccess.DTO
{
    public class NDestination
    {
        public string Name { get; set; }
        public RfcConfigParameters Parameters { get; set; }
    }
}
