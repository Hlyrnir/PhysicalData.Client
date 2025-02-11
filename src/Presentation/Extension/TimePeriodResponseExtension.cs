﻿using PhysicalData.Contract.v01.Response.TimePeriod;
using Presentation.Model;

namespace Presentation.Extension
{
    internal static class TimePeriodResponseExtension
    {
        internal static TimePeriod ParseToModel(this TimePeriodByIdResponse rspnTimePeriod)
        {
            return new TimePeriod()
            {
                ConcurrencyStamp = rspnTimePeriod.ConcurrencyStamp,
                Id = rspnTimePeriod.Id,
                Magnitude = rspnTimePeriod.Magnitude,
                Offset = rspnTimePeriod.Offset,
                PhysicalDimensionId = rspnTimePeriod.PhysicalDimensionId
            };
        }

        internal static IEnumerable<TimePeriod> ParseToModel(this IEnumerable<TimePeriodByIdResponse> enumTimePeriod)
        {
            foreach (TimePeriodByIdResponse rspnTimePeriod in enumTimePeriod)
            {
                yield return rspnTimePeriod.ParseToModel();
            }
        }
    }
}
