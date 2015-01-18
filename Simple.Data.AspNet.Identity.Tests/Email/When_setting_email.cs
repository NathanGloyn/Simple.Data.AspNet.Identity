using System;
using NUnit.Framework;

namespace Simple.Data.AspNet.Identity.Tests.Email
{
    [TestFixture]
    public class When_setting_email
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

            Assert.Throws<ArgumentNullException>(() => target.SetEmailAsync(null, ""));
        }

        [Test]
        public void Should_set_email()
        {
            var target = new UserStore<IdentityUser>();

            var task = target.SetEmailAsync(TestData.GetTestUserJohn(), "John.Boy@test.com");

            task.Wait();

            var db = Database.Open();
            IdentityUser userDetails = db.AspNetUsers.FindAllById(TestData.John_UserId).FirstOrDefault();

            Assert.That(userDetails.Email, Is.EqualTo("John.Boy@test.com"));

        }
    }
}