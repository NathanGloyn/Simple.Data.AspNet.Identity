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
            Assert.Throws<ObjectDisposedException>(() => _target.SetSecurityStampAsync(new IdentityUser(),""));
        }

        [Test]
        public void Should_throw_ArgumentNullException_if_user_is_null()
        {
            Assert.Throws<ArgumentNullException>(() => _target.SetSecurityStampAsync(null,""));
        }

        [Test]
        public void Should_set_the_security_stamp_for_the_user()
        {
            var task = _target.SetSecurityStampAsync(TestData.GetTestUserJohn(), "newStamp");
            task.Wait();

            var db = Database.Open();
            IdentityUser user = db.AspNetUsers.FindAllById(TestData.John_UserId).FirstOrDefault();

            Assert.That(user.SecurityStamp, Is.EqualTo("newStamp"));
        }
    }
}