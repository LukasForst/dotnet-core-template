using System;

namespace DataAccess.Model
{
    /// <summary>
    ///     Application user used for the authorization and authentication.
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global -- db entity without insert
    public class User : AbstractEntity
    {
        public User(string firstName, string lastName, string username, string passwordHash, string role)
        {
            FirstName = firstName;
            LastName = lastName;
            Username = username;
            PasswordHash = passwordHash;
            Role = role;
        }

        /// <summary>
        ///     User first name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        ///     User last name.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        ///     Username.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     Salted and hashed password.
        /// </summary>
        public string PasswordHash { get; set; }

        /// <summary>
        ///     Role name. Plain string that should correspond to the roles defined in Roles class.
        /// </summary>
        public string Role { get; set; }

        protected bool Equals(User other)
        {
            return Id == other.Id && FirstName == other.FirstName && LastName == other.LastName && Username == other.Username &&
                   PasswordHash == other.PasswordHash && Role == other.Role;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((User) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, FirstName, LastName, Username, PasswordHash, Role);
        }
    }
}