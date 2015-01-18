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
        public void Should_throw_ArgumentNullException_if_user_is_null()
        {
            Assert.Throws<ArgumentNullException>(() => _target.SetEmailConfirmedAsync(null,false));
        }

        [Test]
        public void Should_set_email_confirmed()
        {
            var task = _target.SetEmailConfirmedAsync(TestData.GetTestUserSue(), true);
            task.Wait();

            var db = Database.Open();
            IdentityUser user = db.AspNetUsers.FindAllById(TestData.Sue_UserId).FirstOrDefault();

            Assert.That(user.EmailConfirmed, Is.True);
        }
    }
}