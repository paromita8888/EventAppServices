using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventAppServices.Models
{
    public class PushNotification
    {
        [Required]
        [Display(Name = "Subject")]
        [DataType(DataType.Text)]
        [StringLength(30)]
        public string Subject { get; set; }

        [Required]
        [Display(Name = "Body")]
        [DataType(DataType.Text)]
        [StringLength(220)]
        public string Body { get; set; }

        [Required]
        [Display(Name = "AppleConfig")]
        [DataType(DataType.Text)]
        public string AppleConfig { get; set; }
    }
}
