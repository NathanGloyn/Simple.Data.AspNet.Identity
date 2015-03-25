using System;
using NUnit.Framework;

namespace Simple.Data.AspNet.Identity.Tests.SecurityStamp
{
    [TestFixture]
    public class When_setting_security_stamp
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
            Assert.Throws<ObjectDisposedException>( async () => await _target.SetSecurityStampAsync(new IdentityUser(),""));
        }

        [Test]
        public void Should_throw_ArgumentNullException_if_user_is_null()
        {
            Assert.Throws<ArgumentNullException>( async () => await _target.SetSecurityStampAsync(null,""));
        }

        [Test]
        public async void Should_set_the_security_stamp_for_the_user()
        {
            await _target.SetSecurityStampAsync(TestData.GetTestUserJohn(), "newStamp");

            var db = Database.Open();
            IdentityUser user = await db.AspNetUsers.FindAllById(TestData.John_UserId).FirstOrDefault();

            Assert.That(user.SecurityStamp, Is.EqualTo("newStamp"));
        }
    }
}