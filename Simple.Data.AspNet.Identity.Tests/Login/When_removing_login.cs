using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Identity;
using NUnit.Framework;

namespace Simple.Data.AspNet.Identity.Tests.Login
{
    [TestFixture]
    public class When_removing_login
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
        public void Should_throw_ObjectDisposedException_if_disposed()
        {
            _target.Dispose();
            Assert.Throws<ObjectDisposedException>(async () => await _target.RemoveLoginAsync(new IdentityUser(), new UserLoginInfo("", "")));
        }

        [Test]
        public void Should_throw_ArgumentNullException_if_user_is_null()
        {
            Assert.Throws<ArgumentNullException>(async () => await _target.RemoveLoginAsync(null, new UserLoginInfo("", "")));
        }

        [Test]
        public void Should_throw_ArgumentNullException_if_loginInfo_is_null()
        {
            Assert.Throws<ArgumentNullException>(async () => await _target.RemoveLoginAsync(TestData.GetTestUserJohn(), null));
        }

        [Test]
        public void Should_not_error_if_login_does_not_exist()
        {
            var login = new UserLoginInfo("Yahoo", "qwe");
            Assert.DoesNotThrow(async ()=> await _target.RemoveLoginAsync(TestData.GetTestUserJohn(), login));
        }

        [Test]
        public async void Should_remove_login_from_user()
        {
            var login = new UserLoginInfo("Google", "123");
            await _target.RemoveLoginAsync(TestData.GetTestUserJohn(), login);

            var db = Database.Open();
            var records = await db.AspNetUserLogins.FindAllByUserId(TestData.John_UserId)
                .Select(db.AspNetUserLogins.LoginProvider, db.AspNetUserLogins.ProviderKey);

            List<UserLoginInfo> logins = new List<UserLoginInfo>();
            foreach (var record in records)
            {
                logins.Add(new UserLoginInfo(record.LoginProvider, record.ProviderKey));
            }

            Assert.That(logins.Count, Is.EqualTo(2));
            Assert.That(logins.Any(x => x.LoginProvider == "Google"), Is.False);
        }
    }
}