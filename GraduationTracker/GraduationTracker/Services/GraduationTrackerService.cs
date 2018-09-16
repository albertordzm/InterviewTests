using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraduationTracker.Repositories.Interfaces;
using GraduationTracker.Models;
using GraduationTracker.Domain.Enums;
using GraduationTracker.Domain.Exceptions;

namespace GraduationTracker.Services
{
    public class GraduationTrackerService
    {
        private IGraduationTrackerRepository _graduationTrackerRepository;
        private GraduationStanding[] _graduationStandings;

        public GraduationTrackerService(GraduationStanding[] graduationStandings, IGraduationTrackerRepository graduationTrackerRepository)
        {
            _graduationTrackerRepository = graduationTrackerRepository;
            _graduationStandings = graduationStandings; // Could also be passed to method, depending on whether this changes by client/institution.
        }

        public GraduationStanding HasGraduated(Diploma diploma, Student student)
        {
            if (diploma == null)
                throw new GraduationTrackingException("Unable to determine graduation requirements because the assigned diploma is null");

            if (diploma.Requirements == null || diploma.Requirements.Count() == 0)
                throw new GraduationTrackingException("Unable to determine graduation requirements because the diploma has no graduation requirements");

            if (student == null)
                throw new GraduationTrackingException("Unable to determine graduation requirements because the student is null");

            if (student.Courses == null)
                throw new GraduationTrackingException("Unable to determine graduation requirements because the student courses list is null");

            var markSum = student.Courses.Where(sc => 
                           diploma.Requirements.SelectMany(r => r.Courses).Contains(sc.Id)).
                           Select(sc => sc.Mark).
                           Sum();

            var credits = diploma.Requirements.Where(r => 
                           student.Courses.Where(sc => sc.Mark >= r.MinimumMark).
                           Select(sc => sc.Id).Intersect(r.Courses).Count() > 0).
                           Count();

            // If student hasn't got the total # of credits to graduate then they don't have a standing.
            if (credits < diploma.Credits)
                return new GraduationStanding { Id = 1, Standing = STANDING.None, Graduated = false };

            int average = markSum / student.Courses.Length;

            if (_graduationStandings.Where(gs => gs.MinimumRequiredAverage <= average).Count() == 0)
                throw new GraduationTrackingException($"Unable to find a graduation standing for the average {average}. Please check graduation standing configuration.");  

            return _graduationStandings.Where(gs => gs.MinimumRequiredAverage <= average).OrderByDescending(gs => gs.MinimumRequiredAverage).FirstOrDefault();
        }             
    }
}
