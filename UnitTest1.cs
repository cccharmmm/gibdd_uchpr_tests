using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Linq;
using System.Data.Entity;
using gibdd_uchpr.model;
using gibdd_uchpr.window;
using System.Windows;
using System.Collections.Generic;

namespace gibdd_tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Test_SuccessfulAuthorization_ValidUserAndPassword()
        {
            var mockContext = new Mock<gibddEntities>();
            var authWindow = new Authorization(mockContext.Object);
            authWindow.LoginTextBox.Text = "";
            authWindow.PasswordBox.Text = "";
            Application.ResourceAssembly = typeof(Authorization).Assembly;
            authWindow.loginButton(null, null);
            Assert.AreEqual(0, authWindow.failedAttempts);
            Assert.IsNull(authWindow.lockEndTime);
        }
        [TestMethod]
        public void Test_FailedAuthorization_WithIncorrectUsername()
        {
            var mockContext = new Mock<gibddEntities>();
            var authWindow = new Authorization(mockContext.Object);
            authWindow.LoginTextBox.Text = "";
            authWindow.PasswordBox.Text = "";
            Application.ResourceAssembly = typeof(Authorization).Assembly;
            authWindow.loginButton(null, null);
            Assert.AreEqual(0, authWindow.failedAttempts);
            Assert.IsNull(authWindow.lockEndTime);
        }

        [TestMethod]
        public void Test_FailedAuthorization_WithIncorrectPassword()
        {
            var mockContext = new Mock<gibddEntities>();
            var authWindow = new Authorization(mockContext.Object);
            authWindow.LoginTextBox.Text = "";
            authWindow.PasswordBox.Text = "";
            Application.ResourceAssembly = typeof(Authorization).Assembly;
            authWindow.loginButton(null, null);
            Assert.AreEqual(0, authWindow.failedAttempts);
            Assert.IsNull(authWindow.lockEndTime);
        }
        [TestMethod]
        public void Test_FailedAuthorization_EmptyFields()
        {
            var mockContext = new Mock<gibddEntities>();
            var authWindow = new Authorization(mockContext.Object);
            authWindow.LoginTextBox.Text = "";
            authWindow.PasswordBox.Text = "";
            Application.ResourceAssembly = typeof(Authorization).Assembly;
            authWindow.loginButton(null, null);
            Assert.AreEqual(0, authWindow.failedAttempts);
            Assert.IsNull(authWindow.lockEndTime);
        }
        private static Mock<DbSet<Users>> MockDbSet(params Users[] entities)
        {
            var data = entities.AsQueryable();
            var mockSet = new Mock<DbSet<Users>>();

            mockSet.As<IQueryable<Users>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Users>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Users>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Users>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            mockSet.Setup(m => m.Add(It.IsAny<Users>())).Callback<Users>(user => data.Append(user));
            mockSet.Setup(m => m.Remove(It.IsAny<Users>())).Callback<Users>(user => data.Where(u => u != user));
            mockSet.Setup(m => m.Find(It.IsAny<object[]>())).Returns((object[] ids) => data.FirstOrDefault());

            return mockSet;
        }
    }
}
