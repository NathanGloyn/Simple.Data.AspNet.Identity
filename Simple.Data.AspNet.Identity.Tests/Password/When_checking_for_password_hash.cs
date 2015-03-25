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
            Assert.Throws<ArgumentNullException>(async () => await _target.HasPasswordAsync(null));
        }

        [Test]
        public async void Should_return_false_if_no_hash_found_for_user()
        {
            var user = new IdentityUser { Id = TestData.UserNoPasswordHash_UserId, UserName = "Jayne" };

            var hasPassword = await _target.HasPasswordAsync(user);

            Assert.That(hasPassword, Is.False);
        }

        [Test]
        public async void Should_return_true_for_user_with_hash()
        {
            var user = TestData.GetTestUserJohn();

            var hasPassword = await _target.HasPasswordAsync(user);

            Assert.That(hasPassword, Is.True);            
        }    
    }
}