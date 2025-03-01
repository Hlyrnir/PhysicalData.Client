using Passport.Contract.v01.Response.PassportVisa;
using PassportCheckpoint.Interface;
using System.Collections.Generic;

namespace PhysicalData.Presentation.Extension
{
    internal static class PassportVisaResponseExtension
    {
        internal static IPassportVisa ParseToModel(this PassportVisaResponse rspnPassportVisa)
        {
            return new Model.PassportVisa
            {
                Level = rspnPassportVisa.Level,
                Name = rspnPassportVisa.Name
            };
        }

        internal static IEnumerable<IPassportVisa> ParseToModel(this IEnumerable<PassportVisaResponse> enumPassportVisa)
        {
            foreach (PassportVisaResponse rspnPassportVisa in enumPassportVisa)
            {
                yield return rspnPassportVisa.ParseToModel();
            }
        }
    }
}
