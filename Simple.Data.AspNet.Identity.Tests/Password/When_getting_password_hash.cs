using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Simple.Data.AspNet.Identity.Tests.Password {
    [TestFixture]
    public class When_getting_password_hash 
    {
        private UserStore<IdentityUser> _target;

        [SetUp]
        public void SetUp()
        {
            DatabaseHelper.Reset();
            TestData.AddUsers();
            _target = new UserStore<IdentityUser>();
        }

        [Test]
        public void Should_throw_ArgumentNullException_if_user_is_null()
        {
            Assert.Throws<ArgumentNullException>(() => _target.GetPasswordHashAsync(null));
        }

        [Test]
        public void Should_return_empty_string_for_invalid_user_id()
        {
            var user = new IdentityUser{Id = "abc"};

            var task = _target.GetPasswordHashAsync(user);

            task.Wait();

            Assert.That(task.Result, Is.EqualTo(""));
        }

        [Test]
        public void Should_return_user_password_hash()
        {
            var user = new IdentityUser { Id = TestData.John_UserId };

            var task = _target.GetPasswordHashAsync(user);

            task.Wait();

            Assert.That(task.Result, Is.EqualTo("sa;ldfkjsldfjlajte"));
        }
    }
}