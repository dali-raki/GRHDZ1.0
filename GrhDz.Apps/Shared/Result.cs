using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrhDz.Apps.Shared
{
    public class Result
    {
        protected Result(bool isSuccess, Error error)
        {
            if (isSuccess && error != Error.None)
            {
                throw new InvalidCastException();
            }

            if (!isSuccess && error == Error.None)
            {
                throw new InvalidCastException();
            }

            IsSuccess = isSuccess;
            Error = error;
        }

        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;

        public Error Error { get; }

        public static Result Success() => new(true, Error.None);

        public static Result Failure(Error error) => new(false, error);

        public static Result<T> Success<T>(T value) => new(value, true, Error.None);

        public static Result<T> Failure<T>(Error error) => new(default!, false, error);

        public static implicit operator Result(Error error) =>
            Failure(error);

        public static Result<T> Create<T>(T? value) =>
            value is not null ? Success(value) : Failure<T>(Error.NullValue);
    }

    public class Result<T> : Result
    {
        private readonly T? _value;

        protected internal Result(T value, bool isSuccess, Error error) : base(isSuccess, error)
        {
            _value = value;
        }

        protected Result(bool isSuccess, Error error) : base(isSuccess, error)
        {
        }

        [NotNull]
        public T Value => IsSuccess
            ? _value!
            : throw new InvalidOperationException("The value of a failure result can not be accessed");

        public static implicit operator Result<T>(Error error) =>
            Failure<T>(error);

        public static implicit operator Result<T>(T? value) => Create(value);
    }

    public class SearchResult<T> : Result<T>
    {
        private readonly List<T>? _value;
        private readonly int count;

        public List<T>? Value => IsSuccess
            ? _value
            : throw new InvalidOperationException("The value of a failure result can not be accessed");

        protected internal SearchResult(int count, List<T> value, bool isSuccess, Error error) : base(isSuccess, error)
        {
            this.count = count;
            _value = value;
        }

        public int Count => IsSuccess
            ? count
            : throw new InvalidOperationException("The value of a failure result can not be accessed");

        public static SearchResult<T> Success<T>(int count, List<T> value) => new(count, value, true, Error.None);
        public static SearchResult<T> Failure(Error? error = null)
            => new(0, new List<T>(), false, error ?? Error.None);
        public static SearchResult<T> Create<T>(int count, T? value) =>
            (SearchResult<T>)(value is not null ? Success(value) : Failure<T>(Error.NullValue));

        private SearchResult(bool isSuccess, Error error) : base(isSuccess, error)
        {
        }

        public static implicit operator SearchResult<T>(Error error) =>
            new(false, error);
    }
}
