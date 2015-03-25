using System;
using NUnit.Framework;

namespace Simple.Data.AspNet.Identity.Tests.UserRoles {
    [TestFixture]
    public class When_checking_if_user_is_in_a_role {
        [SetUp]
        public void SetUp()
        {
            DatabaseHelper.Reset();
            TestData.AddRoles();
            TestData.AddUsers();
            TestData.AddRolesToUsers();
        }


        [Test]
        public void Should_throw_argument_null_exception_if_user_is_null() {
            
            var target = new UserStore<IdentityUser>();
            Assert.That(async () => await target.IsInRoleAsync(null,"Admin"),
                Throws.Exception.TypeOf<ArgumentNullException>().With.Message.EqualTo("Value cannot be null.\r\nParameter name: user"));
        }

        [Test]
        public void Should_throw_ObjectDisposedException_if_disposed()
        {
            var target = new UserStore<IdentityUser>();
            target.Dispose();
            Assert.Throws<ObjectDisposedException>(async () => await target.IsInRoleAsync(new IdentityUser(), ""));
        }

        [Test] 
        public void Should_throw_argument_null_exception_if_role_name_invalid([Values(null,""," ")] string roleName) {
            var target = new UserStore<IdentityUser>();
            var user = new IdentityUser();
            Assert.That(async () => await target.IsInRoleAsync(user, roleName),
                Throws.Exception.TypeOf<ArgumentNullException>().With.Message.EqualTo("Value cannot be null.\r\nParameter name: roleName"));            
        }

        [Test]
        public async void Should_return_false_for_unknown_role() {
            var user = new IdentityUser();
            user.Id = TestData.John_UserId;

            var target = new UserStore<IdentityUser>();

            var userInRole = await target.IsInRoleAsync(user,"NoRole");

            Assert.That(userInRole, Is.False);
        }

        [Test]
        public async void Should_return_false_if_user_is_not_in_a_role() {
            var user = new IdentityUser();
            user.Id = TestData.Sue_UserId;

            var target = new UserStore<IdentityUser>();

            var userInRole = await target.IsInRoleAsync(user, "Admin");


            Assert.That(userInRole, Is.False);            
        }

        [Test]
        public async void Should_return_true_if_user_is_in_a_role() {
            var user = new IdentityUser();
            user.Id = TestData.John_UserId;
            
            var target = new UserStore<IdentityUser>();

            var userInRole = await target.IsInRoleAsync(user, "Admin");

            Assert.That(userInRole, Is.True);             
        }
    }
}