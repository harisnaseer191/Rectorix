using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Application/Tenants/TenantDtos.cs
using System.ComponentModel.DataAnnotations;

namespace Rectorix.Application.DTOs.Tenants
{


    public record TenantCreateRequest
    (
        [Required,  RegularExpression("^[a-z0-9-]{3,50}$")]
    string Identifier,                       // becomes sub-domain slug

        [Required,  StringLength(100)]
    string Name,

        [StringLength(300)]
    string? ConnectionString                 // null for shared-DB model
    );

    public record TenantUpdateRequest
    (
        [Required,  StringLength(100)]
    string Name,

        [StringLength(300)]
    string? ConnectionString
    );

    public record TenantResponse(string Id, string Identifier, string Name);

}
