namespace PhysicalData.Presentation
{
    internal static class EndpointRoute
    {
        private const string EndpointBase = "/api";

        internal static class Authentication
        {
            public const string Token = $"{EndpointBase}/token";
            public const string RefreshToken = $"{EndpointBase}/refresh";
            public const string Reset = $"{EndpointBase}/reset";
        }

        internal static class Passport
        {
            public const string Base = $"{EndpointBase}/passport";
            public const string Create = Base;
            public const string Delete = Base;
            public const string Update = Base;

            public const string Register = $"{Base}/register";

            public static string GetById(Guid guPassportIdToFind)
            {
                return $"{Base}/{guPassportIdToFind}";
            }
        }

        internal static class PassportHolder
        {
            private const string Base = $"{EndpointBase}/holder";
            public const string Create = Base;
            public const string Delete = Base;
            public const string Update = Base;
            public const string ConfirmEmailAddress = $"{Base}/confirm_email";
            public const string ConfirmPhoneNumber = $"{Base}/confirm_phone";

            public static string GetById(Guid guPassportHolderIdToFind)
            {
                return $"{Base}/{guPassportHolderIdToFind}";
            }
        }

        internal static class PassportToken
        {
            public const string Base = $"{EndpointBase}/token";
            public const string Create = Base;
            public const string Delete = Base;
            public const string GetUnspecific = Base;
            public const string Update = Base;

            public static string GetById(Guid guPassportTokenIdToFind)
            {
                return $"{Base}/{guPassportTokenIdToFind}";
            }
        }

        internal static class PassportVisa
        {
            public const string Base = $"{EndpointBase}/visa";
            public const string Create = Base;
            public const string Delete = Base;
            public const string GetUnspecific = Base;
            public const string GetByPassportId = $"{Base}/passport";
            public const string Update = Base;

            public static string GetById(Guid guPassportVisaIdToFind)
            {
                return $"{Base}/{guPassportVisaIdToFind}";
            }
        }

        internal static class PhysicalDimension
        {
            public const string Base = $"{EndpointBase}/physical_dimension";
            public const string Create = Base;
            public const string Delete = Base;
            public const string GetUnspecific = Base;
            public const string Update = Base;

            public static string GetById(Guid guId)
            {
                return $"{Base}/{guId}";
            }
        }

        internal static class TimePeriod
        {
            public const string Base = $"{EndpointBase}/time_period";
            public const string Create = Base;
            public const string Delete = Base;
            public const string GetUnspecific = Base;
            public const string Update = Base;

            public static string GetById(Guid guId)
            {
                return $"{Base}/{guId}";
            }
        }
    }
}
