using Aero.Application.Interface;
using Aero.Domain.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace Aero.Infrastructure.Persistences.Entities;

    public class AccessLevelComponent : IMac,IDriverId
    {
        [Key]
        public int id {get; set;}
        public short driver_id { get; set; }
        public string mac { get; set;} = string.Empty;
        public Hardware hardware { get; set; }
        public int door_id { get; set; }
        public short acr_id { get; set; }
        public Door door { get; set; }
        public short timezone_id { get; set; }
        public TimeZone timezone { get; set; }

        // Reference 
        public short access_level_id {get; set;}
        public AccessLevel access_level {get; set;}

        public AccessLevelComponent(short driver,string mac,int doorid,short acrid,short timezone) 
        {
        this.driver_id = driver;
        this.mac = mac;
        this.door_id = doorid;
        this.acr_id = acrid;
        this.timezone_id = timezone;
        }

        public void Update(Aero.Domain.Entities.AccessLevelComponent data)
        {
            driver_id = data.DriverId;
            mac = data.Mac;
            door_id = data.DoorId;
            acr_id = data.AcrId;
            timezone_id = data.TimezoneId;
        }
    }
