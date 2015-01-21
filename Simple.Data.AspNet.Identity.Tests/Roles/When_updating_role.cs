using System;
using NUnit.Framework;

namespace Simple.Data.AspNet.Identity.Tests.Roles {
    [TestFixture]
    public class When_updating_role {
        private RoleStore<IdentityRole> _target;

        [SetUp]
        public void SetUp()
        {
            DatabaseHelper.Reset();
            _target = new RoleStore<IdentityRole>();
        }

        [Test]
        public void Should_throw_ArgumentNullException_if_role_is_null()
        {
            Assert.Throws<ArgumentNullException>(() => _target.UpdateAsync(null));
        }

        [Test]
        public void Should_throw_ObjectDisposedException_if_disposed()
        {
            _target.Dispose();
            Assert.Throws<ObjectDisposedException>(() => _target.UpdateAsync(new IdentityRole()));
        }

        [Test]
        public void Should_throw_ArgumentException_if_role_has_no_id()
        {
            var role = new IdentityRole();
            role.Id = "";

            Assert.That(
                () => _target.UpdateAsync(role),
                Throws.Exception.TypeOf<ArgumentException>().With.Message.EqualTo("Missing role Id"));

        }

        [Test]
        public void Should_throw_ArgumentException_if_role_has_no_name()
        {
            var role = new IdentityRole();

            Assert.That(
                () => _target.UpdateAsync(role),
                Throws.Exception.TypeOf<ArgumentException>().With.Message.EqualTo("Missing role Name"));
        }

        [Test]
        public void Should_update_role() {
            const string roleId = "57384BB3-3D5F-4183-A03D-77408D8F225B";

            dynamic db = Database.Open();

            db.AspNetRoles.Insert(Id: roleId, Name: "Admin");

            var role = new IdentityRole("Super Admin", roleId);

            var task = _target.UpdateAsync(role);

            task.Wait();

            var record = db.AspNetRoles.FindAllById(roleId).FirstOrDefault();

            Assert.That(record.Id, Is.EqualTo(roleId));
            Assert.That(record.Name, Is.EqualTo("Super Admin"));
        }
    }
}