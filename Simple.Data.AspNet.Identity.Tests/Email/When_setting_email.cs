﻿using System;
using NUnit.Framework;

namespace Simple.Data.AspNet.Identity.Tests.Email
{
    [TestFixture]
    public class When_setting_email
    {

        [SetUp]
        public void SetUp()
        {
            DatabaseHelper.Reset();
            TestData.AddUsers();
        }

        [Test]
        public void Should_throw_ObjectDisposedException_calling_FindByBName_and_disposed()
        {
            var target = new UserStore<IdentityUser>();
            target.Dispose();
            Assert.Throws<ObjectDisposedException>(async () => await target.SetEmailAsync(new IdentityUser(), ""));
        }

        [Test]
        public void Should_throw_ArgumentNullException_if_user_is_null()
        {
            var target = new UserStore<IdentityUser>();

            Assert.Throws<ArgumentNullException>(async () => await target.SetEmailAsync(null, ""));
        }

        [Test]
        public async void Should_set_email()
        {
            var target = new UserStore<IdentityUser>();

            await target.SetEmailAsync(TestData.GetTestUserJohn(), "John.Boy@test.com");

            var db = Database.Open();
            IdentityUser userDetails = await db.AspNetUsers.FindAllById(TestData.John_UserId).FirstOrDefault();

            Assert.That(userDetails.Email, Is.EqualTo("John.Boy@test.com"));

        }
    }
}