using System.Threading;
using System.Threading.Tasks;

namespace PhysicalData.Presentation.Interface
{
    public interface IPreferenceStorageService
    {
        ValueTask<string?> ReadPreferredCultureNameAsync(CancellationToken tknCancellation);
        ValueTask<string?> ReadPreferredThemeNameAsync(CancellationToken tknCancellation);
        ValueTask<bool> ResetSettingAsync(CancellationToken tknCancellation);
        ValueTask<bool> WriteCultureNameAsync(string? sCultureName, CancellationToken tknCancellation);
        ValueTask<bool> WriteThemeNameAsync(string? sThemeName, CancellationToken tknCancellation);
    }
}