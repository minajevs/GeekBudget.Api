using System;
using System.Linq;
using GeekBudget.Domain.Operations;

namespace GeekBudget.DataAccess.Operations
{
    public static class OperationQueries
    {
        public static IQueryable<Operation> ApplyFilter(this IQueryable<Operation> operations, OperationFilter filter)
        {
            var query = operations;

            if (filter.Id != null)
                query = query.Where(x => x.Id == filter.Id);

            if (filter.Comment != null)
                query = query.Where(x => x.Comment.IndexOf(filter.Comment, StringComparison.OrdinalIgnoreCase) >= 0); //Comment contains TODO: make more simple search with allowed errors

            if (filter.Amount != null)
            { //TODO: Add possibility to add nullable Min/Max values
                query = query.Where(x => x.Amount >= filter.Amount.Min);
                query = query.Where(x => x.Amount <= filter.Amount.Max);
            }

            if (filter.Currency != null)
                query = query.Where(x => x.Currency == filter.Currency); //TODO: must work with any case?

            if (filter.From != null)
                query = query.Where(x => x.From.Id == filter.From);

            if (filter.To != null)
                query = query.Where(x => x.To.Id == filter.To);

            if (filter.Date != null) //TODO: Add possibility to add nullable Min/Max values
                query = query.Where(x =>
                    x.Date >= filter.Date.Min &&
                    x.Date <= filter.Date.Max);

            return query;
        }
    }
}