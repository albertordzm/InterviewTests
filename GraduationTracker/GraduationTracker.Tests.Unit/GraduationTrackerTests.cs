/*
 * Note: Could also have made credit and/or grades into double depending on the business reality, but
 *       the refactor intentions are already clear for the purposes of this exercise.
 */
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using Moq;
using GraduationTracker.Repositories.Interfaces;
using GraduationTracker.Models;
using GraduationTracker.Services;
using GraduationTracker.Domain.Enums;
using GraduationTracker.Domain.Exceptions;

namespace GraduationTracker.Tests.Unit
{    
    [TestClass]
    public class GraduationTrackerTests
    {
        private GraduationTrackerService _graduationTracker;        
        private Mock<IGraduationTrackerRepository> _graduationTrackerRepository;
        
        [TestInitialize]
        public void InitTestSuite() {
            _graduationTrackerRepository = new Mock<IGraduationTrackerRepository>();
            _graduationTracker = new GraduationTrackerService(GetMockGraduationStandings(), _graduationTrackerRepository.Object);
        }

        [TestCategory("Graduation Tracker Unit Tests")]
        [TestMethod]
        public void GraduationTracker_RemedialStudent_ShouldNotHaveGraduated()
        {
            // Arrange
            var student = GetMockStudents().Where(s => s.Id == 4).SingleOrDefault();
            var diploma = GetMockLowMinimumMarkDiploma();    // Need to use a different diploma so that we can trigger a remedial case

            // Act/Assert
            TestStudentGraduated(student, diploma, STANDING.Remedial, false);
        }

        [TestCategory("Graduation Tracker Unit Tests")]
        [TestMethod]
        public void GraduationTracker_AverageStudent_ShouldHaveGraduated()
        {
            // Arrange
            var student = GetMockStudents().Where(s => s.Id == 3).SingleOrDefault();
            var diploma = GetMockDiploma();

            // Act/Assert
            TestStudentGraduated(student, diploma, STANDING.Average, true);
        }

        [TestCategory("Graduation Tracker Unit Tests")]
        [TestMethod]
        public void GraduationTracker_SumaCumLaude_ShouldHaveGraduated()
        {
            // Arrange
            var student = GetMockStudents().Where(s => s.Id == 2).SingleOrDefault();
            var diploma = GetMockDiploma();

            // Act/Assert
            TestStudentGraduated(student, diploma, STANDING.SumaCumLaude, true);
        }

        [TestCategory("Graduation Tracker Unit Tests")]
        [TestMethod]
        public void GraduationTracker_MagnaCumLaude_ShouldHaveGraduated()
        {
            // Arrange
            var student = GetMockStudents().Where(s => s.Id == 1).SingleOrDefault();
            var diploma = GetMockDiploma();

            // Act/Assert
            TestStudentGraduated(student, diploma, STANDING.MagnaCumLaude, true);
        }

        [TestCategory("Graduation Tracker Unit Tests")]
        [TestMethod]
        public void GraduationTracker_NotAllRequirementsMet_ShouldHaveNoStanding()
        {
            // Arrange
            var student = GetMockStudents().Where(s => s.Id == 5).SingleOrDefault();
            var diploma = GetMockDiploma();

            // Act/Assert
            TestStudentGraduated(student, diploma, STANDING.None, false);
        }

        /*
        [TestMethod]
        public void TestHasCredits()
        {
            /* Leaving this unit test commented out, as I don't know what logic it was
             * originally intended to test. It's basically just adding 4 items to a list and
             * asserting that there's nothing in the list (which will always fail). I believe
             * my other unit tests cover any logic this test was originally intended to cover.
             * /

            // Arrange
            var graduated = new List<Tuple<bool, STANDING>>();
            var students = GetMockStudents();
            var diploma = GetMockDiploma();

            _graduationTrackerRepository.Setup(b => b.GetDiploma(1)).Returns(diploma);            

            // Act
            foreach(var student in students)
            {
                graduated.Add(_graduationTracker.HasGraduated(diploma, student));      
            }

            // Assert
            Assert.IsFalse(graduated.Any());
        }
        */

        #region Exception Handling Unit Tests
        [TestCategory("Graduation Tracker: Exception Handling")]
        [ExpectedException(typeof(GraduationTrackingException))]
        [TestMethod]
        public void GraduationTracker_NullStudent_ShouldThrowException()
        {
            var diploma = GetMockDiploma();

            // Act
            TestStudentGraduated(null, diploma, STANDING.None, false);

            // Assert = Exception Thrown
        }

        [TestCategory("Graduation Tracker: Exception Handling")]
        [ExpectedException(typeof(GraduationTrackingException))]
        [TestMethod]
        public void GraduationTracker_NullDiploma_ShouldThrowException()
        {
            var student = GetMockStudents().Where(s => s.Id == 1).SingleOrDefault();            

            // Act/Assert
            TestStudentGraduated(student, null, STANDING.None, false);
        }

        [TestCategory("Graduation Tracker: Exception Handling")]
        [ExpectedException(typeof(GraduationTrackingException))]
        [TestMethod]
        public void GraduationTracker_NullDiplomaRequirements_ShouldThrowException()
        {
            // Arrange
            var student = GetMockStudents().Where(s => s.Id == 1).SingleOrDefault();
            var diploma = GetMockDiploma();
            diploma.Requirements = null;

            // Act
            TestStudentGraduated(student, diploma, STANDING.MagnaCumLaude, true);

            // Assert = Exception Thrown
        }

