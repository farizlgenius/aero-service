using System;
using Aero.Application.Interface;
using Aero.Domain.Entities;
using Aero.Domain.Helpers;
using Aero.Domain.Interface;
using Aero.Infrastructure.Persistences;
using Aero.Infrastructure.Persistences.Entities;
using Microsoft.EntityFrameworkCore;

namespace Aero.Infrastructure.Repositories;

public class AuthRepository(AppDbContext context) : IAuthRepository
{
      public async Task<int> AddRefreshTokenAsync(string hashed, string userId, string username, string info, TimeSpan ttl)
      {
            // write audit row
            var audit = new Aero.Infrastructure.Persistences.Entities.RefreshToken
            {
                hashed_token = hashed,
                user_id = userId,
                user_name = username,
                action = "create",
                info = info,
                created_date = DateTime.UtcNow,
                updated_date = DateTime.UtcNow,
                expire_date = DateTime.UtcNow.Add(ttl)
            };
            await context.refresh_token.AddAsync(audit);
            return await context.SaveChangesAsync();
      }

      public async Task<Domain.Entities.RefreshToken> GetRefreshTokenByHashed(string hashed)
      {
            var refresh = await context.refresh_token
                .AsNoTracking()
                .Where(x => x.hashed_token.Equals(hashed))
                .FirstOrDefaultAsync();

            if(refresh is null) return null;

            return new Domain.Entities.RefreshToken
            {
                  HashedToken = refresh.hashed_token,
                  UserId = refresh.user_id,
                  Username = refresh.user_name,
                  Action = refresh.action,
                  Info = refresh.info,
                  ExpireDate = refresh.expire_date
            };
      }

      public async Task<int> RevokeTokenAsync(string hashed)
      {
            var data = new Aero.Infrastructure.Persistences.Entities.RefreshToken
            {
                  hashed_token = hashed,
                    user_id = "unknown",
                    user_name = "unknown",
                    action = "revoke",
                    created_date= DateTime.UtcNow,
      
            };
            await context.refresh_token.AddAsync(data);
            return await context.SaveChangesAsync();
      }

      public async Task<int> RotateRefreshTokenAsync(string hashed, string userId, string username, string info, TimeSpan ttl)
      {
            var data =new Aero.Infrastructure.Persistences.Entities.RefreshToken
            {
                  hashed_token = hashed,
                  user_id = userId,
                  user_name = username,
                  action = "rotate",
                  info = info,
                  created_date = DateTime.UtcNow,
                  expire_date = DateTime.UtcNow.Add(ttl),
                  updated_date = DateTime.UtcNow

            };
            await context.refresh_token.AddAsync(data);
            return await context.SaveChangesAsync();
      }
}
