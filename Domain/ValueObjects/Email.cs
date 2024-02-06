using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Domain.ValueObjects
{
    public class Email : IEquatable<Email>
    {
        public string Address { get; }
        public string Domain { get; }

        public Email(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Email address cannot be null or empty", nameof(value));
            }

            // Regular expression pattern for email validation
            string emailPattern = @"^([a-zA-Z0-9._%+-]+)@([a-zA-Z0-9.-]+)\.([a-zA-Z]{2,})$";

            Match match = Regex.Match(value, emailPattern);
            if (!match.Success)
            {
                throw new ArgumentException("Invalid email address format", nameof(value));
            }

            Address = value.Trim();
            Domain = match.Groups[2].Value + "." + match.Groups[3].Value;
        }

        public bool Equals(Email? other)
        {
            return other != null && Address == other.Address;
        }
    }
}
