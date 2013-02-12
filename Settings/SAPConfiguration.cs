using System.Collections.Generic;
using SAP.Middleware.Connector;

namespace SAP.DataAccess.Settings
{
    public class SAPConfiguration : IDestinationConfiguration
    {
        Dictionary<string, RfcConfigParameters> availableDestinations;
        RfcDestinationManager.ConfigurationChangeHandler changeHandler;

        public SAPConfiguration()
        {
            availableDestinations = new Dictionary<string, RfcConfigParameters>();
        }

        public RfcConfigParameters GetParameters(string destName)
        {
            RfcConfigParameters foundDestination;
            availableDestinations.TryGetValue(destName, out foundDestination);
            return foundDestination;
        }

        public bool ChangeEventsSupported()
        {
            return true;
        }

        public event RfcDestinationManager.ConfigurationChangeHandler ConfigurationChanged
        {
            add
            {
                changeHandler = value;
            }
            remove
            {
                //do nothing
            }
        }

        //allows adding or modifying a destination for a specific application server
        public void AddOrEditDestination(string destName, string user, string password, string language, string client, string applicationServer, string systemNumber)
        {
            RfcConfigParameters parameters = new RfcConfigParameters();
            parameters[RfcConfigParameters.Name] = destName;
            parameters[RfcConfigParameters.PeakConnectionsLimit] = "10";
            parameters[RfcConfigParameters.PoolSize] = "35";
            parameters[RfcConfigParameters.IdleTimeout] = "1";
            parameters[RfcConfigParameters.User] = user;
            parameters[RfcConfigParameters.Password] = password;
            parameters[RfcConfigParameters.Client] = client;
            parameters[RfcConfigParameters.Language] = language;
            parameters[RfcConfigParameters.AppServerHost] = applicationServer;
            parameters[RfcConfigParameters.SystemNumber] = systemNumber;
            availableDestinations[destName] = parameters;
        }

        //removes the destination that is known under the given name
        public void RemoveDestination(string destName)
        {
            if (destName != null && availableDestinations.Remove(destName))
            {
                changeHandler(destName, new RfcConfigurationEventArgs(RfcConfigParameters.EventType.DELETED));
            }
        }
    }
}
