using System;
using NUnit.Framework;

namespace Simple.Data.AspNet.Identity.Tests.Password
{
    [TestFixture]
    public class When_checking_for_password_hash
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
        public void Should_return_false_if_no_hash_found_for_user()
        {
            var user = new IdentityUser { Id = TestData.UserNoPasswordHash_UserId, UserName = "Jayne" };

            var task = _target.HasPasswordAsync(user);

            task.Wait();

            Assert.That(task.Result, Is.False);
        }

        [Test]
        public void Should_return_true_for_user_with_hash()
        {
            var user = new IdentityUser { Id = TestData.John_UserId, UserName = "John" };

            var task = _target.HasPasswordAsync(user);

            task.Wait();

            Assert.That(task.Result, Is.True);            
        }    
    }
}