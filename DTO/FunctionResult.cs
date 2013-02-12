using System.Collections.Generic;

namespace SAP.DataAccess.DTO
{
    public class FunctionResult
    {
        public bool IsSuccess { get; set; }

        public List<object> Data { get; set; }

        public string ErrorMessage { get; set; }
    }
}