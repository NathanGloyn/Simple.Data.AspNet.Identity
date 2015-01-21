using System;
using NUnit.Framework;

namespace Simple.Data.AspNet.Identity.Tests.UserRoles {
    [TestFixture]
    public class When_adding_role_for_a_user {

        private UserStore<IdentityUser> _target;

        [SetUp]
        public void SetUp() 
        {
            DatabaseHelper.Reset();
            TestData.AddRoles();
            TestData.AddUsers();
            _target = new UserStore<IdentityUser>();    
        }

        [Test]
        public void Should_throw_ArgumentNullException_if_user_is_null() {
            Assert.That(() => _target.AddToRoleAsync(null,null), Throws.TypeOf<ArgumentNullException>().With.Message.EqualTo("Value cannot be null.\r\nParameter name: user"));
        }

        [Test]
        public void Should_throw_ObjectDisposedException_if_disposed()
        {
            _target.Dispose();
            Assert.Throws<ObjectDisposedException>(() => _target.AddToRoleAsync(new IdentityUser(),""));
        }

        [Test]
        public void Should_throw_ArgumentException_if_roleName_is_null([Values(null, "", " ")] string roleName) 
        {
            var user = new IdentityUser();

            Assert.That(() => _target.AddToRoleAsync(user, roleName), Throws.ArgumentException.With.Message.EqualTo("Argument cannot be null or empty: roleName."));    
        }

        [Test]
        public void Should_throw_ArgumentException_if_roleName_is_not_an_actual_role() 
        {
            var user = new IdentityUser();

            Assert.That(() => _target.AddToRoleAsync(user, "SuperUser"), Throws.ArgumentException.With.Message.EqualTo("Unknown role: SuperUser"));            
        }


        [Test]
        public void Should_throw_Exception_if_user_id_is_not_in_the_database() {

            var user = new IdentityUser();
            user.Id = "CB382B36-11BC-45F9-999B-62FC41CF8A9A";

            Assert.That(
                () => _target.AddToRoleAsync(user, "Admin"),
                Throws.TypeOf<Simple.Data.Ado.AdoAdapterException>()
                    .With.Message.EqualTo(
                        "The INSERT statement conflicted with the FOREIGN KEY constraint \"FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId\". The conflict occurred in database \"IdentityTest\", table \"dbo.AspNetUsers\", column 'Id'.\r\nThe statement has been terminated."));
        }

        [Test]
        public void Should_add_role_for_user() {
            var user = new IdentityUser();
            user.Id = TestData.John_UserId;

            var task = _target.AddToRoleAsync(user, "Admin");

            task.Wait();

            Assert.That(task.IsCompleted,Is.True);

            var db = Database.Open();
            var userRole = db.AspNetUserRole.FindAllByUserId(TestData.John_UserId).FirstOrDefault();
            
            Assert.That(userRole, Is.Not.Null);
            Assert.That(userRole.RoleId, Is.EqualTo(TestData.Admin_RoleId));

        }
    }
}