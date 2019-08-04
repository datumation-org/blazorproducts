using System;
using System.Collections.Generic;

namespace datumation_products.Shared.Models {
    public partial class AspNetUsers {
        public AspNetUsers () {
            AspNetUserClaims = new HashSet<AspNetUserClaims> ();
            AspNetUserLogins = new HashSet<AspNetUserLogins> ();
            // AspNetUserRoles = new HashSet<AspNetUserRoles>();
        }

        public string Id { get; set; }
        public string Email { get; set; }
        // public int EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string PhoneNumber { get; set; }
        // public int PhoneNumberConfirmed { get; set; }
        public string UserCompany { get; set; }
        // public int TwoFactorEnabled { get; set; }
        public DateTime? LockoutEndDateUtc { get; set; }
        //  public int LockoutEnabled { get; set; }
        //  public int AccessFailedCount { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string NormalizedUserName { get; set; }
        public string ConcurrencyStamp { get; set; }
        public DateTime LockoutEnd { get; set; }
        public string NormalizedEmail { get; set; }

        public ICollection<AspNetUserClaims> AspNetUserClaims { get; set; }
        public ICollection<AspNetUserLogins> AspNetUserLogins { get; set; }
        // public ICollection<AspNetUserRoles> AspNetUserRoles { get; set; }
    }
}