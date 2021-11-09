using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TWatchSKDesigner.Models
{
    public class Result
    {
        public bool IsSuccess { get; set; }
        public string? Code { get; set; }
        public string? ErrorMessage { get; set; }

        public void OnException(Exception ex)
        {
            IsSuccess = false;
            Code = "ERROR";
            ErrorMessage = ex.ToString();
        }

        public void OnSuccess()
        {
            IsSuccess = true;
            Code = "OK";
            ErrorMessage = null;
        }

        internal Result OnError(string code, string errorMessage)
        {
            IsSuccess = false;
            Code = code;
            ErrorMessage = errorMessage;

            return this;
        }
    }

    public class Result<T> : Result
    {
        public T? Data { get; set; }

        private new void OnSuccess() { }

        public void OnSuccess(T data)
        {
            Data = data;
            IsSuccess = true;
            Code = "OK";
            ErrorMessage = null;
        }
    }
}
