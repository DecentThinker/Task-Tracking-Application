using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTracker_Modules;

namespace TaskTracker_Tests.Module_Tests
{
    public class UserProfileServiceTests
    {
        private UserProfileService userProfileService;

        [SetUp]
        public void SetUp()
        {
            // Initialize the UserProfileService before each test
            userProfileService = new UserProfileService();
        }

        [Test]
        public void AddProfile_ValidProfile_SuccessfullyAdded()
        {
            // Arrange
            UserProfile userProfile = new UserProfile("user1", "John Doe", "john@example.com");

            // Act
            userProfileService.AddProfile(userProfile);
            UserProfile retrievedProfile = userProfileService.GetProfile("user1");

            // Assert
            Assert.NotNull(retrievedProfile);
            Assert.AreEqual("user1", retrievedProfile.Username);
            Assert.AreEqual("John Doe", retrievedProfile.FullName);
            Assert.AreEqual("john@example.com", retrievedProfile.Email);
        }

        [Test]
        public void UpdateProfile_ExistingProfile_SuccessfullyUpdated()
        {
            // Arrange
            UserProfile userProfile = new UserProfile("user1", "John Doe", "john@example.com");
            userProfileService.AddProfile(userProfile);

            // Act
            bool isUpdated = userProfileService.UpdateProfile("user1", "Jane Smith", "jane@example.com");
            UserProfile updatedProfile = userProfileService.GetProfile("user1");

            // Assert
            Assert.IsTrue(isUpdated);
            Assert.AreEqual("Jane Smith", updatedProfile.FullName);
            Assert.AreEqual("jane@example.com", updatedProfile.Email);
        }

        [Test]
        public void UpdateProfile_NonExistingProfile_ReturnsFalse()
        {
            // Act
            bool isUpdated = userProfileService.UpdateProfile("user1", "Jane Smith", "jane@example.com");

            // Assert
            Assert.IsFalse(isUpdated);
        }

        [Test]
        public void DeleteProfile_ExistingProfile_SuccessfullyDeleted()
        {
            // Arrange
            UserProfile userProfile = new UserProfile("user1", "John Doe", "john@example.com");
            userProfileService.AddProfile(userProfile);

            // Act
            bool isDeleted = userProfileService.DeleteProfile("user1");
            UserProfile deletedProfile = userProfileService.GetProfile("user1");

            // Assert
            Assert.IsTrue(isDeleted);
            Assert.IsNull(deletedProfile);
        }

        [Test]
        public void DeleteProfile_NonExistingProfile_ReturnsFalse()
        {
            // Act
            bool isDeleted = userProfileService.DeleteProfile("user1");

            // Assert
            Assert.IsFalse(isDeleted);
        }
    }
}
