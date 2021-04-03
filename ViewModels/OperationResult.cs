using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TWatchSKDesigner.ViewModels
{
    public class OperationResult
    {
        public OperationResult()
        {
            IsSuccess = true;
            ErrorMessage = "";
        }

        public OperationResult(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class OperationResult<T> : OperationResult
    {
        public OperationResult(T data)
        {
            IsSuccess = true;
            Data = data;
        }

        public OperationResult(string errorMessage)
        {
            ErrorMessage = errorMessage;
            Data = default;
        }

        public T? Data { get; set; }
    }
}
