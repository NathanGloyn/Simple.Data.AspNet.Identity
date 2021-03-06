﻿using System;
using NUnit.Framework;

namespace Simple.Data.AspNet.Identity.Tests.Lockout
{
    [TestFixture]
    public class When_setting_lockout_date
    {

        [SetUp]
        public void SetUp()
        {
            DatabaseHelper.Reset();
            TestData.AddUsers();
        }

        [Test]
        public void Should_throw_ObjectDisposedException_if_disposed()
        {
            var target = new UserStore<IdentityUser>();
            target.Dispose();
            Assert.Throws<ObjectDisposedException>(() => target.SetLockoutEndDateAsync(new IdentityUser(), new DateTimeOffset()));
        }

        [Test]
        public void Should_throw_ArgumentNullException_if_user_is_null()
        {
            var target = new UserStore<IdentityUser>();            

            Assert.Throws<ArgumentNullException>(() => target.SetLockoutEndDateAsync(null, new DateTimeOffset()));
        }

        [Test]
        public void Should_set_the_lockout_date()
        {
            var target = new UserStore<IdentityUser>();

            var lockout = DateTimeOffset.UtcNow.AddMinutes(15);

            var task = target.SetLockoutEndDateAsync(TestData.GetTestUserJohn(), lockout);

            task.Wait();

            var db = Database.Open();
            IdentityUser record = db.AspNetUsers.FindAllByUserName("John").SingleOrDefault();

            Assert.That(record.LockoutEndDateUtc.Value.Date,Is.EqualTo(lockout.UtcDateTime.Date));
            Assert.That(record.LockoutEndDateUtc.Value.Hour, Is.EqualTo(lockout.UtcDateTime.Hour));
            Assert.That(record.LockoutEndDateUtc.Value.Minute, Is.EqualTo(lockout.UtcDateTime.Minute));
        }
    }
}