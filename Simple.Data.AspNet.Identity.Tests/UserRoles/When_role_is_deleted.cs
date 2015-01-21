using System;
using NUnit.Framework;

namespace Simple.Data.AspNet.Identity.Tests.UserRoles {
    [TestFixture]
    public class When_role_is_deleted 
    {
        [SetUp]
        public void SetUp() 
        {
            DatabaseHelper.Reset();
            TestData.AddRoles();
            TestData.AddUsers();
            TestData.AddRolesToUsers(); 
        }

        [Test]
        public void Should_throw_ObjectDisposedException_if_disposed()
        {
            var target = new UserStore<IdentityUser>();
            target.Dispose();
            Assert.Throws<ObjectDisposedException>(() => target.GetRolesAsync(new IdentityUser()));
        }

        [Test]
        public void Should_remove_user_role_record() 
        {
            var role = new IdentityRole("Admin");
            role.Id = TestData.Admin_RoleId;

            var target = new RoleStore<IdentityRole>();

            var task = target.DeleteAsync(role);

            task.Wait();

            dynamic db = Database.Open();

            var userRoles = db.AspNetUserRole.FindAllByRoleId(role.Id);
            Assert.That(userRoles, Is.Empty);            
        }
    }
}