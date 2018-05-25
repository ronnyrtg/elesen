namespace TradingLicense.Model
{
    public class RegularExpressions
    {
        /// <summary>
        /// Email address regular expression
        /// </summary>
        public const string Email = @"[A-Za-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[A-Za-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[A-Za-z0-9](?:[A-Za-z0-9-]*[A-Za-z0-9])?\.)+[A-Za-z0-9](?:[A-Za-z0-9-]*[A-Za-z0-9])?";

        /// <summary>
        /// Html tag bracket regular expression
        /// </summary>
        public const string HtmlTag = @"[^<>]*";

        /// <summary>
        /// User Name tag bracket + whiteSpace regular expression
        /// </summary>
        public const string UserName = @"[^\s<>]*";

        /// <summary>
        /// Geater Then Zero
        /// </summary>        
        public const string Integer = @"/[1-9]\d*/";
    }
}
