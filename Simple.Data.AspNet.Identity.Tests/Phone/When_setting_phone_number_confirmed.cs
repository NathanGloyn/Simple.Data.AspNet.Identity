using System;
using NUnit.Framework;

namespace Simple.Data.AspNet.Identity.Tests.Phone
{
    [TestFixture]
    public class When_setting_phone_number_confirmed
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
        public void Should_throw_ObjectDisposedException_calling_FindByBName_and_disposed()
        {
            var target = new UserStore<IdentityUser>();
            target.Dispose();
            Assert.Throws<ObjectDisposedException>(async () => await target.GetPhoneNumberConfirmedAsync(new IdentityUser()));
        }
       
        [Test]
        public void Should_throw_ArgumentNullException_if_user_is_null()
        {
            Assert.Throws<ArgumentNullException>(async () => await _target.SetPhoneNumberConfirmedAsync(null,false));
        }

        [Test]
        public async void Should_set_phone_confirmed()
        {
            await _target.SetPhoneNumberConfirmedAsync(TestData.GetTestUserSue(), true);

            var db = Database.Open();
            IdentityUser user = await db.AspNetUsers.FindAllById(TestData.Sue_UserId).FirstOrDefault();

            Assert.That(user.PhoneNumberConfirmed, Is.True);
        }
    }
}