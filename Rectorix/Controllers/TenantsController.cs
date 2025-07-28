using Asp.Versioning;
// Api/Controllers/TenantsController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rectorix.Application.DTOs.Tenants;
using Rectorix.Application.Services.Tenants;

namespace Rectorix.API.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion(1.0)]
    public class TenantsController(ITenantService svc) : RectorixBaseController
    {
        [HttpGet]
        public async Task<IEnumerable<TenantResponse>> GetAll(CancellationToken ct) =>
            await svc.GetAllAsync(ct);

        [HttpGet("{id}")]
        public async Task<ActionResult<TenantResponse>> Get(string id, CancellationToken ct)
        {
            var result = await svc.GetAsync(id, ct);
            return result is null ? NotFound() : Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<TenantResponse>> Create(
            TenantCreateRequest dto, CancellationToken ct)
        {
            var created = await svc.CreateAsync(dto, ct);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            string id, TenantUpdateRequest dto, CancellationToken ct)
        {
            var ok = await svc.UpdateAsync(id, dto, ct);
            return ok ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id, CancellationToken ct)
        {
            var ok = await svc.DeleteAsync(id, ct);
            return ok ? NoContent() : NotFound();
        }
    }

}
