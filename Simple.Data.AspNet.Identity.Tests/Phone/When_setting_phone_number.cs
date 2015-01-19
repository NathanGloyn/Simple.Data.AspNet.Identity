using System;
using NUnit.Framework;

namespace Simple.Data.AspNet.Identity.Tests.Phone
{
    [TestFixture]
    public class When_setting_phone_number
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

            Assert.Throws<ArgumentNullException>(() => target.SetPhoneNumberAsync(null, ""));
        }

        [Test]
        public void Should_set_phone()
        {
            var target = new UserStore<IdentityUser>();

            var task = target.SetPhoneNumberAsync(TestData.GetTestUserJohn(), "987-654");

            task.Wait();

            var db = Database.Open();
            IdentityUser userDetails = db.AspNetUsers.FindAllById(TestData.John_UserId).FirstOrDefault();

            Assert.That(userDetails.PhoneNumber, Is.EqualTo("987-654"));

        }         
    }
}