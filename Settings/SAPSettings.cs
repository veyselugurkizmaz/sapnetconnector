
namespace SAP.DataAccess.Settings
{
    public class SAPSettings
    {
        private static string _userName = "username",
                  _password = "password",
                  _language = "EN",
                  _client = "000",
                  _applicationServer = "0.0.0.0",
                  _systemNumber = "00";

        protected static string UserName { get { return _userName; } }
        protected static string Password { get { return _password; } }
        protected static string Language { get { return _language; } }
        protected static string Client { get { return _client; } }
        protected static string ApplicationServer { get { return _applicationServer; } }
        protected static string SystemNumber { get { return _systemNumber; } }

    }
}
