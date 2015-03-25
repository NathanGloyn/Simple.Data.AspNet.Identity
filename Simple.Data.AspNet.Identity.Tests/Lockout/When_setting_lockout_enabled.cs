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
        public void Should_throw_ObjectDisposedException_if_disposed()
        {
            var target = new UserStore<IdentityUser>();
            target.Dispose();
            Assert.Throws<ObjectDisposedException>(async () => await target.SetLockoutEnabledAsync(new IdentityUser(), true));
        }

        [Test]
        public void Should_throw_ArgumentNullException_if_user_is_null()
        {
            var target = new UserStore<IdentityUser>();

            Assert.Throws<ArgumentNullException>(async () => await target.SetLockoutEnabledAsync(null,true));
        }

        [Test]
        public async void Should_set_the_lockout_enabled()
        {
            var target = new UserStore<IdentityUser>();

            await target.SetLockoutEnabledAsync(TestData.GetTestUserJohn(), true);

            var db = Database.Open();
            IdentityUser record = await db.AspNetUsers.FindAllByUserName("John").SingleOrDefault();

            Assert.That(record.LockoutEnabled, Is.True);
        }
    }
}