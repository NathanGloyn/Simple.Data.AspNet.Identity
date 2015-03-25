using System;
using NUnit.Framework;

namespace Simple.Data.AspNet.Identity.Tests.UserRoles {
    [TestFixture]
    public class When_removing_a_role_from_a_user {
        private UserStore<IdentityUser> _target;

        [SetUp]
        public void SetUp() 
        {
            DatabaseHelper.Reset();
            TestData.AddUsers();
            TestData.AddRoles();
            TestData.AddRolesToUsers();
            _target = new UserStore<IdentityUser>();
        }

        [Test]
        public void Should_throw_ArgumentNullException_if_user_is_null()
        {
            Assert.That(async () => await _target.RemoveFromRoleAsync(null,"Admin"), Throws.TypeOf<ArgumentNullException>().With.Message.EqualTo("Value cannot be null.\r\nParameter name: user"));
        }

        [Test]
        public void Should_throw_ObjectDisposedException_if_disposed()
        {
            _target.Dispose();
            Assert.Throws<ObjectDisposedException>(async () => await _target.RemoveFromRoleAsync(new IdentityUser(),""));
        }

        [Test]
        public void Should_throw_ArgumentException_if_roleName_is_null([Values(null, "", " ")] string roleName)
        {
            var user = new IdentityUser();

            Assert.That(async () => await _target.RemoveFromRoleAsync(user, roleName), Throws.ArgumentException.With.Message.EqualTo("Argument cannot be null or empty: roleName."));
        }

        [Test]
        public void Should_throw_ArgumentException_if_roleName_is_not_an_actual_role()
        {
            var user = new IdentityUser();

            Assert.That(async () => await _target.RemoveFromRoleAsync(user, "SuperUser"), Throws.ArgumentException.With.Message.EqualTo("Unknown role: SuperUser"));
        }

        [Test]
        public async void Should_remove_specific_role_from_the_user() {
            var user = new IdentityUser();
            user.Id = TestData.John_UserId;

            await _target.RemoveFromRoleAsync(user, "Admin");

            dynamic db = Database.Open();

            var found = await db.AspNetUserRole.FindAllByUserId(TestData.John_UserId);

            Assert.That(found, Is.Empty);
        }
    }
}