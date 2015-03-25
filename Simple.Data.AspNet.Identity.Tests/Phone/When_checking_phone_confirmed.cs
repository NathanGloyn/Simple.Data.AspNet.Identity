using System;
using NUnit.Framework;

namespace Simple.Data.AspNet.Identity.Tests.Phone
{
    [TestFixture]
    public class When_checking_phone_confirmed
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
            Assert.Throws<ObjectDisposedException>(async () => await _target.GetPhoneNumberConfirmedAsync(new IdentityUser()));
        }

        [Test]
        public void Should_throw_ArgumentNullException_if_user_is_null()
        {
            Assert.Throws<ArgumentNullException>(async () => await _target.GetPhoneNumberConfirmedAsync(null));
        }

        [Test]
        public async void Should_return_phone_confirmed()
        {
            var phoneConfirmed = await _target.GetPhoneNumberConfirmedAsync(TestData.GetTestUserJohn());

            Assert.That(phoneConfirmed, Is.True);
        }
    }
}
