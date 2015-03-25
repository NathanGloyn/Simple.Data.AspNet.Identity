using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Simple.Data.AspNet.Identity.Tests.UserRoles {
    [TestFixture]
    public class When_getting_user_roles {
        [SetUp]
        public void SetUp()
        {
            DatabaseHelper.Reset();
            TestData.AddRoles();
            TestData.AddUsers();
            TestData.AddRolesToUsers();
        }

        [Test]
        public void Should_throw_argument_null_exception_if_user_is_null()
        {
            var target = new UserStore<IdentityUser>();
            Assert.That(async () => await target.GetRolesAsync(null),
                Throws.Exception.TypeOf<ArgumentNullException>().With.Message.EqualTo("Value cannot be null.\r\nParameter name: user"));
        }

        [Test]
        public void Should_throw_ObjectDisposedException_if_disposed()
        {
            var target = new UserStore<IdentityUser>();
            target.Dispose();
            Assert.Throws<ObjectDisposedException>(async () => await target.GetRolesAsync(new IdentityUser()));
        }

        [Test]
        public async void Should_return_empty_list_if_user_has_no_roles() 
        {
            var user = new IdentityUser();
            user.Id = TestData.UserNoRoles_UserId;

            var target = new UserStore<IdentityUser>();

            var roles = await target.GetRolesAsync(user);


            Assert.That(roles, Is.Empty);
        }

        [Test]
        public async void Should_return_roles_for_user_that_has_them() {
            var user = new IdentityUser();
            user.Id = TestData.John_UserId;

            var target = new UserStore<IdentityUser>();

            var roles = await target.GetRolesAsync(user);

            Assert.That(roles, Is.Not.Empty);
        }

        [Test]
        public async void Should_return_expected_roles_for_user_that_has_them()
        {
            var user = new IdentityUser();
            user.Id = TestData.John_UserId;

            var target = new UserStore<IdentityUser>();

            var roles = await target.GetRolesAsync(user);

            var userRoles = roles;

            Assert.That(userRoles[0],Is.EqualTo("Admin") );
        }
    }
}