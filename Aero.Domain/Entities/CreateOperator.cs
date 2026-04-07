using Aero.Domain.Helpers;
using System;

namespace Aero.Domain.Entities;

public sealed class CreateOperator
{
       public short ComponentId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string ImagePath { get; set; } = string.Empty;
        public short RoleId { get; set; }
        public List<short> LocationIds { get; set; } = new List<short>();

        public CreateOperator() { }

        public CreateOperator(short componentId, string userId, string username, string password, string email, string title, string firstName, string middleName, string lastName, string phone, string imagePath, short roleId, List<short> locationIds)
        {
                ComponentId = componentId;
                UserId = ValidateRequiredString(userId, nameof(userId));
                Username = ValidateRequiredString(username, nameof(username));
                Password = ValidateRequiredString(password, nameof(password));
                Email = ValidateRequiredString(email, nameof(email));
                Title = ValidateRequiredString(title, nameof(title));
                FirstName = ValidateRequiredString(firstName, nameof(firstName));
                MiddleName = ValidateRequiredString(middleName, nameof(middleName));
                LastName = ValidateRequiredString(lastName, nameof(lastName));
                Phone = ValidateRequiredString(phone, nameof(phone));
                ImagePath = ValidateRequiredString(imagePath, nameof(imagePath));
                RoleId = roleId;
                LocationIds = locationIds ?? throw new ArgumentNullException(nameof(locationIds));
        }

        private static string ValidateRequiredString(string value, string field)
        {
                ArgumentException.ThrowIfNullOrWhiteSpace(value, field);
                var trimmed = value.Trim();
                var sanitized = trimmed.Replace("-", string.Empty).Replace("_", string.Empty).Replace(".", string.Empty).Replace(":", string.Empty).Replace("/", string.Empty).Replace("@", string.Empty);
                if (!RegexHelper.IsValidName(trimmed) && !RegexHelper.IsValidOnlyCharAndDigit(sanitized))
                {
                        throw new ArgumentException($"{field} invalid.", field);
                }

                return value;
        }
}
