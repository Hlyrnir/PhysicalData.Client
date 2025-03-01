using System;

namespace PhysicalData.Presentation.Result
{
    public readonly struct ApiError
    {
        /// <summary>
        /// Gets or sets the code for this error.
        /// </summary>
        /// <value>
        /// The code for this error.
        /// </value>
        public required string Code { get; init; }

        /// <summary>
        /// Gets or sets the description for this error.
        /// </summary>
        /// <value>
        /// The description for this error.
        /// </value>
        public required string Description { get; init; }

        public override bool Equals(object? objCompareTo)
        {
            if (objCompareTo is ApiError)
            {
                ApiError apiError = (ApiError)objCompareTo;

                if (Code == apiError.Code && Description == apiError.Description)
                    return true;
            }

            return false;
        }

        public static bool operator ==(ApiError apiErrorA, ApiError apiErrorB)
        {
            return apiErrorA.Equals(apiErrorB);
        }

        public static bool operator !=(ApiError apiErrorA, ApiError apiErrorB)
        {
            return !apiErrorA.Equals(apiErrorB);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Code, Description);
        }
    }
}