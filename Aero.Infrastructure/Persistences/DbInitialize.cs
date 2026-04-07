using System;
using Aero.Infrastructure.Persistences.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Aero.Infrastructure.Persistences;


public static class DbInitializer
{
    // public static async Task SeedPermissionsAsync(IServiceProvider services)
    // {
    //     using var scope = services.CreateScope();
    //     var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    //     await db.Database.MigrateAsync();

    //     IEnumerable<Source> sources = await db.sources.ToListAsync();

    //     // var permissions = sources.SelectMany(s => scopes.Select(sc => new Permission
    //     // {
    //     //     source_id = s.id,
    //     //     permission_scope_id = sc.id,
    //     //     code = $"{s.name}:{sc.name}"
    //     // }));

    //     foreach (var per in sources)
    //     {
    //         if (!db.permissions.Any())
    //             db.permissions.Add(new Permission
    //             {
    //                 source_id = per.id,
    //                 code = per.name,
    //                 is_allow = false,
    //                 is_create = false,
    //                 is_modify = false,
    //                 is_delete = false,
    //                 is_action = false
    //             });
    //     }


    //     await db.SaveChangesAsync();
    // }

    public static async Task SeedRolesAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        await db.Database.MigrateAsync();

        var role = await db.role.Where(x => x.name == "Administrator").FirstOrDefaultAsync();

        if(role == null)
        {
            role = new Role
            {
                name = "Administrator"
            };
            db.role.Add(role);
            await db.SaveChangesAsync();
        }

        if (!db.permissions.Any())
        {

            var allSource = await db.sources.ToListAsync();
            foreach (var perm in allSource)
            {
                db.permissions.Add(new Permission(
                    perm.id,
                    role.id,
                    true,
                    true,
                    true,
                    true,
                    true
                ));
                
            }

            await db.SaveChangesAsync();
        }

       
    }
}