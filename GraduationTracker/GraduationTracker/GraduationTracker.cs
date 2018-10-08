using System;
using System.Linq;

namespace GraduationTracker
{
    public partial class GraduationTracker
    {   
        public Tuple<bool, STANDING> HasGraduated(Diploma diploma, Student student)
        {
            var reportCard = GetReportCard(diploma, student);            

            return new Tuple<bool, STANDING>(reportCard.Credits >= diploma.Credits, reportCard.Standing);
        }

        public ReportCard GetReportCard(Diploma diploma, Student student)
        {
            var result = new ReportCard()
            {
                DiplomaId = diploma.Id,
                StudentId = student.Id
            };
            var gradeSum = 0;
            var totalRequiredCoursesTakenForDiploma = 0;

            foreach (var requirementId in diploma.Requirements)
            {
                // Need to check if the student meets this requirement.
                var requirement = Repository.GetRequirement(requirementId);

                // Get all the courses the student has taken that are part of the requirement.
                var requirementCoursesTakenByStudent =
                    student.Courses.Where(c => requirement.CourseIds.Contains(c.Id));

                // Increment total number of courses taken for this diploma
                totalRequiredCoursesTakenForDiploma += requirementCoursesTakenByStudent.Count();

                // Aggregate the marks obtained.
                gradeSum += requirementCoursesTakenByStudent.Sum(c => c.Mark);

                // Aggregate credits if mark meets minimum requirement
                result.Credits += requirementCoursesTakenByStudent
                    .Where(c => c.Mark > requirement.MinimumMark)
                    .Sum(c => requirement.Credits);
            }

            result.Average = gradeSum / totalRequiredCoursesTakenForDiploma;

            return result;
        }

        public static STANDING GetStanding(int average)
        {
            var result = STANDING.None;

            if (average < 50)
                result = STANDING.Remedial;
            else if (average < 80)
                result = STANDING.Average;
            else if (average < 95)
                result = STANDING.MagnaCumLaude;
            else
                result = STANDING.SumaCumLaude;

            return result;
        }
    }
}
