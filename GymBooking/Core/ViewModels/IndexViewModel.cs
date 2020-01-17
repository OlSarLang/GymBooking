using GymBooking.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GymBooking.Core.ViewModels
{
    public class IndexViewModel
    {
        public IEnumerable<GymClass> GymClasses { get; set; }
        public bool History { get; set; }
    }
}
