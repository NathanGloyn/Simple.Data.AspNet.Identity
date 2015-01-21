using System;
using NUnit.Framework;

namespace Simple.Data.AspNet.Identity.Tests.Roles {
    [TestFixture]
    public class When_deleting_roles {

        private RoleStore<IdentityRole> _target;

        [SetUp]
        public void SetUp() 
        {
            DatabaseHelper.Reset();   
            TestData.AddRoles();
            _target = new RoleStore<IdentityRole>();
        }

        [Test]
        public void Should_throw_ArgumentNullException_if_role_is_null() {
            Assert.Throws<ArgumentNullException>(() => _target.DeleteAsync(null));
        }

        [Test]
        public void Should_throw_ObjectDisposedException_if_disposed()
        {
            _target.Dispose();
            Assert.Throws<ObjectDisposedException>(() => _target.DeleteAsync(new IdentityRole()));
        }


        [Test]
        public void Should_throw_ArgumentException_if_role_has_no_id()
        {
            var role = new IdentityRole();
            role.Id = "";

            Assert.That(
                () => _target.DeleteAsync(role),
                Throws.Exception.TypeOf<ArgumentException>().With.Message.EqualTo("Missing role Id"));

        }

        [Test]
        public void Should_not_throw_an_exception_trying_to_delete_a_nonexistent_record() {
            var role = new IdentityRole("Missing","B8600730-EE6D-4EA1-9A6A-7A817866DD67");

             var task = _target.DeleteAsync(role);

             task.Wait();

             Assert.That(task.IsCompleted, Is.True);
        }
    }
}