        [TestCategory("Graduation Tracker: Exception Handling")]
        [ExpectedException(typeof(GraduationTrackingException))]
        [TestMethod]
        public void GraduationTracker_EmptyRequirements_ShouldThrowException()
        {
            // Arrange
            var student = GetMockStudents().Where(s => s.Id == 1).SingleOrDefault();
            var diploma = GetMockDiploma();
            diploma.Requirements = new Requirement[] { };

            // Act
            TestStudentGraduated(student, diploma, STANDING.MagnaCumLaude, true);

            // Assert = Exception Thrown
        }

        [TestCategory("Graduation Tracker: Exception Handling")]
        [ExpectedException(typeof(GraduationTrackingException))]
        [TestMethod]
        public void GraduationTracker_NullStudentCourses_ShouldThrowException()
        {
            // Arrange
            var student = GetMockStudents().Where(s => s.Id == 1).SingleOrDefault();
            var diploma = GetMockDiploma();
            student.Courses = null;

            // Act
            TestStudentGraduated(student, diploma, STANDING.MagnaCumLaude, true);

            // Assert = Exception Thrown
        }
        #endregion 

        #region Unit Test Helpers
        private void TestStudentGraduated(Student student, Diploma diploma, STANDING expectedStanding, bool shouldBeGraduating)
        {
            // Arrange
            _graduationTrackerRepository.Setup(b => b.GetDiploma(1)).Returns(diploma);

            // Act
            var graduationResult = _graduationTracker.HasGraduated(diploma, student);

            // Assert
            Assert.AreEqual(shouldBeGraduating, graduationResult.Graduated);
            Assert.AreEqual(expectedStanding, graduationResult.Standing);
        }
        #endregion 

        #region Mock Data
        // Note: Could also move this into a static helper class if we needed this data for multiple unit test suites.
        private Diploma GetMockDiploma()
        {
            return new Diploma
            {
                Id = 1,
                Credits = 4,
                Requirements = GetMockRequirements()
            };
        }

        private Diploma GetMockLowMinimumMarkDiploma()
        {
            return new Diploma
            {
                Id = 1,
                Credits = 4,
                Requirements = GetMockLowMinimumMarkRequirements()
            };
        }

        private Student[] GetMockStudents()
        {
            return new[]
            {
               new Student
               {
                   Id = 1,
                   Courses = new Course[]
                   {
                        new Course{Id = 1, Name = "Math", Mark=95 },
                        new Course{Id = 2, Name = "Science", Mark=95 },
                        new Course{Id = 3, Name = "Literature", Mark=95 },
                        new Course{Id = 4, Name = "Physichal Education", Mark=95 }
                   }
               },
               new Student
               {
                   Id = 2,
                   Courses = new Course[]
                   {
                        new Course{Id = 1, Name = "Math", Mark=80 },
                        new Course{Id = 2, Name = "Science", Mark=80 },
                        new Course{Id = 3, Name = "Literature", Mark=80 },
                        new Course{Id = 4, Name = "Physichal Education", Mark=80 }
                   }
               },
               new Student
               {
                   Id = 3,
                   Courses = new Course[]
                   {
                        new Course{Id = 1, Name = "Math", Mark=50 },
                        new Course{Id = 2, Name = "Science", Mark=50 },
                        new Course{Id = 3, Name = "Literature", Mark=50 },
                        new Course{Id = 4, Name = "Physichal Education", Mark=50 }
                   }
                },
                new Student
                {
                    Id = 4,
                    Courses = new Course[]
                    {
                        new Course{Id = 1, Name = "Math", Mark=40 },
                        new Course{Id = 2, Name = "Science", Mark=40 },
                        new Course{Id = 3, Name = "Literature", Mark=40 },
                        new Course{Id = 4, Name = "Physichal Education", Mark=40 }
                    }
                },
                new Student
                {
                    Id = 5,
                    Courses = new Course[]
                    {
                        new Course{Id = 1, Name = "Math", Mark=100 },
                        new Course{Id = 2, Name = "Science", Mark=100 },
                        new Course{Id = 3, Name = "Literature", Mark=100 }
                    }
                }
            };
        }

        private GraduationStanding[] GetMockGraduationStandings()
        {
            return new[]
            {
                new GraduationStanding{Id = 1, Standing = STANDING.MagnaCumLaude, Graduated = true, MinimumRequiredAverage = 95},
                new GraduationStanding{Id = 2, Standing = STANDING.SumaCumLaude, Graduated = true, MinimumRequiredAverage = 80},
                new GraduationStanding{Id = 3, Standing = STANDING.Average, Graduated = true, MinimumRequiredAverage = 50},
                new GraduationStanding{Id = 4, Standing = STANDING.Remedial, Graduated = false, MinimumRequiredAverage = 0}
            };
        }

        private Requirement[] GetMockRequirements()
        {
            return new[]
            {
                new Requirement{Id = 100, Name = "Math", MinimumMark=50, Courses = new int[] {1}, Credits=1 },
                new Requirement{Id = 102, Name = "Science", MinimumMark=50, Courses = new int[]{2}, Credits=1 },
                new Requirement{Id = 103, Name = "Literature", MinimumMark=50, Courses = new int[]{3}, Credits=1},
                new Requirement{Id = 104, Name = "Physichal Education", MinimumMark=50, Courses = new int[]{4}, Credits=1 }
            };
        }

        private Requirement[] GetMockLowMinimumMarkRequirements()
        {
            return new[]
            {
                new Requirement{Id = 100, Name = "Math", MinimumMark=20, Courses = new int[] {1}, Credits=1 },
                new Requirement{Id = 102, Name = "Science", MinimumMark=20, Courses = new int[]{2}, Credits=1 },
                new Requirement{Id = 103, Name = "Literature", MinimumMark=20, Courses = new int[]{3}, Credits=1},
                new Requirement{Id = 104, Name = "Physichal Education", MinimumMark=20, Courses = new int[]{4}, Credits=1 }
            };
        }
        #endregion 
    }
}
