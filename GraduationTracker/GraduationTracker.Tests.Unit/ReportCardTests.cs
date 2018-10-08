using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GraduationTracker.Tests.Unit
{
    [TestClass]
    public class ReportCardTests
    {
        [TestMethod]
        public void ReportCard_Standing()
        {
            // Arrange
            var expectedStanding = STANDING.SumaCumLaude;

            // Act
            var reportCard = new ReportCard
            {
                Average = 95
            };

            // Assert
            Assert.AreEqual(expectedStanding, reportCard.Standing);
        }
    }
}
