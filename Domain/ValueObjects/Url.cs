using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ValueObjects
{
    public class Url : IEquatable<Url>
    {
        public string Value { get; }

        public Url(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("URL cannot be null or empty", nameof(value));
            }

            // Check if the URL is a valid absolute URI
            if (!Uri.TryCreate(value, UriKind.Absolute, out _))
            {
                throw new ArgumentException("Invalid URL format", nameof(value));
            }

            Value = value.Trim();
        }
        public bool Equals(Url? other)
        {
            return other != null && Value == other.Value;
        }
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
        public override string ToString()
        {
            return Value;
        }
    }
}
