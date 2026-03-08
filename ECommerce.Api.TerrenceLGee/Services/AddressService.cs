using ECommerce.Contracts.TerrenceLGee.Common.Pagination;
using ECommerce.Contracts.TerrenceLGee.Common.Results;
using ECommerce.Contracts.TerrenceLGee.Interfaces.RepositoryInterfaces;
using ECommerce.Contracts.TerrenceLGee.Interfaces.ServiceInterfaces;
using ECommerce.Contracts.TerrenceLGee.Mappings.AddressMappings;
using ECommerce.Shared.TerrenceLGee.DTOs.AddressDTOs;
using ECommerce.Shared.TerrenceLGee.Parameters.AddressParameters;

namespace ECommerce.Api.TerrenceLGee.Services;

public class AddressService : IAddressService
{
    private readonly IAddressRepository _addressRepository;

    public AddressService(IAddressRepository addressRepository)
    {
        _addressRepository = addressRepository;
    }

    public async Task<Result<RetrievedAddressDto?>> AddAddressAsync(CreateAddressDto address)
    {
        var addedAddress = await _addressRepository.AddAddressAsync(address.FromCreateAddressDto());

        if (addedAddress is null)
        {
            return Result<RetrievedAddressDto?>.Fail("Unable to add address at this time.");
        }

        return Result<RetrievedAddressDto?>.Ok(addedAddress.ToRetrievedAddressDto());
    }

    public async Task<Result<RetrievedAddressDto?>> UpdateAddressAsync(UpdateAddressDto address)
    {
        var updatedAddress = await _addressRepository.UpdateAddressAsync(address.FromUpdateAddressDto());

        if (updatedAddress is null)
        {
            return Result<RetrievedAddressDto?>.Fail($"Unable to update address {address.Id}");
        }

        return Result<RetrievedAddressDto?>.Ok(updatedAddress.ToRetrievedAddressDto());
    }

    public async Task<Result> DeleteAddressAsync(AddressIdDto addressIdDto)
    {
        var deletion = await _addressRepository.DeleteAddressAsync(addressIdDto.Id, addressIdDto.CustomerId);

        if (!deletion)
        {
            return Result.Fail($"Unable to delete address {addressIdDto.Id}");
        }

        return Result.Ok();
    }

    public async Task<Result<RetrievedAddressDto?>> GetAddressAsync(AddressIdDto addressIdDto)
    {
        var address = await _addressRepository.GetAddressAsync(addressIdDto.Id, addressIdDto.CustomerId);

        if (address is null)
        {
            return Result<RetrievedAddressDto?>.Fail($"Unable to retrieve address {addressIdDto.Id}");
        }

        return Result<RetrievedAddressDto?>.Ok(address.ToRetrievedAddressDto());
    }

    public async Task<Result<PagedList<RetrievedAddressDto>>> GetCustomerAddressesAsync(AddressQueryParams addressQueryParams)
    {
        var addresses = await _addressRepository.GetCustomerAddressesAsync(addressQueryParams);

        return Result<PagedList<RetrievedAddressDto>>.Ok(new PagedList<RetrievedAddressDto>(
            addresses.Select(a => a.ToRetrievedAddressDto()),
            addresses.TotalEntities,
            addressQueryParams.Page,
            addressQueryParams.PageSize));
    }

    public async Task<Result<PagedList<RetrievedAddressDto>>> GetAllCustomerAddressesForAdminAsync(AddressQueryParams addressQueryParams)
    {
        var addresses = await _addressRepository.GetAllCustomerAddressesForAdminAsync(addressQueryParams);

        return Result<PagedList<RetrievedAddressDto>>.Ok(new PagedList<RetrievedAddressDto>(
            addresses.Select(a => a.ToRetrievedAddressDto()),
            addresses.TotalEntities,
            addressQueryParams.Page,
            addressQueryParams.PageSize));
    }

    public async Task<Result<int>> GetCustomerAddressCountAsync(AddressIdDto addressIdDto)
    {
        var count = await _addressRepository.GetCustomerAddressCountAsync(addressIdDto.CustomerId);

        if (count == -1)
        {
            return Result<int>.Fail("Unable to retrieved address count");
        }

        return Result<int>.Ok(count);
    }

    public async Task<Result<int>> GetAllAddressCountForAdminAsync()
    {
        var count = await _addressRepository.GetAllAddressCountForAdminAsync();

        if (count == -1)
        {
            return Result<int>.Fail("Unable to retrieve address count");
        }

        return Result<int>.Ok(count);
    }
}
