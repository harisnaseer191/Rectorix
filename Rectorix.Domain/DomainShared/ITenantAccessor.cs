using Finbuckle.MultiTenant.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rectorix.Domain.DomainShared
{
    public interface ITenantAccessor
    {
        RectorixTenantInfo Current {  get; }
    }

    /// <remarks>
    /// `IMultiTenantContextAccessor<TenantInfo>` is scoped; it’s null during app startup,
    /// then populated by the UseMultiTenant middleware on each request.
    /// </remarks>
    public sealed class TenantAccessor(
            IMultiTenantContextAccessor<RectorixTenantInfo> ctxAccessor)
        : ITenantAccessor
    {
        public RectorixTenantInfo Current =>
            ctxAccessor.MultiTenantContext?.TenantInfo
            ?? throw new InvalidOperationException(
                 "Tenant has not been resolved. Did you forget app.UseMultiTenant()?");
    }


    public sealed class RectorixTenantInfo : Finbuckle.MultiTenant.TenantInfo
    {

    }
}
