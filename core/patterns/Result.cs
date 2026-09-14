using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend_admon.core.patterns
{
    public class Result<T>
    {
          public T Value { get; }
    public bool IsSuccess { get; }
    public string ErrorMessage { get; }
    public int statusCode;

    private Result(T value,int statusCode)
    {
        Value = value;
        IsSuccess = true;
        this.statusCode = statusCode;
    }

    private Result(string errorMessage,int statusCode,bool IsSuccess)
    {
        ErrorMessage = errorMessage;
        this.IsSuccess = IsSuccess;
        this.statusCode = statusCode;
    }

    public static Result<T> Success(T value,int statusCode) => new Result<T>(value,statusCode);
    public static Result<T> Failure(string errorMessage,int statusCode,bool IsSuccess = false) => new Result<T>(errorMessage,statusCode,IsSuccess);
    }
}