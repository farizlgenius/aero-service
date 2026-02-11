using System;
using Aero.Infrastructure.Data.Entities;

namespace Aero.Infrastructure.Mapper;

public sealed class HolderMapper
{
      public static CardHolder ToEf(Aero.Domain.Entities.CardHolder data)
      {
            var res = new CardHolder();
            // Base 
            NoMacBaseMapper.ToEf(data,res);
            res.user_id = data.UserId;
            res.title = data.Title;
            res.first_name = data.FirstName;
            res.middle_name = data.MiddleName;
            res.last_name = data.LastName;
            res.sex = data.Sex;
            res.email = data.Email;
            res.phone = data.Phone;
            res.company = data.Company;
            res.department  =data.Department;
            res.position = data.Position;
            res.flag = data.Flag;
            res.additionals = data.Additionals.Select(x => new CardHolderAdditional
            {
                  holder_id = data.UserId,
                  additional = x
            }).ToArray();
            res.image_path = data.ImagePath;
            res.credentials = data.Credentials is null || data.Credentials.Count <= 0 ? new List<Credential>() :  data.Credentials.Select(x => new Credential
            {
                  component_id = x.ComponentId,
                  bits = x.Bits,
                  issue_code = x.IssueCode,
                  fac_code = x.FacilityCode,
                  card_no = x.CardNo,
                  pin = x.Pin,
                  active_date = x.ActiveDate,
                  deactive_date = x.DeactiveDate,
                  cardholder_id = data.UserId
            }).ToList();
            res.cardholder_access_levels = data.AccessLevels is null || data.AccessLevels.Count <= 0 ? new List<CardHolderAccessLevel>() :  data.AccessLevels.Select(x => new CardHolderAccessLevel
            {
                  holder_id = data.UserId,
                  accesslevel_id = x.ComponentId
            }).ToList();

            return res;
      }

      public static void Update(Aero.Infrastructure.Data.Entities.CardHolder res,Aero.Domain.Entities.CardHolder data)
      {
            // Base 
            NoMacBaseMapper.Update(data,res);
            res.user_id = data.UserId;
            res.title = data.Title;
            res.first_name = data.FirstName;
            res.middle_name = data.MiddleName;
            res.last_name = data.LastName;
            res.sex = data.Sex;
            res.email = data.Email;
            res.phone = data.Phone;
            res.company = data.Company;
            res.department  =data.Department;
            res.position = data.Position;
            res.flag = data.Flag;
            res.additionals = data.Additionals.Select(x => new CardHolderAdditional
            {
                  holder_id = data.UserId,
                  additional = x
            }).ToArray();
            res.image_path = data.ImagePath;
            res.credentials = data.Credentials is null || data.Credentials.Count <= 0 ? new List<Credential>() :  data.Credentials.Select(x => new Credential
            {
                  component_id = x.ComponentId,
                  bits = x.Bits,
                  issue_code = x.IssueCode,
                  fac_code = x.FacilityCode,
                  card_no = x.CardNo,
                  pin = x.Pin,
                  active_date = x.ActiveDate,
                  deactive_date = x.DeactiveDate,
                  cardholder_id = data.UserId
            }).ToList();
            res.cardholder_access_levels = data.AccessLevels is null || data.AccessLevels.Count <= 0 ? new List<CardHolderAccessLevel>() :  data.AccessLevels.Select(x => new CardHolderAccessLevel
            {
                  holder_id = data.UserId,
                  accesslevel_id = x.ComponentId
            }).ToList();

      }

}
