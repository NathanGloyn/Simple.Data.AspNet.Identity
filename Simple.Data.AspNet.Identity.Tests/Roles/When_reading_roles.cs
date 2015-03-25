using System;
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
            Assert.Throws<ObjectDisposedException>(async () => await _target.FindByNameAsync(""));
        }

        [Test]
        public void Should_throw_ObjectDisposedException_callaing_FindById_and_if_disposed()
        {
            _target.Dispose();
            Assert.Throws<ObjectDisposedException>(async () => await _target.FindByIdAsync(""));
        }

        [Test]
        public async void Should_return_null_for_unknown_role()
        {
            var identityRole = await _target.FindByNameAsync("non-existent role");

            Assert.That(identityRole, Is.Null);
        }

        [Test]
        public async void Should_return_null_for_unknown_role_id() 
        {
            var identityRole = await _target.FindByIdAsync("123");

            Assert.That(identityRole, Is.Null);
        }

        [Test]
        public async void Should_return_role_based_on_role_name() {
            
            var identityRole = await _target.FindByNameAsync("Admin");


            Assert.That(identityRole.Id, Is.EqualTo("57384BB3-3D5F-4183-A03D-77408D8F225B"));            
        }

        [Test]
        public async void Should_return_role_based_on_id() {

            var role = await _target.FindByIdAsync("57384BB3-3D5F-4183-A03D-77408D8F225B");

            Assert.That(role.Name, Is.EqualTo("Admin"));              
        }
    }
}