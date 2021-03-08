using System;
using System.Linq;
using System.Reflection;

namespace NAdmin.Persistence
{
    public static partial class CustomExtensions
    {
        static readonly MethodInfo SetMethod = typeof(NAdminDbContext)
            .GetMethods().First(x => x.Name.Equals(nameof(NAdminDbContext.Set), StringComparison.InvariantCultureIgnoreCase));

        public static IQueryable<object> Query(this NAdminDbContext context, Type entityType) =>
            (IQueryable<object>)SetMethod.MakeGenericMethod(entityType).Invoke(context, null);
    }
}