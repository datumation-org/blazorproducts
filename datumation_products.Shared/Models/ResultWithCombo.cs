using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace datumation_products.Shared.Models {
    public class ResultWithCombo {
        public int NPI { get; set; }
        public string NamePrefix { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string NameSuffix { get; set; }
        public string Credential { get; set; }
        public string Gender { get; set; }
        public string EnumerationDate { get; set; }
        public string LastUpdateDate { get; set; }
        public string SoleProprietor { get; set; }
        public string MailingAddressStreet1 { get; set; }
        public string MailingAddressStreet2 { get; set; }
        public string MailingAddressCity { get; set; }
        public string MailingAddressState { get; set; }
        public string MailingAddressZip { get; set; }
        public string MailingAddressCounty { get; set; }
        public string MailingAddressLatitude { get; set; }
        public string MailingAddressLongitude { get; set; }
        public string MailingAddressCoordinateType { get; set; }
        public string MailingAddressPhone { get; set; }
        public string MailingAddressFax { get; set; }
        public string PracticeAddressStreet1 { get; set; }
        public string PracticeAddressStreet2 { get; set; }
        public string PracticeAddressCity { get; set; }
        public string PracticeAddressState { get; set; }
        public string PracticeAddressZip { get; set; }
        public string PracticeAddressCounty { get; set; }
        public string PracticeAddressLatitude { get; set; }
        public string PracticeAddressLongitude { get; set; }
        public string PracticeAddressCoordinateType { get; set; }
        public string PracticeAddressPhone { get; set; }
        public string PracticeAddressFax { get; set; }
        public string ProviderType { get; set; }
        public string ProviderSpecialty { get; set; }
        public string ProviderSubSpecialty { get; set; }
        public string PrimaryLicenseNumber { get; set; }
        public string PrimaryLicenseState { get; set; }
        public string Pt { get; set; }

        public int CountByCombo { get; set; }
    }
}