using System;
using NUnit.Framework;

namespace Simple.Data.AspNet.Identity.Tests.Lockout
{
    [TestFixture]
    public class When_getting_lockout_enddate
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
        public void Should_throw_ObjectDisposedException_if_disposed()
        {
            _target.Dispose();
            Assert.Throws<ObjectDisposedException>(async () => await _target.GetLockoutEndDateAsync(new IdentityUser()));
        }

        [Test]
        public void Should_throw_ArgumentNullException_if_user_is_null()
        {
            Assert.Throws<ArgumentNullException>(async () => await _target.GetLockoutEndDateAsync(null));
        }

        [Test]
        public async void Should_return_default_DateTimeOfset_if_user_has_no_lockout_date()
        {
            var lockoutDate = await _target.GetLockoutEndDateAsync(TestData.GetTestUserJohn());

            var expected = new DateTimeOffset();

            Assert.That(lockoutDate.Date, Is.EqualTo(expected.Date));
        }

        [Test]
        public async void Should_return_expected_DateTimeOffset()
        {
            var user = new IdentityUser { Id = TestData.LockedOut_UserId };

            var lockoutDate = await _target.GetLockoutEndDateAsync(user);

            var expected = new DateTimeOffset(DateTime.SpecifyKind(DateTime.Now.AddHours(1), DateTimeKind.Utc));

            Assert.That(lockoutDate.Date, Is.EqualTo(expected.Date));
            Assert.That(lockoutDate.Hour, Is.EqualTo(expected.Hour));
        }
    }
}