using GeekBudget.Entities;
using Moq;
using Moq.Language.Flow;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GeekBudget.Tests
{
    public static class TestingExtensions
    {

        public static IReturnsResult<TSource> ReturnsAsyncServiceResult<TSource>(this ISetup<TSource, Task<ServiceResult>> flow) where TSource : class
        {
            return flow.ReturnsAsync(ServiceResult());
        }
        public static IReturnsResult<TSource> ReturnsAsyncServiceResult<TSource>(this ISetup<TSource, Task<ServiceResult>> flow, Enums.ServiceResultStatus status) where TSource : class
        {
            return flow.ReturnsAsync(ServiceResult(status));
        }

        public static IReturnsResult<TSource> ReturnsAsyncServiceResult<TSource, TResult>(this ISetup<TSource, Task<ServiceResult<TResult>>> flow, TResult result) where TSource : class
        {
            return flow.ReturnsAsync(ServiceResult(result));
        }

        public static IReturnsResult<TSource> ReturnsAsyncServiceResult<TSource, TResult>(this ISetup<TSource, Task<ServiceResult<TResult>>> flow, Enums.ServiceResultStatus status, TResult result) where TSource : class
        {
            return flow.ReturnsAsync(ServiceResult(status, result));
        }

        public static ServiceResult ServiceResult() => new ServiceResult(Enums.ServiceResultStatus.Success);
        public static ServiceResult ServiceResult(Enums.ServiceResultStatus status) => new ServiceResult(status);
        public static ServiceResult<T> ServiceResult<T>(T data) => new ServiceResult<T>(Enums.ServiceResultStatus.Success, data);
        public static ServiceResult<T> ServiceResult<T>(Enums.ServiceResultStatus status, T data) => new ServiceResult<T>(status, data);
    }
}
