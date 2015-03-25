using System;
using System.Threading.Tasks;
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
            Assert.Throws<ArgumentNullException>(async () => await _target.SetPasswordHashAsync(null, ""));
        }

        [Test]
        public void Should_throw_ObjectDisposedException_if_disposed()
        {
            _target.Dispose();
            Assert.Throws<ObjectDisposedException>(async () => await _target.SetPasswordHashAsync(new IdentityUser(), ""));
        }

        [Test]
        public void Should_throw_argument_exception_if_password_hash_is_null_empty_or_whitespace([Values(null, "", " ")] string hash)
        {
            var user = TestData.GetTestUserJohn();

            Assert.Throws<ArgumentException>(async () => await _target.SetPasswordHashAsync(user, hash));
        }

        [Test]
        public async void Should_set_password_hash_correctly()
        {
            var user = TestData.GetTestUserJohn();

            var hashValue = Guid.NewGuid().ToString();

            await _target.SetPasswordHashAsync(user, hashValue);

            var userFromDb = await GetTestUser();

            Assert.That(userFromDb.PasswordHash, Is.EqualTo(hashValue));
        }

        private async Task<IdentityUser> GetTestUser()
        {
            dynamic db = Database.Open();
            IdentityUser user = await db.AspNetUsers.FindAllByUserName("John").SingleOrDefault();

            return user;
        }
    }
}