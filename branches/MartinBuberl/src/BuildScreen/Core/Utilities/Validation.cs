using System;
using System.Text.RegularExpressions;

namespace BuildScreen.Core.Utilities
{
    public static class Validation
    {
        /// <summary>
        /// Checks if the given string is a valid domain.
        /// </summary>
        /// <param name="value">The string to be checked.</param>
        /// <returns>A boolean value.</returns>
        public static bool IsDomain(string value)
        {
            if (string.IsNullOrEmpty(value))
                return false;

            Regex url = new Regex(@"^[\w\-_]+((\.[\w\-_]+)+([a-z]))?$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            return url.IsMatch(value);
        }

        /// <summary>
        /// Checks if a given string is a valid IP address.
        /// </summary>
        /// <param name="value">The string to be checked.</param>
        /// <returns>A boolean value.</returns>
        public static bool IsIPv4(string value)
        {
            if (string.IsNullOrEmpty(value))
                return false;

            Regex regex = new Regex(@"^([1-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])(\.([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])){3}$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            return regex.IsMatch(value);
        }

        /// <summary>
        /// Checks if the given string is a valid port number.
        /// </summary>
        /// <param name="value">The string to be checked.</param>
        /// <returns>A boolean value.</returns>
        public static bool IsPort(string value)
        {
            if (string.IsNullOrEmpty(value))
                return false;

            Regex numeric = new Regex(@"^[0-9]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

            if (numeric.IsMatch(value))
            {
                try
                {
                    if (Convert.ToInt32(value) < 65536)
                        return true;
                }
                catch (OverflowException)
                {
                }
            }

            return false;
        }
    }
}
