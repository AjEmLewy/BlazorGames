using Gejms.Server.Data;
using Microsoft.EntityFrameworkCore;

namespace Gejms.Server.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder MigrateDatabase(this IApplicationBuilder app)
    {
        using var servicesScope = app.ApplicationServices.CreateScope();
        using var gejmsDb = servicesScope.ServiceProvider.GetRequiredService<GejmsDbContext>();
        gejmsDb.Database.Migrate();
        return app;
    }
}
