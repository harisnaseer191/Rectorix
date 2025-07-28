using Rectorix.Application.DTOs.Tenants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rectorix.Application.Services.Tenants
{
    public interface ITenantService
    {
        Task<IEnumerable<TenantResponse>> GetAllAsync(CancellationToken ct);
        Task<TenantResponse?> GetAsync(string id, CancellationToken ct);
        Task<TenantResponse> CreateAsync(TenantCreateRequest dto, CancellationToken ct);
        Task<bool> UpdateAsync(string id, TenantUpdateRequest dto, CancellationToken ct);
        Task<bool> DeleteAsync(string id, CancellationToken ct);
    }
}
