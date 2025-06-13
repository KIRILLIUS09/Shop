using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.DTOs.Responses.Address
{
    public record AddressResponse(
    int Id,
    string FullAddress,  
    string Region,
    string City,
    string Street,
    string Building,
    string? Apartment,
    int? UserId
);
}
