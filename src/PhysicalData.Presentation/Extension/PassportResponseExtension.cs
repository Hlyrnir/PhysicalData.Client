using Passport.Contract.v01.Response.Passport;
using PassportCheckpoint.Interface;

namespace PhysicalData.Presentation.Extension
{
    internal static class PassportResponseExtension
    {
        internal static IPassport ParseToModel(this PassportResponse rspnPassport, IEnumerable<IPassportVisa> enumPassportVisa)
        {
            return new Model.Passport
            {
                EmailAddress = "--",
                ExpiredAt = rspnPassport.ExpiredAt,
                HolderId = rspnPassport.HolderId,
                IsAuthority = rspnPassport.IsAuthority,
                IsEnabled = rspnPassport.IsEnabled,
                Visa = enumPassportVisa
            };
        }
    }
}
