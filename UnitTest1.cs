using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using gibdd_uchpr.window;
using gibdd_uchpr.model;
using Moq;
using System.Linq;
using System.Data.Entity;

namespace gibdd_tests
{
    [TestClass]
    public class UnitTest1
    {

        [TestMethod]
        public void Test_SuccessfulAuthorization_Inspector()
        {
            var mockContext = new Mock<gibddEntities>();
            var mockDbSet = MockDbSet(new Users { log_in = "inspector", passwword = "inspector" });
            mockContext.Setup(c => c.Users).Returns(mockDbSet.Object);
            var authWindow = new Authorization();
            authWindow.LoginTextBox.Text = "inspector";
            authWindow.PasswordBox.Text = "inspector";
            authWindow.loginButton(null, null);
            Assert.AreEqual(0, authWindow.failedAttempts);
            Assert.IsNull(authWindow.lockEndTime);
        }

        [TestMethod]
        public void Test_FailedAuthorization_WrongPassword()
        {
            var mockContext = new Mock<gibddEntities>();
            var mockDbSet = MockDbSet(new Users { log_in = "inspector", passwword = "inspector" });
            mockContext.Setup(c => c.Users).Returns(mockDbSet.Object);
            var authWindow = new Authorization();
            authWindow.LoginTextBox.Text = "inspector";
            authWindow.PasswordBox.Text = "wrongpassword";
            authWindow.loginButton(null, null);
            Assert.AreEqual(1, authWindow.failedAttempts);
            Assert.IsNull(authWindow.lockEndTime);
        }

        [TestMethod]
        public void Test_FailedAuthorization_LockAfterThreeAttempts()
        {
            var mockContext = new Mock<gibddEntities>();
            var mockDbSet = MockDbSet(new Users { log_in = "inspector", passwword = "inspector" });
            mockContext.Setup(c => c.Users).Returns(mockDbSet.Object);
            var authWindow = new Authorization();
            authWindow.LoginTextBox.Text = "inspector";
            authWindow.PasswordBox.Text = "wrongpassword";

            for (int i = 0; i < 3; i++)
            {
                authWindow.loginButton(null, null);
            }

            Assert.AreEqual(3, authWindow.failedAttempts);
            Assert.IsNotNull(authWindow.lockEndTime);
            Assert.IsTrue((authWindow.lockEndTime - DateTime.UtcNow).Value.TotalSeconds < 60, "Lock time should be set for approximately 1 minute from now.");
        }

        [TestMethod]
        public void Test_FailedAuthorization_EmptyFields()
        {
            var authWindow = new Authorization();
            authWindow.LoginTextBox.Text = "";
            authWindow.PasswordBox.Text = "";

            authWindow.loginButton(null, null);

            Assert.AreEqual(0, authWindow.failedAttempts);
            Assert.IsNull(authWindow.lockEndTime);
        }

        [TestMethod]
        public void Test_FailedAuthorization_UnknownUser()
        {
            var mockContext = new Mock<gibddEntities>();
            var mockDbSet = MockDbSet(new Users { log_in = "inspector", passwword = "inspector" });
            mockContext.Setup(c => c.Users).Returns(mockDbSet.Object);
            var authWindow = new Authorization();
            authWindow.LoginTextBox.Text = "unknown";
            authWindow.PasswordBox.Text = "unknown";
            authWindow.loginButton(null, null);

            Assert.AreEqual(1, authWindow.failedAttempts);
            Assert.IsNull(authWindow.lockEndTime);
        }
        private static Mock<System.Data.Entity.DbSet<Users>> MockDbSet(params Users[] entities)
        {
            var queryable = entities.AsQueryable();
            var mockSet = new Mock<System.Data.Entity.DbSet<Users>>();

            mockSet.As<IQueryable<Users>>().Setup(m => m.Provider).Returns(queryable.Provider);
            mockSet.As<IQueryable<Users>>().Setup(m => m.Expression).Returns(queryable.Expression);
            mockSet.As<IQueryable<Users>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            mockSet.As<IQueryable<Users>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());
            mockSet.Setup(m => m.Add(It.IsAny<Users>())).Callback<Users>(user => queryable.ToList().Add(user));
            mockSet.Setup(m => m.Remove(It.IsAny<Users>())).Callback<Users>(user => queryable.ToList().Remove(user));
            return mockSet;
        }

    }
}
