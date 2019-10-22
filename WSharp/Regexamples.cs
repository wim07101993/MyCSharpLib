namespace WSharp
{
    /// <summary>Examples of regex strings.</summary>
    public static class Regexamples
    {
        /// <summary>
        /// Regex string that filters IP Addresess
        /// </summary>
        public const string IpAddressRegex = @"^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])$";
        public const string IdRegex = @".*I[D,d]$";
        public const string NameRegex = @".*Name$";
    }
}