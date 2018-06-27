using System;
using System.Collections.Generic;
using System.Linq;
using GeekBudget.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GeekBudget.Entities
{
    public class ServiceResult : ServiceResult<object>
    {
        public ServiceResult(ServiceResultStatus status) : base(status){}
        
        public ServiceResult(ServiceResultStatus status, IEnumerable<Error> errors) : base(status, errors){}
        
        public ServiceResult(ServiceResultStatus status, Error error) : this(status, new List<Error>{error}){}

        public new static ServiceResult From<T>(ServiceResult<T> result)
        {
            return new ServiceResult(result.Status, result.Errors);
        }
    }
    
    public class ServiceResult<T>
    {
        public ServiceResultStatus Status { get; set; }
        public Error[] Errors { get; set; }
        public T Data { get; set; }

        public bool Succeeded => Status.Equals(ServiceResultStatus.Success);
        public bool Failed => Status.Equals(ServiceResultStatus.Failure);
        
        public ServiceResult(ServiceResultStatus status)
        {
            Status = status;
        }
        
        public ServiceResult(ServiceResultStatus status, IEnumerable<Error> errors) : this(status)
        {
            Errors = errors.ToArray();
        }
        
        public ServiceResult(ServiceResultStatus status, Error error) : this(status, new List<Error>{error}){}
        
        public ServiceResult(ServiceResultStatus status, IEnumerable<Error> errors, T data) : this(status, errors)
        {
            Data = data;
        }
        
        public ServiceResult(ServiceResultStatus status, Error error, T data) : this(status, new List<Error>{error}, data){}

        public ServiceResult(ServiceResultStatus status, T data) : this(status)
        {
            Data = data;
        }
        
        public ServiceResult(T data) : this(ServiceResultStatus.Success, data) {}
        
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