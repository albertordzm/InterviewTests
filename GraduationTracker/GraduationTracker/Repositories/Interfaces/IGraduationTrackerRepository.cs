using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraduationTracker.Models;

namespace GraduationTracker.Repositories.Interfaces
{
    public interface IGraduationTrackerRepository
    {
        Student GetStudent(int id);
        Diploma GetDiploma(int id);
    }
}
