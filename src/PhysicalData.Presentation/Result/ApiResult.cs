using System;
using System.Threading.Tasks;

namespace PhysicalData.Presentation.Result
{
    public enum MessageResultState : byte
    {
        Failure,
        Success
    }

    public readonly struct ApiResult<T>
    {
        private readonly MessageResultState enumState;
        private readonly T? gValue;

        private static ApiError msgDefault = new ApiError() { Code = "API_RESULT", Description = "DEFAULT" };
        private readonly ApiError msgError = msgDefault;

        public ApiResult(T gValue)
        {
            enumState = MessageResultState.Success;
            this.gValue = gValue;
            msgError = msgDefault;
        }

        public ApiResult(ApiError msgError)
        {
            enumState = MessageResultState.Failure;
            gValue = default;
            this.msgError = msgError;
        }

        public static implicit operator ApiResult<T>(T gValue)
        {
            return new ApiResult<T>(gValue);
        }

        public bool IsFailed
        {
            get
            {
                if (enumState == MessageResultState.Failure)
                    return true;

                return false;
            }
        }

        public bool IsSuccess
        {
            get
            {
                if (enumState == MessageResultState.Success)
                    return true;

                return false;
            }
        }

        public R Match<R>(Func<ApiError, R> MethodIfIsFailed, Func<T, R> MethodIfIsSuccess)
        {
            if (MethodIfIsSuccess is null || MethodIfIsFailed is null)
                throw new NotImplementedException("Match function is not defined.");

            if (gValue is null && msgError == msgDefault)
                throw new InvalidOperationException("No result was found.");

            if (IsSuccess)
                return MethodIfIsSuccess!(gValue!);

            return MethodIfIsFailed!(msgError);
        }

        public async Task<R> MatchAsync<R>(Func<ApiError, R> MethodIfIsFailed, Func<T, Task<R>> MethodIfIsSuccess)
        {
            if (MethodIfIsSuccess is null || MethodIfIsFailed is null)
                throw new NotImplementedException("Match function is not defined.");

            if (gValue is null && msgError == msgDefault)
                throw new InvalidOperationException("No result was found.");

            if (enumState == MessageResultState.Success)
                return await MethodIfIsSuccess(gValue!);

            return MethodIfIsFailed(msgError!);
        }
    }
}
