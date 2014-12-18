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
            Assert.That(() => target.GetRolesAsync(null),
                Throws.Exception.TypeOf<ArgumentNullException>().With.Message.EqualTo("Value cannot be null.\r\nParameter name: user"));
        }


        [Test]
        public void Should_return_empty_list_if_user_has_no_roles() 
        {
            var user = new IdentityUser();
            user.Id = TestData.UserNoRoles_UserId;

            var target = new UserStore<IdentityUser>();

            var task = target.GetRolesAsync(user);

            task.Wait();

            Assert.That(task.Result, Is.Empty);
        }

        [Test]
        public void Should_return_roles_for_user_that_has_them() {
            var user = new IdentityUser();
            user.Id = TestData.John_UserId;

            var target = new UserStore<IdentityUser>();

            var task = target.GetRolesAsync(user);

            task.Wait();

            Assert.That(task.Result, Is.Not.Empty);
        }

        [Test]
        public void Should_return_expected_roles_for_user_that_has_them()
        {
            var user = new IdentityUser();
            user.Id = TestData.John_UserId;

            var target = new UserStore<IdentityUser>();

            var task = target.GetRolesAsync(user);

            task.Wait();

            var userRoles = task.Result;

            Assert.That(userRoles[0],Is.EqualTo("Admin") );
        }
    }
}