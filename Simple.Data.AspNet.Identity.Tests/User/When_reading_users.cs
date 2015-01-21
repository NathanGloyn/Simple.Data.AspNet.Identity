using System;
using System.Linq;
using NUnit.Framework;

namespace Simple.Data.AspNet.Identity.Tests.User {
    [TestFixture]
    public class When_reading_users {


        private UserStore<IdentityUser> _target;

        [SetUp]
        public void SetUp() 
        {
            DatabaseHelper.Reset();
            TestData.AddUsers();
            _target = new UserStore<IdentityUser>();
        }

        [Test]
        public void Should_throw_ObjectDisposedException_calling_FindByBName_and_disposed()
        {
            _target.Dispose();
            Assert.Throws<ObjectDisposedException>(() => _target.FindByNameAsync(""));
        }

        [Test]
        public void Should_throw_ObjectDisposedException_callaing_FindById_and_if_disposed()
        {
            _target.Dispose();
            Assert.Throws<ObjectDisposedException>(() => _target.FindByIdAsync(""));
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
    }
}