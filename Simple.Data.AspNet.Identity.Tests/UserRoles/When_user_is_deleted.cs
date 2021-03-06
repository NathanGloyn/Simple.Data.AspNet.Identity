﻿using System;
using NUnit.Framework;

namespace Simple.Data.AspNet.Identity.Tests.UserRoles {
    [TestFixture]
    public class When_user_is_deleted 
    {

        [SetUp]
        public void SetUp()
        {
            DatabaseHelper.Reset();
            TestData.AddRoles();
            TestData.AddUsers();
            TestData.AddRolesToUsers();
        }

        [Test]
        public void Should_throw_ObjectDisposedException_if_disposed()
        {
            var target = new UserStore<IdentityUser>();
            target.Dispose();
            Assert.Throws<ObjectDisposedException>(() => target.GetRolesAsync(new IdentityUser()));
        }

        [Test]
        public void Should_remove_user_role_record() 
        {
            var user = new IdentityUser();
            user.Id = TestData.John_UserId;

            var target = new UserStore<IdentityUser>();

            var task = target.DeleteAsync(user);

            task.Wait();

            dynamic db = Database.Open();

            var userRoles = db.AspNetUserRole.FindAllByUserId(user.Id);
            Assert.That(userRoles, Is.Empty);
        }
    }
}