using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.DTOs.Responses.Address
{
    public record AddressShortResponse(
     int Id,
     string ShortAddress,
     int? UserId
 );
}
