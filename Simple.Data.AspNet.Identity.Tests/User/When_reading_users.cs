using System;
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
            Assert.Throws<ObjectDisposedException>( async () => await _target.FindByNameAsync(""));
        }

        [Test]
        public void Should_throw_ObjectDisposedException_callaing_FindById_and_if_disposed()
        {
            _target.Dispose();
            Assert.Throws<ObjectDisposedException>(async () => await _target.FindByIdAsync(""));
        }

        [Test]
        public async void Should_return_null_for_unknown_user_id() {
            var identityUser = await _target.FindByIdAsync("8133EE09-58ED-4710-A572-2A77DA5E5562");

            Assert.That(identityUser, Is.Null);
        }
        
        [Test]
        public async void Should_find_user_by_Id() {
            var identityUser = await _target.FindByIdAsync("4455E2EB-B7F8-4C17-940B-199922298A02");


            Assert.That(identityUser.UserName, Is.EqualTo("John"));
            Assert.That(identityUser.Email, Is.EqualTo("John@test.com"));
        }

        [Test]
        public async void Should_return_null_for_unknown_user_name() 
        {
            var identityUser = await _target.FindByNameAsync("Jack");

            Assert.That(identityUser, Is.Null);
        }

        [Test]
        public async  void Should_find_user_by_name()
        {
            var identityUser = await _target.FindByNameAsync("Sue");

            Assert.That(identityUser.UserName, Is.EqualTo("Sue"));
            Assert.That(identityUser.Email, Is.EqualTo("Sue@test.com"));
        }
    }
}