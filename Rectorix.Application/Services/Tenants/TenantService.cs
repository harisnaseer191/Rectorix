using Finbuckle.MultiTenant.Abstractions;
using Rectorix.Application.DTOs.Tenants;
using Rectorix.Domain.DomainShared;

namespace Rectorix.Application.Services.Tenants
{
    public class TenantService(IMultiTenantStore<RectorixTenantInfo> store) : ITenantService
    {
        public async Task<IEnumerable<TenantResponse>> GetAllAsync(CancellationToken ct) =>
            (await store.GetAllAsync())
            .Select(t => new TenantResponse(t.Id!, t.Identifier!, t.Name!));

        public async Task<TenantResponse?> GetAsync(string id, CancellationToken ct)
        {
            var t = await store.TryGetAsync(id);
            return t == null ? null : new TenantResponse(t.Id!, t.Identifier!, t.Name!);
        }

        public async Task<TenantResponse> CreateAsync(TenantCreateRequest dto, CancellationToken ct)
        {
            // generate a numeric string PK; you can switch to Guid or Snowflake
            var newId = Guid.NewGuid().ToString();                   // or $"{Snowflake.NextId()}"

            var tenant = new RectorixTenantInfo
            {
                Id = newId,
                Identifier = dto.Identifier.ToLowerInvariant(),
                Name = dto.Name,
            };

            if (!await store.TryAddAsync(tenant))
                throw new InvalidOperationException("Tenant identifier already exists.");

            return new TenantResponse(tenant.Id!, tenant.Identifier!, tenant.Name!);
        }

        public async Task<bool> UpdateAsync(string id, TenantUpdateRequest dto, CancellationToken ct)
        {
            var t = await store.TryGetAsync(id);
            if (t == null) return false;

            t.Name = dto.Name;

            return await store.TryUpdateAsync(t);
        }

        public Task<bool> DeleteAsync(string id, CancellationToken ct) =>
            store.TryRemoveAsync(id);
    }
}
