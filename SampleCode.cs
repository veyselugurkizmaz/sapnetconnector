using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAP.DataAccess;
using SAP.DataAccess.DTO;
using SAP.Middleware.Connector;

namespace SAPIntegration.Business
{
    public class SAPOperations
    {
        SAPConnection connection = new SAPConnection("SAPConnection", new SAPConnectionParameter
            {
                ApplicationServer = "0.0.0.0",
                UserName = "USERNAME",
                Password = "PASSWORD"
            });

        protected List<Customer> GetCustomers()
        {
            List<Customer> customerList = new List<Customer>();

            FunctionResult customerResult =  connection.ExecuteSelect("FUNCTION_NAME",
                 new List<NParameter>
                {
                    new NParameter{Name = "PARAMETER_NAME", Value="PARAMETER_VALUE"}
                },
                 new List<string>
                {
                    "TABLE_NAME1","TABLE_NAME2"
                });
            if (customerResult.IsSuccess)
            {
                IRfcTable customersTable = (IRfcTable)customerResult.Data[0];
                if (customersTable.ElementCount > 0 && customersTable.RowCount > 0)
                {
                    customersTable.CurrentIndex = 0;
                    for (int i = 0; i < customersTable.RowCount; i++)
                    {
                        customerList.Add(new Customer
                            {
                                CustomerId = customersTable.GetString("CUSTOMER_ID"),
                                Name = customersTable.GetString("CUSTOMER_NAME")                                
                            });

                        if (customersTable.CurrentIndex < (customersTable.RowCount - 1))
                        {
                            customersTable.CurrentIndex++;
                        }
                    }
                }
            }
            return customerList;
        }
    }
}
