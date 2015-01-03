using Microsoft.AspNet.Identity;
using NUnit.Framework;

namespace Simple.Data.AspNet.Identity.Tests {
    [TestFixture]
    public class When_used_with_UserManager {

        [Test]
        public void Should_be_able_to_create_manager() {
            var userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>());

            Assert.IsNotNull(userManager);
        }
    }
}