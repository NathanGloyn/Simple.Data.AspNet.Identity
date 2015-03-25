using System;
using NUnit.Framework;

namespace Simple.Data.AspNet.Identity.Tests.Email
{
    [TestFixture]
    public class When_setting_email_confirmed
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
            _target.Dispose();
            Assert.Throws<ObjectDisposedException>(async () => await _target.SetEmailConfirmedAsync(new IdentityUser(), true));
        }
       
        [Test]
        public void Should_throw_ArgumentNullException_if_user_is_null()
        {
            Assert.Throws<ArgumentNullException>(async () => await _target.SetEmailConfirmedAsync(null,false));
        }

        [Test]
        public async void Should_set_email_confirmed()
        {
            await _target.SetEmailConfirmedAsync(TestData.GetTestUserSue(), true);

            var db = Database.Open();
            IdentityUser user = await db.AspNetUsers.FindAllById(TestData.Sue_UserId).FirstOrDefault();

            Assert.That(user.EmailConfirmed, Is.True);
        }
    }
}