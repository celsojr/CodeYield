namespace CodeYield.Common.Extensions
{
    /// <summary>
    /// Extension methods for masking sensitive data in strings,
    /// useful for structured logging where PII must be partially visible.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Masks an email address, showing the first character of the local part
        /// and the full domain. Example: <c>j***@example.com</c>.
        /// </summary>
        public static string MaskEmail(this string email)
        {
            var atIndex = email.IndexOf('@');
            if (atIndex < 1) return "***";

            var local = email[..atIndex];
            var domain = email[atIndex..];

            if (local.Length == 1)
                return $"*{domain}";

            return $"{local[0]}{new string('*', local.Length - 1)}{domain}";
        }

        /// <summary>
        /// Masks a credit card number, showing only the first four and last four digits.
        /// Example: <c>4111-****-****-1111</c>.
        /// </summary>
        public static string MaskCreditCard(this string cardNumber)
        {
            var digits = new string(cardNumber.Where(char.IsDigit).ToArray());
            if (digits.Length < 8)
                return new string('*', digits.Length);

            var first = digits[..4];
            var last = digits[^4..];
            var masked = new string('*', digits.Length - 8);

            return $"{first}-{masked[..4]}-{masked[4..]}-{last}";
        }

        /// <summary>
        /// Masks a phone number, showing only the last four digits.
        /// Example: <c>******1234</c>.
        /// </summary>
        public static string MaskPhone(this string phone)
        {
            var digits = new string(phone.Where(char.IsDigit).ToArray());
            if (digits.Length < 4)
                return new string('*', digits.Length);

            var visible = digits[^4..];
            var masked = new string('*', digits.Length - 4);
            return $"{masked}{visible}";
        }

        /// <summary>
        /// Masks a string by replacing characters between <paramref name="visibleStart"/>
        /// and <paramref name="visibleEnd"/> with asterisks.
        /// Example: <c>"ABCDEFGH".MaskMiddle(2, 5)</c> → <c>"AB***FGH"</c>.
        /// </summary>
        public static string MaskMiddle(this string value, int visibleStart, int visibleEnd)
        {
            if (visibleStart < 0) throw new ArgumentOutOfRangeException(nameof(visibleStart));
            if (visibleEnd > value.Length) throw new ArgumentOutOfRangeException(nameof(visibleEnd));
            if (visibleStart >= visibleEnd) return value;

            var before = value[..visibleStart];
            var middle = new string('*', visibleEnd - visibleStart);
            var after = value[visibleEnd..];
            return $"{before}{middle}{after}";
        }
    }
}
