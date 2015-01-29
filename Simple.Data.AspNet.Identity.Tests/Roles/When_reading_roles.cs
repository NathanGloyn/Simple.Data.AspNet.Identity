using System;
using System.Linq;
using NUnit.Framework;

namespace Simple.Data.AspNet.Identity.Tests.Roles {
    [TestFixture]
    public class When_reading_roles {
        private RoleStore<IdentityRole> _target;

        [SetUp]
        public void SetUp() {
            DatabaseHelper.Reset();
            TestData.AddRoles();
            _target = new RoleStore<IdentityRole>();
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
        public void Should_return_null_for_unknown_role()
        {
            var task = _target.FindByNameAsync("non-existent role");

            task.Wait();

            Assert.That(task.Result, Is.Null);
        }

        [Test]
        public void Should_return_null_for_unknown_role_id() 
        {
            var task = _target.FindByIdAsync("123");

            task.Wait();

            Assert.That(task.Result, Is.Null);
        }

        [Test]
        public void Should_return_role_based_on_role_name() {
            
            var task = _target.FindByNameAsync("Admin");

            task.Wait();

            Assert.That(task.Result.Id, Is.EqualTo("57384BB3-3D5F-4183-A03D-77408D8F225B"));            
        }

        [Test]
        public void Should_return_role_based_on_id() {

            var task = _target.FindByIdAsync("57384BB3-3D5F-4183-A03D-77408D8F225B");

            task.Wait();

            Assert.That(task.Result.Name, Is.EqualTo("Admin"));              
        }
    }
}