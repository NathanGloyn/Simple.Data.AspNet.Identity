﻿using System;
using NUnit.Framework;

namespace Simple.Data.AspNet.Identity.Tests.Lockout
{
    [TestFixture]
    public class When_checking_if_lockout_enabled
    {
        [SetUp]
        public void SetUp()
        {
            DatabaseHelper.Reset();
            TestData.AddUsers();
        }

        [Test]
        public void Should_throw_argument_null_exception_if_user_is_null()
        {
            var target = new UserStore<IdentityUser>();

            Assert.Throws<ArgumentNullException>(async ()=> await target.GetLockoutEnabledAsync(null));
        }

        [Test]
        public void Should_throw_ObjectDisposedException_if_disposed()
        {
            var target = new UserStore<IdentityUser>();
            target.Dispose();
            Assert.Throws<ObjectDisposedException>(async () => await target.GetLockoutEnabledAsync(new IdentityUser()));
        }

        [Test]
        public async void Should_return_lockout_value()
        {
            var target = new UserStore<IdentityUser>();

            var task = await target.GetLockoutEnabledAsync(TestData.GetTestUserLockedOut());

            Assert.That(task, Is.True);
        }
    }
}