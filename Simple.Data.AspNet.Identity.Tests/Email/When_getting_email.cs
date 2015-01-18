using System;
using NUnit.Framework;

namespace Simple.Data.AspNet.Identity.Tests.Email
{
    [TestFixture]
    public class When_getting_email
    {
        [Test]
        public void Should_throw_ArgumentNullException_if_user_is_null()
        {
            var target = new UserStore<IdentityUser>();

            Assert.Throws<ArgumentNullException>(() => target.GetEmailAsync(null));
        }

        [Test]
        public void Should_return_email_for_user_supplied()
        {
            var target = new UserStore<IdentityUser>();
            var user = TestData.GetTestUserJohn();
            var task = target.GetEmailAsync(user);

            task.Wait();

            Assert.That(task.Result, Is.EqualTo(user.Email));
        }
    }
}