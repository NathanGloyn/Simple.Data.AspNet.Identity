using System;
using NUnit.Framework;

namespace Simple.Data.AspNet.Identity.Tests.User {
    [TestFixture]
    public class When_creating_users {

        UserStore<IdentityUser> _target;

        [SetUp]
        public void SetUp() 
        {
            DatabaseHelper.Reset();    
            _target = new UserStore<IdentityUser>();
        }

        [Test]
        public void Should_throw_ArgumentNullException_if_user_is_null() 
        {
            Assert.Throws<ArgumentNullException>(() => _target.CreateAsync(null));
        }

        [Test]
        public void Should_insert_user_with_just_UserName() {

            var user = new IdentityUser("John");
            var task = _target.CreateAsync(user);

            task.Wait();

            Assert.That(task.IsCompleted, Is.True);

            var testUser = GetTestUser();

            Assert.That(testUser, Is.Not.Null);
            Assert.That(testUser.UserName, Is.EqualTo("John"));
        }


        [Test]
        public void Should_insert_user_with_fields_to_create_user_populated() {
            var userId = Guid.NewGuid().ToString();
            var user = new IdentityUser {
                Id = userId,
                UserName = "John",
                Email = "John@test.com",
                EmailConfirmed = true,
                PhoneNumber = "0800 900788",
                PhoneNumberConfirmed = true
            };

            var task = _target.CreateAsync(user);

            task.Wait();

            var testUser = GetTestUser();

            Assert.That(testUser.Id, Is.EqualTo(userId));
            Assert.That(testUser.UserName, Is.EqualTo("John"));
            Assert.That(testUser.Email, Is.EqualTo("John@test.com"));
            Assert.That(testUser.EmailConfirmed, Is.True);
            Assert.That(testUser.PhoneNumber, Is.EqualTo("0800 900788"));
            Assert.That(testUser.PhoneNumberConfirmed, Is.True);
        }


        private IdentityUser GetTestUser()
        {
            dynamic db = Database.Open();
            return db.AspNetUsers.FindAllByUserName("John").SingleOrDefault();
        }
    }
}