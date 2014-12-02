﻿using System.Linq;
using NUnit.Framework;

namespace Simple.Data.AspNet.Identity.Tests.User {
    [TestFixture]
    public class When_reading_users {


        private UserStore<IdentityUser> _target;

        [SetUp]
        public void SetUp() 
        {
            DatabaseHelper.Reset();
            AddUsers();
            _target = new UserStore<IdentityUser>();
        }

        [Test]
        public void Should_return_null_for_unknown_user_id() {
            var task = _target.FindByIdAsync("8133EE09-58ED-4710-A572-2A77DA5E5562");

            task.Wait();

            Assert.That(task.Result, Is.Null);
        }
        
        [Test]
        public void Should_find_user_by_Id() {
            var task = _target.FindByIdAsync("4455E2EB-B7F8-4C17-940B-199922298A02");

            task.Wait();

            Assert.That(task.Result.UserName, Is.EqualTo("John"));
            Assert.That(task.Result.Email, Is.EqualTo("John@test.com"));
        }

        [Test]
        public void Should_return_null_for_unknown_user_name() 
        {
            var task = _target.FindByNameAsync("Jack");

            task.Wait();

            Assert.That(task.Result, Is.Null);
        }

        [Test]
        public void Should_find_user_by_name()
        {
            var task = _target.FindByNameAsync("Sue");

            task.Wait();

            Assert.That(task.Result.UserName, Is.EqualTo("Sue"));
            Assert.That(task.Result.Email, Is.EqualTo("Sue@test.com"));
        }

        [Test]
        public void Should_be_able_to_query_users() {
            var user = _target.Users.SingleOrDefault(u => u.UserName == "Sue");

            Assert.That(user, Is.Not.Null);
            Assert.That(user.UserName, Is.EqualTo("Sue"));
        }

        private void AddUsers() 
        {
            dynamic db = Database.Open();

            db.AspNetUsers.Insert(Id: "4455E2EB-B7F8-4C17-940B-199922298A02", UserName: "John", Email: "John@test.com", EmailConfirmed: false, PhoneNumberConfirmed: false, TwoFactorEnabled: false, LockoutEnabled: false, AccessFailedCount: 0);
            db.AspNetUsers.Insert(Id: "30222D63-8AD0-4A21-9B68-32ADC4FF3F45", UserName: "Sue", Email: "Sue@test.com", EmailConfirmed: false, PhoneNumberConfirmed: false, TwoFactorEnabled: false, LockoutEnabled: false, AccessFailedCount: 0);
        }
    }
}