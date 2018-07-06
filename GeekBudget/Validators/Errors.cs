using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeekBudget.Models;

namespace GeekBudget.Validators
{
    public static class Errors
    {
        // Tab
        public static Error TabIdIsRequired => new Error{ Id = 100, Description = $"Tab Id is required!" };
        public static Error TabNameIsRequired => new Error{ Id = 101, Description = $"Tab Name is required!" };
        public static Error TabTypeIsRequired => new Error { Id = 102, Description = $"Tab Type is required!" };
        public static Error TabWithIdDoesNotExist(int id) => new Error { Id = 103, Description = $"No Tab with id '{id}' was found!" };

        // Operation
        public static Error OperationIdIsRequired => new Error { Id = 200, Description = $"Operation Id is required!" };
        public static Error OperationFromIsRequired => new Error { Id = 201, Description = $"'From' id can't be null!" };
        public static Error OperationToIsRequired => new Error { Id = 202, Description = $"'To' id can't be null!" };
        public static Error OperationAmountIsRequired => new Error { Id = 203, Description = $"'Amount' is required!" };
        public static Error OperationNotAllowed => new Error { Id = 204, Description = $"Can't add operation with that tab target types!" };
        public static Error OperationWithIdDoesNotExist(int id) => new Error { Id = 205, Description = $"No Operation with id '{id}' was found!" };
        public static Error OperationAlreadyExist(int operationId, int tabId) => new Error { Id = 206, Description = $"Operation with id '{operationId}' already exists on Tab with id '{tabId}'!" };

    }
}
