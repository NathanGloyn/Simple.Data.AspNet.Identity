using System;
using NUnit.Framework;

namespace Simple.Data.AspNet.Identity.Tests.Roles {
    [TestFixture]
    public class When_creating_roles {
        private RoleStore<IdentityRole> _target;

        [SetUp]
        public void SetUp() {
            DatabaseHelper.Reset();
            _target = new RoleStore<IdentityRole>();
        }

        [Test]
        public void Should_throw_ArgumentNullException_if_role_is_null() 
        {
            Assert.Throws<ArgumentNullException>(() => _target.CreateAsync(null));
        }

        [Test]
        public void Should_throw_ArgumentException_if_role_has_no_id() 
        {
            var role = new IdentityRole();
            role.Id = "";

            Assert.That(
                () => _target.CreateAsync(role),
                Throws.Exception.TypeOf<ArgumentException>().With.Message.EqualTo("Missing role Id"));

        }

        [Test]
        public void Should_throw_ArgumentException_if_role_has_no_name()
        {
            var role = new IdentityRole();

            Assert.That(
                () => _target.CreateAsync(role),
                Throws.Exception.TypeOf<ArgumentException>().With.Message.EqualTo("Missing role Name"));
        }

        [Test]
        public void Should_add_role() 
        {    
            var task = _target.CreateAsync(new IdentityRole("Admin"));

            task.Wait();

            Assert.That(task.IsCompleted, Is.True);
        }

        [Test]
        public void Should_throw_exception_on_create_if_role_with_same_id_already_exists() {
            dynamic db = Database.Open();

            db.AspNetRoles.Insert(Id: "57384BB3-3D5F-4183-A03D-77408D8F225B", Name: "Admin");

            Assert.That(
                () => _target.CreateAsync(new IdentityRole("Admin", "57384BB3-3D5F-4183-A03D-77408D8F225B")),
                Throws.Exception.TypeOf<Simple.Data.Ado.AdoAdapterException>()
                    .With.Message.Contains("Violation of PRIMARY KEY constraint 'PK_dbo.AspNetRole'. Cannot insert duplicate key in object 'dbo.AspNetRoles'. The duplicate key value is (57384BB3-3D5F-4183-A03D-77408D8F225B)"));

        }

    }
}