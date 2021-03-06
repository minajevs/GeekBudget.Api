﻿using System.Threading.Tasks;
using GeekBudget.Core;
using Moq;
using Moq.Language.Flow;

namespace GeekBudget.Application.Tests
{
    public static class TestingExtensions
    {

        public static IReturnsResult<TSource> ReturnsAsyncServiceResult<TSource>(this ISetup<TSource, Task<ServiceResult>> flow) where TSource : class
        {
            return flow.ReturnsAsync(ServiceResult());
        }
        public static IReturnsResult<TSource> ReturnsAsyncServiceResult<TSource>(this ISetup<TSource, Task<ServiceResult>> flow, ServiceResultStatus status) where TSource : class
        {
            return flow.ReturnsAsync(ServiceResult(status));
        }

        public static IReturnsResult<TSource> ReturnsAsyncServiceResult<TSource, TResult>(this ISetup<TSource, Task<ServiceResult<TResult>>> flow, TResult result) where TSource : class
        {
            return flow.ReturnsAsync(ServiceResult(result));
        }

        public static IReturnsResult<TSource> ReturnsAsyncServiceResult<TSource, TResult>(this ISetup<TSource, Task<ServiceResult<TResult>>> flow, ServiceResultStatus status, TResult result) where TSource : class
        {
            return flow.ReturnsAsync(ServiceResult(status, result));
        }

        public static ServiceResult ServiceResult() => new ServiceResult(ServiceResultStatus.Success);
        public static ServiceResult ServiceResult(ServiceResultStatus status) => new ServiceResult(status);
        public static ServiceResult<T> ServiceResult<T>(T data) => new ServiceResult<T>(ServiceResultStatus.Success, data);
        public static ServiceResult<T> ServiceResult<T>(ServiceResultStatus status, T data) => new ServiceResult<T>(status, data);
    }
}
