using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GraduationTracker.Tests.Unit
{
    [TestClass]
    public class GraduationTrackerTests
    {
        [TestMethod]
        public void HasGraduated_Passed()
        {
            // Arrange
            var tracker = new GraduationTracker();
            var expectedResult = new Tuple<bool, STANDING>(true, STANDING.SumaCumLaude);
            var diploma = new Diploma
            {
                Id = 1,
                Credits = 4,
                Requirements = new int[] { 100, 102, 103, 104 }
            };
            var student = new Student
            {
                Id = 1,
                Courses = new Course[]
                   {
                        new Course{Id = 1, Name = "Math", Mark = 95 },
                        new Course{Id = 2, Name = "Science", Mark = 95 },
                        new Course{Id = 3, Name = "Literature", Mark = 95 },
                        new Course{Id = 4, Name = "Physichal Education", Mark = 95 }
                   }
            };

            // Act
            var actualResult = tracker.HasGraduated(diploma, student);

            // Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void HasGraduated_NotPassed()
        {
            // Arrange
            var tracker = new GraduationTracker();
            var expectedResult = new Tuple<bool, STANDING>(false, STANDING.Remedial);
            var diploma = new Diploma
            {
                Id = 1,
                Credits = 4,
                Requirements = new int[] { 100, 102, 103, 104 }
            };
            var student = new Student
            {
                Id = 4,
                Courses = new Course[]
                    {
                        new Course{Id = 1, Name = "Math", Mark = 40 },
                        new Course{Id = 2, Name = "Science", Mark = 40 },
                        new Course{Id = 3, Name = "Literature", Mark = 40 },
                        new Course{Id = 4, Name = "Physichal Education", Mark = 40 }
                    }
            };

            // Act
            var actualResult = tracker.HasGraduated(diploma, student);

            // Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void GetReportCard_Average()
        {
            // Arrange
            var tracker = new GraduationTracker();
            var expectedAverage = 95;
            var diploma = new Diploma
            {
                Id = 1,
                Credits = 4,
                Requirements = new int[] { 100, 102, 103, 104 }
            };
            var student = new Student
            {
                Id = 1,
                Courses = new Course[]
                   {
                        new Course{Id = 1, Name = "Math", Mark = 95 },
                        new Course{Id = 2, Name = "Science", Mark = 95 },
                        new Course{Id = 3, Name = "Literature", Mark = 95 },
                        new Course{Id = 4, Name = "Physichal Education", Mark = 95 }
                   }
            };

            // Act
            var reportCard = tracker.GetReportCard(diploma, student);

            // Assert
            Assert.AreEqual(expectedAverage, reportCard.Average);
        }

        [TestMethod]
        public void GetStanding()
        {
            // Arrange
            var tracker = new GraduationTracker();
            var average = 75;
            var mockStanding = STANDING.Average;

            // Act
            var actualStanding = GraduationTracker.GetStanding(average);

            // Assert
            Assert.AreEqual(mockStanding, actualStanding);
        }
    }
}
