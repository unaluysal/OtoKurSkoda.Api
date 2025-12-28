using OtoKurSkoda.Domain.Defaults;

namespace OtoKurSkoda.Domain.Entitys
{
    public class UserAddress :BaseEntity
    {
        public Guid UserId { get; set; }
        public virtual User User { get; set; }


        public AddressType Type { get; set; }    // Billing veya Shipping


        public string Title { get; set; }        // "Ev Adresi", "İş Yeri"
        public string FullName { get; set; }     // Alıcı adı
        public string Phone { get; set; }

        public string City { get; set; }
        public string District { get; set; }
        public string AddressLine { get; set; }
        public string? PostalCode { get; set; }

        // Kurumsal fatura için (sadece Billing tipinde dolu)
        public string? CompanyName { get; set; }
        public string? TaxNumber { get; set; }
        public string? TaxOffice { get; set; }
        public bool IsDefault { get; set; }

    }

    public enum AddressType
    {
        Shipping = 1,    // Kargo adresi
        Billing = 2      // Fatura adresi
    }
}
