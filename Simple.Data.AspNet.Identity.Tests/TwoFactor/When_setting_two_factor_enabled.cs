using System;
using NUnit.Framework;

namespace Simple.Data.AspNet.Identity.Tests.TwoFactor
{
    [TestFixture]
    public class When_setting_two_factor_enabled
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

            Assert.Throws<ArgumentNullException>(() => target.SetTwoFactorEnabledAsync(null, true));
        }

        [Test]
        public void Should_set_two_factor_auth_enabled()
        {
            var target = new UserStore<IdentityUser>();

            var task = target.SetTwoFactorEnabledAsync(TestData.GetTestUserJohn(),true);

            task.Wait();

            var db = Database.Open();
            IdentityUser userDetails = db.AspNetUsers.FindAllById(TestData.John_UserId).FirstOrDefault();

            Assert.That(userDetails.TwoFactorEnabled, Is.True);

        }          
    }
}