using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationTracker.Models
{
    public class Diploma
    {
        public int Id { get; set; }
        public int Credits { get; set; }
        public Requirement[] Requirements { get; set; }
    }
}
