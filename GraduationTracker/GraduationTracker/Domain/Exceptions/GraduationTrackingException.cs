using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationTracker.Domain.Exceptions
{
    public class GraduationTrackingException : Exception 
    {
        public GraduationTrackingException(string message) : base(message)
        {
            // Custom properties can be set in Data here.
        }
    }
}
