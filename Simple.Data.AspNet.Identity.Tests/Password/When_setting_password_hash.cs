using System;
using NUnit.Framework;

namespace Simple.Data.AspNet.Identity.Tests.Password
{
    [TestFixture]
    public class When_setting_password_hash
    {
        private UserStore<IdentityUser> _target;

        [SetUp]
        public void SetUp()
        {
            DatabaseHelper.Reset();
            _target = new UserStore<IdentityUser>();   
            TestData.AddUsers();
        }

        [Test]
        public void Should_throw_argument_null_exception_if_user_is_null()
        {
            Assert.Throws<ArgumentNullException>(() => _target.SetPasswordHashAsync(null, ""));
        }

        [Test]
        public void Should_throw_argument_exception_if_password_hash_is_null_empty_or_whitespace([Values(null, "", " ")] string hash)
        {
            var user = TestData.GetTestUserJohn();

            Assert.Throws<ArgumentException>(() => _target.SetPasswordHashAsync(user, hash));
        }

        [Test]
        public void Should_set_password_hash_correctly()
        {
            var user = TestData.GetTestUserJohn();

            var hashValue = Guid.NewGuid().ToString();

            var task = _target.SetPasswordHashAsync(user, hashValue);

            task.Wait();

            var userFromDb = GetTestUser();

            Assert.That(userFromDb.PasswordHash, Is.EqualTo(hashValue));
        }

        private IdentityUser GetTestUser()
        {
            dynamic db = Database.Open();
            return db.AspNetUsers.FindAllByUserName("John").SingleOrDefault();
        }
    }
}