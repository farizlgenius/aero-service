using Aero.Application.Interface;
using Aero.Domain.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace Aero.Infrastructure.Persistences.Entities;

    public class AccessLevelComponent : IDeviceId
    {
        [Key]
        public int id {get; set;}
        public short driver_id { get; set; }
        public short device_id { get; set;} 
        public Device device { get; set; }
        public int door_id { get; set; }
        public short acr_id { get; set; }
        public Door door { get; set; }
        public short timezone_id { get; set; }
        public TimeZone timezone { get; set; }

        // Reference 
        public int access_level_id {get; set;}
        public AccessLevel access_level {get; set;}

        public AccessLevelComponent(){}


        public AccessLevelComponent(short driver_id,short device_id,int door_id,short acr_id,short timezone_id) 
        {
        this.driver_id = driver_id;
        this.device_id = device_id;
        this.door_id = door_id;
        this.acr_id = acr_id;
        this.timezone_id = timezone_id;
        }

        public void Update(Aero.Domain.Entities.AccessLevelComponent data)
        {
            driver_id = data.DriverId;
            device_id = data.DeviceId;
            door_id = data.DoorId;
            acr_id = data.AcrId;
            timezone_id = data.TimezoneId;
        }

    }

