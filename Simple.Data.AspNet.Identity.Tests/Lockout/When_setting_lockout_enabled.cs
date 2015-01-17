using System;
using NUnit.Framework;

namespace Simple.Data.AspNet.Identity.Tests.Lockout
{
    [TestFixture]
    public class When_setting_lockout_enabled
    {

        [SetUp]
        public void SetUp()
        {
            DatabaseHelper.Reset();
            TestData.AddUsers();
        }

        [Test]
        public void Should_throw_ArgumentNullException_if_user_is_null()
        {
            var target = new UserStore<IdentityUser>();

            Assert.Throws<ArgumentNullException>(() => target.SetLockoutEnabledAsync(null,true));
        }

        [Test]
        public void Should_set_the_lockout_enabled()
        {
            var target = new UserStore<IdentityUser>();

            var task = target.SetLockoutEnabledAsync(TestData.GetTestUserJohn(), true);

            task.Wait();

            var db = Database.Open();
            IdentityUser record = db.AspNetUsers.FindAllByUserName("John").SingleOrDefault();

            Assert.That(record.LockoutEnabled, Is.True);
        }
    }
}