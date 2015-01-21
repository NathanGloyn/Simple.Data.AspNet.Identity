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

            Assert.Throws<ArgumentNullException>(() => target.GetPhoneNumberAsync(null));
        }

        [Test]
        public void Should_throw_ObjectDisposedException_calling_FindByBName_and_disposed()
        {
            var target = new UserStore<IdentityUser>();
            target.Dispose();
            Assert.Throws<ObjectDisposedException>(() => target.GetPhoneNumberConfirmedAsync(new IdentityUser()));
        }

        [Test]
        public void Should_return_phone_number_for_user_supplied()
        {
            var target = new UserStore<IdentityUser>();
            var user = TestData.GetTestUserJohn();
            var task = target.GetPhoneNumberAsync(user);

            task.Wait();

            Assert.That(task.Result, Is.EqualTo(user.PhoneNumber));
        }
    }
}
