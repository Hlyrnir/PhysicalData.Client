namespace PhysicalData.Presentation.Interface
{
    public interface ITokenStorageService
    {
        ValueTask<string?> ReadAuthenticationTokenAsync(CancellationToken tknCancellation);
        ValueTask<string?> ReadProviderAsync(CancellationToken tknCancellation);
        ValueTask<string?> ReadRefreshTokenAsync(CancellationToken tknCancellation);

        ValueTask<bool> WriteAuthenticationTokenAsync(string? sToken, CancellationToken tknCancellation);
        ValueTask<bool> WriteProviderAsync(string? sToken, CancellationToken tknCancellation);
        ValueTask<bool> WriteRefreshTokenAsync(string? sToken, CancellationToken tknCancellation);

        ValueTask<bool> ResetTokenAsync(CancellationToken tknCancellation);
    }
}
