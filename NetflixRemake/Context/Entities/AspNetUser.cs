﻿namespace Infrastructure.Entities
{
    public partial class AspNetUser
    {
        public AspNetUser()
        {
            AspNetUserClaims = new HashSet<AspNetUserClaim>();
            AspNetUserLogins = new HashSet<AspNetUserLogin>();
            AspNetUserTokens = new HashSet<AspNetUserToken>();
            Ratings = new HashSet<Rating>();
            UserMovies = new HashSet<UserMovie>();
            Roles = new HashSet<AspNetRole>();
        }

        public string Id { get; set; } = null!;
        public int AccessFailedCount { get; set; }
        public string? ConcurrencyStamp { get; set; }
        public string? Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool LockoutEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public string? NormalizedEmail { get; set; }
        public string? NormalizedUserName { get; set; }
        public string? PasswordHash { get; set; }
        public string? PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public string? SecurityStamp { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public string? UserName { get; set; }
        public decimal Bank { get; set; }

        public virtual ICollection<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual ICollection<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual ICollection<AspNetUserToken> AspNetUserTokens { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }
        public virtual ICollection<UserMovie> UserMovies { get; set; }

        public virtual ICollection<AspNetRole> Roles { get; set; }
    }
}
