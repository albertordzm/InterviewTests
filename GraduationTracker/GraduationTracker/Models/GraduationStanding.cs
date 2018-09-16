using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraduationTracker.Domain.Enums;

namespace GraduationTracker.Models
{
    public class GraduationStanding
    {
        public int Id { get; set; }
        public STANDING Standing { get; set; }
        public bool Graduated { get; set; }
        public int MinimumRequiredAverage { get; set; }
    }
}
