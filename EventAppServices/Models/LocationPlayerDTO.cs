using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventAppServices.Models
{
    public class LocationPlayerDTO
    { 
        public string Location { get; set; }
        public List<PlayerDTO> PlayerDto { get; set; }
    }
}