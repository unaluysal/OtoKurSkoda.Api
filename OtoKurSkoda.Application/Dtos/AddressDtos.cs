using OtoKurSkoda.Application.Defaults;
using OtoKurSkoda.Domain.Entitys;

namespace OtoKurSkoda.Application.Dtos
{
    public class AddressDto : BaseDto
    {
        public Guid UserId { get; set; }
        public AddressType Type { get; set; }
        public string Title { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string AddressLine { get; set; }
        public string? PostalCode { get; set; }
        public string? CompanyName { get; set; }
        public string? TaxNumber { get; set; }
        public string? TaxOffice { get; set; }
        public bool IsDefault { get; set; }
    }

    public class CreateAddressRequest
    {
        public AddressType Type { get; set; }
        public string Title { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string AddressLine { get; set; }
        public string? PostalCode { get; set; }
        public string? CompanyName { get; set; }
        public string? TaxNumber { get; set; }
        public string? TaxOffice { get; set; }
        public bool IsDefault { get; set; }
    }

    public class UpdateAddressRequest : CreateAddressRequest
    {
        public Guid Id { get; set; }
    }
}
