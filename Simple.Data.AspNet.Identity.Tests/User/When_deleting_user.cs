using System;
using NUnit.Framework;

namespace Simple.Data.AspNet.Identity.Tests.User {
    [TestFixture]
    public class When_deleting_user {

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
            Assert.Throws<ArgumentNullException>(() => _target.DeleteAsync(null));            
        }

        [Test]
        public void Should_throw_ArgumentException_if_user_id_is_missing() {
            var user = new IdentityUser();
            user.Id = null;

            Assert.That(
                () => _target.DeleteAsync(user),
                Throws.Exception.TypeOf<ArgumentException>().With.Message.EqualTo("Missing user Id"));
        }

        [Test]
        public void Should_not_throw_exception_if_trying_to_delete_a_non_existant_record() {
            var user = new IdentityUser("Missing");

            var task = _target.DeleteAsync(user);

            task.Wait();

            Assert.That(task.IsCompleted, Is.True);
        }

        [Test]
        public void Should_delete_user() {
            var userId = Guid.NewGuid().ToString();

            var user = TestData.GetTestUserJohn();             

            dynamic db = Database.Open();
            db.AspNetUsers.Insert(user);

            var task = _target.DeleteAsync(user);

            task.Wait();

            Assert.That(db.AspNetUsers.FindAllById(userId).SingleOrDefault(), Is.Null);
        }

    }
}