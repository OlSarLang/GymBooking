using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GymBooking.Core.Models
{
    public class ApplicationUserGymClass
    {
      
        public string ApplicationUserId { get; set; }
        public int GymClassId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public GymClass GymClass { get; set; }
    }
}
