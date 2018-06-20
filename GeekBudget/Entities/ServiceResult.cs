using System;
using System.Collections.Generic;
using GeekBudget.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GeekBudget.Entities
{
    public class ServiceResult : ServiceResult<object>
    {
        public ServiceResult(Enums.ServiceResultStatus status) : base(status){}
        
        public ServiceResult(Enums.ServiceResultStatus status, IEnumerable<Error> errors) : base(status, errors){}
        
        public ServiceResult(Enums.ServiceResultStatus status, Error error) : this(status, new List<Error>{error}){}

        public static ServiceResult From<T>(ServiceResult<T> result)
        {
            return new ServiceResult(result.Status, result.Errors);
        }
    }
    
    public class ServiceResult<T>
    {
        public Enums.ServiceResultStatus Status { get; set; }
        public IEnumerable<Error> Errors { get; set; }
        public T Data { get; set; }

        public bool Succeeded => Status.Equals(Enums.ServiceResultStatus.Success);
        public bool Failed => Status.Equals(Enums.ServiceResultStatus.Failure);
        
        public ServiceResult(Enums.ServiceResultStatus status)
        {
            Status = status;
        }
        
        public ServiceResult(Enums.ServiceResultStatus status, IEnumerable<Error> errors) : this(status)
        {
            Errors = errors;
        }
        
        public ServiceResult(Enums.ServiceResultStatus status, Error error) : this(status, new List<Error>{error}){}
        
        public ServiceResult(Enums.ServiceResultStatus status, IEnumerable<Error> errors, T data) : this(status, errors)
        {
            Data = data;
        }
        
        public ServiceResult(Enums.ServiceResultStatus status, Error error, T data) : this(status, new List<Error>{error}, data){}

        public ServiceResult(Enums.ServiceResultStatus status, T data) : this(status)
        {
            Data = data;
        }
        
        public ServiceResult(T data) : this(Enums.ServiceResultStatus.Success, data) {}
        
        public static ServiceResult<T> From<TAny>(ServiceResult<TAny> result)
        {
            return new ServiceResult<T>(result.Status, result.Errors);
        }
        
        public static ServiceResult<T> From<TAny>(ServiceResult<TAny> result, T data)
        {
            return new ServiceResult<T>(result.Status, result.Errors, data);
        }
    }
}