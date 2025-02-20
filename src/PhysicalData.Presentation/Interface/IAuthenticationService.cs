using PassportCheckpoint.Interface;
using PhysicalData.Presentation.Result;

namespace PhysicalData.Presentation.Interface
{
    public interface IAuthenticationService
    {
        /// <summary>
        /// Request <see cref="IPassport"/> using jwt token of local storage.
        /// </summary>
        /// <param name="tknCancellation"></param>
        /// <returns></returns>
		ValueTask<ApiResult<IPassport>> FindPassport(CancellationToken tknCancellation);

        /// <summary>
        /// Initialize and store a jwt token using <see cref="ILogInCredential">ILogInCredential</see>.
        /// </summary>
        /// <param name="bwpCredential"></param>
        /// <param name="tknCancellation"></param>
        /// <returns></returns>
		ValueTask<ApiResult<bool>> InitializeBearerTokenAsync(IPassportCredential bwpCredential, CancellationToken tknCancellation);

        /// <summary>
        /// Read jwt token in local storage.
        /// </summary>
        /// <param name="tknCancellation"></param>
        /// <returns></returns>
        ValueTask<ApiResult<string>> ReadJwtTokenAsync(CancellationToken tknCancellation);


        /// <summary>
        /// Replace jwt token using refresh token of local storage.
        /// </summary>
        /// <param name="tknCancellation"></param>
        /// <returns>
        /// <see cref="bool">true</see>: Jwt token has been replaced.<br/>
        /// <see cref="bool">false</see>: Jwt token is not expired.<br/>
        /// <see cref="IApiError">IApiError</see>: Jwt token could not be replaced.
        /// </returns>
        ValueTask<ApiResult<bool>> RefreshBearerTokenAsync(CancellationToken tknCancellation);

        /// <summary>
        /// Remove jwt token from local storage.
        /// </summary>
        /// <param name="tknCancellation"></param>
        /// <returns></returns>
        ValueTask<ApiResult<bool>> ResetJwtTokenAsync(CancellationToken tknCancellation);
    }
}
