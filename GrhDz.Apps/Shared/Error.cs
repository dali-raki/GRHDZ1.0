using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrhDz.Apps.Shared
{
    public  class Error
    {
        public static readonly Error None = new(string.Empty, string.Empty, ErrorType.Failure);

        public static readonly Error NullValue = new(
            "General.Null",
            "Null value was provided",
            ErrorType.Failure);

        private Error(string code, string description, ErrorType type)
        {
            Code = code;
            Description = description;
            Type = type;
        }

        public string Code { get; }

        public string Description { get; }

        public ErrorType Type { get; }

        public static Error Failure(string code, string description) =>
            new(code, description, ErrorType.Failure);

        public static Error NotFound(string code, string description) =>
            new(code, description, ErrorType.NotFound);

        public static Error Problem(string code, string description) =>
            new(code, description, ErrorType.Problem);

        public static Error Conflict(string code, string description) =>
            new(code, description, ErrorType.Conflict);

        public static Error Exception(Exception ex) =>
            new("Error.Exception", ex.Message, ErrorType.Exception);

        public static Error Validation(string description) =>
            new("Error.Validation", description, ErrorType.Validation);
    }
}
