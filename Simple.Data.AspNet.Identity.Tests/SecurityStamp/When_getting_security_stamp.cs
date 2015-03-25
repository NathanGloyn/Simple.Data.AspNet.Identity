using System;
using NUnit.Framework;

namespace Simple.Data.AspNet.Identity.Tests.SecurityStamp
{
    public class When_getting_security_stamp
    {
        private UserStore<IdentityUser> _target;

        [SetUp]
        public void SetUp()
        {
            _target = new UserStore<IdentityUser>();            
        }

        [Test]
        public void Should_throw_ObjectDisposedException_calling_FindByBName_and_disposed()
        {
            _target.Dispose();
            Assert.Throws<ObjectDisposedException>(async () => await _target.GetSecurityStampAsync(new IdentityUser()));
        }

        [Test]
        public void Should_throw_ArgumentNullException_if_user_is_null()
        {
            Assert.Throws<ArgumentNullException>( async () => await _target.GetSecurityStampAsync(null));
        }

        [Test]
        public async void Should_get_security_stamp_for_user()
        {
            var user = TestData.GetTestUserJohn();
            var securityStamp = await _target.GetSecurityStampAsync(user);

            Assert.That(securityStamp, Is.EqualTo(user.SecurityStamp));
        }
    }
}