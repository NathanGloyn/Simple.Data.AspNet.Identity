using System;
using NUnit.Framework;

namespace Simple.Data.AspNet.Identity.Tests.User {
    [TestFixture]
    public class When_updating_user {

        UserStore<IdentityUser> _target;

        [SetUp]
        public void SetUp()
        {
            DatabaseHelper.Reset();
            TestData.AddUsers();
            _target = new UserStore<IdentityUser>();
        }

        [Test]
        public void Should_throw_ArgumentNullException_if_user_is_null() {
            Assert.Throws<ArgumentNullException>(() => _target.UpdateAsync(null));
        }

        [Test]
        public void Should_throw_ObjectDisposedException_if_disposed()
        {
            _target.Dispose();
            Assert.Throws<ObjectDisposedException>(() => _target.UpdateAsync(new IdentityUser()));
        }

        [Test]
        public void Should_throw_ArgumentException_if_missing_user_id() {
            var user = new IdentityUser();
            user.Id = "";

            Assert.That(
                () => _target.UpdateAsync(user),
                Throws.Exception.TypeOf<ArgumentException>().With.Message.EqualTo("Missing Id\r\nParameter name: user"));
        }

        [Test]
        public async void Should_update_user() {
            dynamic db = Database.Open();
            
            var user = new IdentityUser("Jack");
            user.Id = "D0F971C4-123B-4736-97EC-8F3B57D038AB";
            user.Email = "Jack@test.com";
            
            await db.AspNetUsers.Insert(user);

            user.PhoneNumber = "0800 12345678";

            await _target.UpdateAsync(user);


            IdentityUser updatedUser = await db.AspNetUsers.FindAllById("D0F971C4-123B-4736-97EC-8F3B57D038AB").FirstOrDefault();

            Assert.That(updatedUser.PhoneNumber, Is.EqualTo("0800 12345678"));

        }
    }
}