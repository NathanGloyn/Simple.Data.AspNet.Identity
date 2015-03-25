using System;
using NUnit.Framework;

namespace Simple.Data.AspNet.Identity.Tests.Phone
{
    [TestFixture]
    public class When_getting_phone_number
    {
        [Test]
        public void Should_throw_ArgumentNullException_if_user_is_null()
        {
            var target = new UserStore<IdentityUser>();

            Assert.Throws<ArgumentNullException>(async () => await target.GetPhoneNumberAsync(null));
        }

        [Test]
        public void Should_throw_ObjectDisposedException_calling_FindByBName_and_disposed()
        {
            var target = new UserStore<IdentityUser>();
            target.Dispose();
            Assert.Throws<ObjectDisposedException>(async () => await target.GetPhoneNumberConfirmedAsync(new IdentityUser()));
        }

        [Test]
        public async void Should_return_phone_number_for_user_supplied()
        {
            var target = new UserStore<IdentityUser>();
            var user = TestData.GetTestUserJohn();
            var phoneNumber = await target.GetPhoneNumberAsync(user);

            Assert.That(phoneNumber, Is.EqualTo(user.PhoneNumber));
        }
    }
}
