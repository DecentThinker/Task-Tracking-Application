using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task_Tracker;
using TaskTracker_Modules;

namespace TaskTracker_Tests.Module_Tests
{
    public class EmailTest
    {
        GmailVerification gmail;
        [SetUp]
        public void SetUp()
        {
          
        }

        [Test]
        public void Authenticate_ValidCredentials_ReturnsTrue()
        {
            // Arrange
            string res;
            gmail = new GmailVerification("descent.aman01@gmail.com");

            // Act
            res = gmail.Validity();
            bool ans = res == "";

            // Assert
            Assert.IsTrue(ans,"Verified Email");
        }

        [Test]
        public void Authenticate_InvalidCredentials_ReturnsFalse()
        {
            // Arrange
            string res;
            gmail = new GmailVerification("descent@gmail.com");

            // Act
            res = gmail.Validity();
            bool ans = res == "";

            // Assert
            Assert.IsFalse(ans);
        }
    }
}
