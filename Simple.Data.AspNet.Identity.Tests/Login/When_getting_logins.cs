using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Identity;
using NUnit.Framework;

namespace Simple.Data.AspNet.Identity.Tests.Login
{
    [TestFixture]
    public class When_getting_logins
    {
        private UserStore<IdentityUser> _target;

        [SetUp]
        public void SetUp()
        {
            DatabaseHelper.Reset();
            TestData.AddUsers();
            TestData.AdLoginsForUsers();
            _target = new UserStore<IdentityUser>();
        }

        [Test]
        public void Should_throw_ArgumentNullException_if_user_is_null()
        {
            Assert.Throws<ArgumentNullException>(() => _target.GetLoginsAsync(null));
        }

        [Test]
        public void Should_return_emptylist_if_user_has_no_logins()
        {
            var task = _target.GetLoginsAsync(TestData.GetTestUserSue());

            task.Wait();

            Assert.That(task.Result, Is.Empty);
        }

        [Test]
        public void Should_return_expected_logins()
        {
            var task = _target.GetLoginsAsync(TestData.GetTestUserJohn());

            task.Wait();

            Assert.That(task.Result.Count, Is.EqualTo(3));

            var expectedLogins = new List<UserLoginInfo>
            {
                new UserLoginInfo("Google", "abc"),
                new UserLoginInfo("GitHub", "123"),
                new UserLoginInfo("Facebook", "xyz-123")
            };

            foreach (var userLoginInfo in expectedLogins)
            {
                Assert.That(task.Result.Any(x => x.LoginProvider ==userLoginInfo.LoginProvider),Is.True);
            }
        }
    }
}