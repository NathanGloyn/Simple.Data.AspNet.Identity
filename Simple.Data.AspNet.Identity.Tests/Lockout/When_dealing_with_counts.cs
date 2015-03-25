using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Simple.Data.AspNet.Identity.Tests.Lockout
{
    [TestFixture]
    public class When_dealing_with_counts
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
            Assert.Throws<ArgumentNullException>(async() => await _target.IncrementAccessFailedCountAsync(null));
            Assert.Throws<ArgumentNullException>(async() => await _target.ResetAccessFailedCountAsync(null));
            Assert.Throws<ArgumentNullException>(async() => await _target.GetAccessFailedCountAsync(null));
        }

        [Test]
        public void Should_throw_ObjectDisposedException_if_disposed()
        {
           _target.Dispose();
            Assert.Throws<ObjectDisposedException>(async () => await _target.IncrementAccessFailedCountAsync(new IdentityUser()));
            Assert.Throws<ObjectDisposedException>(async () => await _target.ResetAccessFailedCountAsync(new IdentityUser()));
            Assert.Throws<ObjectDisposedException>(async () => await _target.GetAccessFailedCountAsync(new IdentityUser()));
        }

        [Test]
        public async void Should_increment_the_failed_count()
        {
            var failedCount = await _target.IncrementAccessFailedCountAsync(TestData.GetTestUserJohn());

            Assert.That(failedCount , Is.EqualTo(1));
        }

        [Test]
        public async void Should_reset_the_failed_count()
        {
            await _target.ResetAccessFailedCountAsync(TestData.GetTestUserLockedOut());

            var db = Database.Open();
            IdentityUser record = await db.AspNetUsers.FindAllById(TestData.LockedOut_UserId).SingleOrDefault();

            Assert.That(record.AccessFailedCount, Is.EqualTo(0));
        }

        [Test]
        public async void Should_get_the_failed_count()
        {
            var failedAccessCount = await _target.GetAccessFailedCountAsync(TestData.GetTestUserLockedOut());

            Assert.That(failedAccessCount,Is.EqualTo(5));
        }
    }
}