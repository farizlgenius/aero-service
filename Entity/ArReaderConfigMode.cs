﻿using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public class ArReaderConfigMode : ArBaseEntity
    {
        public string Name { get; set; }
        public short Value { get; set; }
        public string Description { get; set; }

    }
}
