using System;
using NUnit.Framework;

namespace Simple.Data.AspNet.Identity.Tests.Email
{
    [TestFixture]
    public class When_getting_email
    {
        [Test]
        public void Should_throw_ObjectDisposedException_calling_FindByBName_and_disposed()
        {
            var target = new UserStore<IdentityUser>();
            target.Dispose();
            Assert.Throws<ObjectDisposedException>(async () => await target.GetEmailAsync(new IdentityUser()));
        }

        [Test]
        public void Should_throw_ArgumentNullException_if_user_is_null()
        {
            var target = new UserStore<IdentityUser>();

            Assert.Throws<ArgumentNullException>(async () => await target.GetEmailAsync(null));
        }

        [Test]
        public async void Should_return_email_for_user_supplied()
        {
            var target = new UserStore<IdentityUser>();
            var user = TestData.GetTestUserJohn();
            var task = await target.GetEmailAsync(user);

            Assert.That(task, Is.EqualTo(user.Email));
        }
    }
}