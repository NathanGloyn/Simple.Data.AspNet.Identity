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
            Assert.Throws<ArgumentNullException>(() => _target.IncrementAccessFailedCountAsync(null));
            Assert.Throws<ArgumentNullException>(() => _target.ResetAccessFailedCountAsync(null));
            Assert.Throws<ArgumentNullException>(() => _target.GetAccessFailedCountAsync(null));
        }

        [Test]
        public void Should_throw_ObjectDisposedException_if_disposed()
        {
           _target.Dispose();
            Assert.Throws<ObjectDisposedException>(() => _target.IncrementAccessFailedCountAsync(new IdentityUser()));
            Assert.Throws<ObjectDisposedException>(() => _target.ResetAccessFailedCountAsync(new IdentityUser()));
            Assert.Throws<ObjectDisposedException>(() => _target.GetAccessFailedCountAsync(new IdentityUser()));
        }

        [Test]
        public void Should_increment_the_failed_count()
        {
            var task = _target.IncrementAccessFailedCountAsync(TestData.GetTestUserJohn());

            task.Wait();


            Assert.That(task.Result , Is.EqualTo(1));
        }

        [Test]
        public void Should_reset_the_failed_count()
        {
            dynamic task = _target.ResetAccessFailedCountAsync(TestData.GetTestUserLockedOut());

            task.Wait();

            Assert.That(task.Result , Is.EqualTo(0));

            var db = Database.Open();
            IdentityUser record = db.AspNetUsers.FindAllById(TestData.LockedOut_UserId).SingleOrDefault();

            Assert.That(record.AccessFailedCount, Is.EqualTo(0));
        }

        [Test]
        public void Should_get_the_failed_count()
        {
            var task = _target.GetAccessFailedCountAsync(TestData.GetTestUserLockedOut());

            task.Wait();

            Assert.That(task.Result,Is.EqualTo(5));
        }
    }
}