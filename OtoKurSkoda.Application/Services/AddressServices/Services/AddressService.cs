using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OtoKurSkoda.Application.Defaults;
using OtoKurSkoda.Application.Dtos;
using OtoKurSkoda.Application.Services.UserServices.Interfaces;
using OtoKurSkoda.Domain.Entitys;
using OtoKurSkoda.Infrastructure.UnitOfWork;

namespace OtoKurSkoda.Application.Services.AddressServices.Services
{
    public class AddressService : BaseApplicationService, IAddressService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AddressService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ServiceResult> GetUserAddressesAsync(Guid userId)
        {
            var repo = _unitOfWork.GetRepository<UserAddress>();
            var addresses = await repo.GetWhere(a => a.UserId == userId && a.Status == true).ToListAsync();
            var dtos = _mapper.Map<List<AddressDto>>(addresses.OrderByDescending(a => a.IsDefault).ThenBy(a => a.Title));
            return SuccessListDataResult(dtos, "ADDRESSES_FOUND", "Adresler listelendi.");
        }

        public async Task<ServiceResult> GetByIdAsync(Guid id, Guid userId)
        {
            var repo = _unitOfWork.GetRepository<UserAddress>();
            var address = await repo.GetFirstWhereAsync(a => a.Id == id && a.UserId == userId);

            if (address == null)
                return ErrorDataResult<AddressDto>(null, "ADDRESS_NOT_FOUND", "Adres bulunamadı.");

            var dto = _mapper.Map<AddressDto>(address);
            return SuccessDataResult(dto, "ADDRESS_FOUND", "Adres bulundu.");
        }

        public async Task<ServiceResult> CreateAsync(Guid userId, CreateAddressRequest request)
        {
            var repo = _unitOfWork.GetRepository<UserAddress>();

            var address = new UserAddress
            {
                UserId = userId,
                Type = request.Type,
                Title = request.Title,
                FullName = request.FullName,
                Phone = request.Phone,
                City = request.City,
                District = request.District,
                AddressLine = request.AddressLine,
                PostalCode = request.PostalCode,
                CompanyName = request.CompanyName,
                TaxNumber = request.TaxNumber,
                TaxOffice = request.TaxOffice,
                IsDefault = request.IsDefault
            };

            // Eğer varsayılan olarak işaretlendiyse, diğer aynı tipteki adreslerin varsayılanını kaldır
            if (request.IsDefault)
            {
                var existingDefaults = await repo.GetWhere(a => a.UserId == userId
                && a.Status
                && a.Type == request.Type && a.IsDefault).ToListAsync();
                foreach (var existingDefault in existingDefaults)
                {
                    existingDefault.IsDefault = false;
                    repo.UpdateWithOutToken(existingDefault);
                }
            }

            await repo.AddWithoutTokenAsync(address);
            await _unitOfWork.SaveChangesAsync();

            var dto = _mapper.Map<AddressDto>(address);
            return SuccessDataResult(dto, "ADDRESS_CREATED", "Adres eklendi.");
        }

        public async Task<ServiceResult> UpdateAsync(Guid userId, UpdateAddressRequest request)
        {
            var repo = _unitOfWork.GetRepository<UserAddress>();
            var address = await repo.GetFirstWhereAsync(a => a.Id == request.Id && a.UserId == userId);

            if (address == null)
                return ErrorDataResult<AddressDto>(null, "ADDRESS_NOT_FOUND", "Adres bulunamadı.");

            address.Type = request.Type;
            address.Title = request.Title;
            address.FullName = request.FullName;
            address.Phone = request.Phone;
            address.City = request.City;
            address.District = request.District;
            address.AddressLine = request.AddressLine;
            address.PostalCode = request.PostalCode;
            address.CompanyName = request.CompanyName;
            address.TaxNumber = request.TaxNumber;
            address.TaxOffice = request.TaxOffice;

            // Eğer varsayılan olarak işaretlendiyse
            if (request.IsDefault && !address.IsDefault)
            {
                var existingDefaults = await repo.GetWhere(a => a.UserId == userId && a.Type == request.Type && a.IsDefault && a.Id != request.Id
                && a.Status).ToListAsync();
                foreach (var existingDefault in existingDefaults)
                {
                    existingDefault.IsDefault = false;
                    repo.UpdateWithOutToken(existingDefault);
                }
            }
            address.IsDefault = request.IsDefault;

            repo.UpdateWithOutToken(address);
            await _unitOfWork.SaveChangesAsync();

            var dto = _mapper.Map<AddressDto>(address);
            return SuccessDataResult(dto, "ADDRESS_UPDATED", "Adres güncellendi.");
        }

        public async Task<ServiceResult> DeleteAsync(Guid id, Guid userId)
        {
            var repo = _unitOfWork.GetRepository<UserAddress>();
            var address = await repo.GetFirstWhereAsync(a => a.Id == id && a.UserId == userId);

            if (address == null)
                return ErrorResult("ADDRESS_NOT_FOUND", "Adres bulunamadı.");

            repo.Delete(address);
            await _unitOfWork.SaveChangesAsync();

            return SuccessResult("ADDRESS_DELETED", "Adres silindi.");
        }

        public async Task<ServiceResult> SetDefaultAsync(Guid id, Guid userId)
        {
            var repo = _unitOfWork.GetRepository<UserAddress>();
            var address = await repo.GetFirstWhereAsync(a => a.Id == id && a.UserId == userId);

            if (address == null)
                return ErrorResult("ADDRESS_NOT_FOUND", "Adres bulunamadı.");

            // Aynı tipteki diğer varsayılanları kaldır
            var existingDefaults = await repo.GetWhere(a => a.UserId == userId &&
            a.Type == address.Type && a.IsDefault && a.Status).ToListAsync();
            foreach (var existingDefault in existingDefaults)
            {
                existingDefault.IsDefault = false;
                repo.UpdateWithOutToken(existingDefault);
            }

            address.IsDefault = true;
            repo.UpdateWithOutToken(address);
            await _unitOfWork.SaveChangesAsync();

            return SuccessResult("DEFAULT_SET", "Varsayılan adres güncellendi.");
        }
    }
}
