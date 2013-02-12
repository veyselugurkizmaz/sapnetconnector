using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using SAP.DataAccess.DTO;
using SAP.DataAccess.Settings;
using SAP.Middleware.Connector;

namespace SAP.DataAccess
{
    public class SAPConnection : SAPSettings
    {
        #region Variables

        private string _destinationName;
        private SAPConfiguration _config;
        private RfcDestination _destination;
        private IRfcFunction _function;

        #endregion Variables

        #region Constructors

        public SAPConnection(string destinationName)
        {
            _destinationName = destinationName;

            _config = new SAPConfiguration();

            RfcDestinationManager.RegisterDestinationConfiguration(_config);

            _config.AddOrEditDestination(_destinationName,
                UserName,
                Password,
                Language,
                Client,
                ApplicationServer,
                SystemNumber);
            _destination = RfcDestinationManager.GetDestination(_destinationName);
        }

        #endregion Constructors

        public void RemoveDestination(string destName)
        {
            _config.RemoveDestination(destName);
        }

        public void UnregisterDestinationConfiguration()
        {
            RfcDestinationManager.UnregisterDestinationConfiguration(_config);
        }

        #region Execute Methods

        public FunctionResult ExecuteInsert(string functionName, List<NTable> parameters, List<string> tableNames)
        {
            try
            {
                _function = _destination.Repository.CreateFunction(functionName);

                for (int i = 0; i < tableNames.Count; i++)
                {
                    RfcStructureMetadata metaData = _destination.Repository.GetStructureMetadata(parameters[i].StructureName);

                    IRfcTable tblInput = _function.GetTable(tableNames[i]);

                    foreach (DataRow row in parameters[i].Parameters.Rows)
                    {
                        IRfcStructure structRow = metaData.CreateStructure();

                        foreach (DataColumn column in parameters[i].Parameters.Columns)
                        {
                            object obj = row[column];
                            structRow.SetValue(column.ToString(), obj);
                        }

                        tblInput.Append(structRow);
                    }
                }

                RfcSessionManager.BeginContext(_destination);
                _function.Invoke(_destination);

                IRfcTable returnTable = _function.GetTable("NOTESRETURN");

                return new FunctionResult
                {
                    IsSuccess = true,
                    Data = new List<object> { returnTable }
                };
            }
            catch (Exception ex)
            {
                return new FunctionResult
                {
                    IsSuccess = false,
                    ErrorMessage = ex.ToString()
                };
            }
        }

        public FunctionResult ExecuteSelect(string functionName, List<NParameter> parameters, List<string> tableNames)
        {
            try
            {
                _function = _destination.Repository.CreateFunction(functionName);

                foreach (NParameter param in parameters)
                    _function.SetValue(param.Name, param.Value);

                _function.Invoke(_destination);

                List<object> tables = tableNames.Select(table => _function.GetTable(table)).Cast<object>().ToList();

                return new FunctionResult()
                {
                    IsSuccess = true,
                    Data = tables
                };
            }
            catch (Exception ex)
            {
                return new FunctionResult
                {
                    IsSuccess = false,
                    ErrorMessage = ex.ToString()
                };
            }
        }

        #endregion Execute Methods
    }
}