using System;
using Aero.Infrastructure.Mapper;
using Aero.Domain.Entities;
using Aero.Domain.Interface;
using Aero.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Aero.Application.Helpers;

namespace Aero.Infrastructure.Repositories;

public class HolderRepository(AppDbContext context) : IHolderRepository
{
      public async Task<int> AddAsync(CardHolder data)
      {
            var en = HolderMapper.ToEf(data);
            await context.cardholder.AddAsync(en);
            return await context.SaveChangesAsync();
      }

      public async Task<int> DeleteByComponentIdAsync(short component)
      {
            var en = await context.cardholder
            .Where(x => x.component_id == component)
            .OrderBy(x => x.component_id)
            .FirstOrDefaultAsync();

            if(en is null) return 0;

            context.cardholder.Remove(en);
            return await context.SaveChangesAsync();
      }

      public async Task<int> DeleteByUserIdAsync(string UserId)
      {
            var en = await context.cardholder
            .Where(x => x.user_id.Equals(UserId))
            .OrderBy(x => x.component_id)
            .FirstOrDefaultAsync();

            if(en is null) return 0;

            context.cardholder.Remove(en);
            return await context.SaveChangesAsync();
      }

      public async Task<int> DeleteReferenceByUserIdAsync(string UserId)
      {
           // Additionals
           var additional = await context.cardholder_additional
           .Where(x => x.holder_id.Equals(UserId))
           .ToArrayAsync();

            context.cardholder_additional.RemoveRange(additional);

            // Access Level
            var access = await context.cardholder_access_level
            .Where(x => x.holder_id.Equals(UserId))
            .ToArrayAsync();

            context.cardholder_access_level.RemoveRange(access);

            // Credentials
            var credential = await context.credential
            .Where(x => x.cardholder_id.Equals(UserId))
            .ToArrayAsync();

            context.credential.RemoveRange(credential);

            return await context.SaveChangesAsync();
      }

      public async Task<int> UpdateAsync(CardHolder newData)
      {
            var en = await context.cardholder
            .Where(x => x.user_id.Equals(newData.UserId))
            .OrderBy(x => x.component_id)
            .FirstOrDefaultAsync();

            if(en is null) return 0;

            // Delete Credentials 

            // Delete link from access level 

            HolderMapper.Update(en,newData);

            context.cardholder.Update(en);
            return await context.SaveChangesAsync();
      }

    public async Task<int> UpdateImagePathAsync(string path,string userid)
    {
        var en = await context.cardholder
            .Where(x => x.user_id.Equals(userid))
            .OrderBy(x => x.component_id)
            .FirstOrDefaultAsync();

        if(en is null) return 0;

        en.image_path = path;
        context.cardholder.Update(en);
        return await context.SaveChangesAsync();
    }
}
