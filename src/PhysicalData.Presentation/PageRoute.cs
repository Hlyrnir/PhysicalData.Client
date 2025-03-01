using System;

namespace PhysicalData.Presentation
{
    internal static class PageRoute
    {
        public const string Index = "/";
        public const string Counter = "/counter";
        public const string Chart = "/chart";

        public static class Passport
        {
            public const string Holder = "/passport/overview";
        }

        public static class PhysicalDimension
        {
            private const string Root = "/physical_dimension";

            public const string List = $"{Root}";
            public const string Create = $"{Root}/create";
            public const string Edit = $"{Root}/edit/{{guPhysicalDimensionId:guid}}";
            public const string Delete = $"{Root}";

            public static string EditById(Guid guPhysicalDimensionId)
            {
                return $"{Root}/edit/{guPhysicalDimensionId}";
            }
        }

        public static class TimePeriod
        {
            private const string Root = "/time_period";

            public const string List = $"{Root}";
            public const string Create = $"{Root}/create";
            public const string Edit = $"{Root}/edit/{{guTimePeriodId:guid}}";
            public const string Delete = $"{Root}";

            public static string EditById(Guid guTimePeriodId)
            {
                return $"{Root}/edit/{guTimePeriodId}";
            }
        }
    }
}
