using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GraduationTracker.Tests.Unit
{
    [TestClass]
    public class RepositoryTests
    {
        [TestMethod]
        public void GetRequirementById()
        {
            // Arrange
            var mockRequirement = new Requirement
            {
                Id = 100,
                Name = "Math",
                MinimumMark = 50,
                CourseIds = new int[] { 1 },
                Credits = 1
            };

            // Act
            var requirementFromRepo = Repository.GetRequirement(100);

            // Assert
            Assert.AreEqual(mockRequirement.Id, requirementFromRepo.Id);
        }

        [TestMethod]
        public void GetStudentById()
        {
            // Arrange
            var mockStudent = new Student
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
            var studentFromRepo = Repository.GetStudent(1);

            // Assert
            Assert.AreEqual(mockStudent.Id, studentFromRepo.Id);
        }

        [TestMethod]
        public void GetDiplomaById()
        {
            // Arrange
            var mockDiploma = new Diploma
            {
                Id = 1,
                Credits = 4,
                Requirements = new int[] { 100, 102, 103, 104 }
            };

            // Act
            var diplomaFromRepo = Repository.GetDiploma(1);

            // Assert
            Assert.AreEqual(mockDiploma.Id, diplomaFromRepo.Id);
        }
    }
}
