using NUnit.Framework;

namespace Simple.Data.AspNet.Identity.Tests
{
    [TestFixture]
    public class When_using_different_connection_configuration
    {
        [SetUp]
        public void SetUp()
        {
            DatabaseHelper.Reset();
            TestData.AddUsers();
            TestData.AddRoles();
        }

        [Test]
        public void Should_create_UserStore_with_default_connection()
        {
            var target = new UserStore<IdentityUser>();
            var task = target.FindByIdAsync(TestData.John_UserId);
            task.Wait();

            var user = task.Result;
            Assert.That(user, Is.Not.Null);
            Assert.That(user.Id, Is.EqualTo(TestData.John_UserId));
        }

        [Test]
        public void Should_create_UserStore_with_named_connection()
        {
            var target = new UserStore<IdentityUser>("TestDatabase");
            var task = target.FindByIdAsync(TestData.John_UserId);
            task.Wait();

            var user = task.Result;
            Assert.That(user, Is.Not.Null);
            Assert.That(user.Id, Is.EqualTo(TestData.John_UserId));
        }

        [Test]
        public void Should_create_RoleStore_with_default_connection()
        {
            var target = new RoleStore<IdentityRole>();
            var task = target.FindByIdAsync(TestData.Admin_RoleId);
            task.Wait();

            var role = task.Result;
            Assert.That(role, Is.Not.Null);
            Assert.That(role.Id, Is.EqualTo(TestData.Admin_RoleId));
        }

        [Test]
        public void Should_create_RoleStore_with_named_connection()
        {
            var target = new RoleStore<IdentityRole>("TestDatabase");
            var task = target.FindByIdAsync(TestData.Admin_RoleId);
            task.Wait();

            var role = task.Result;
            Assert.That(role, Is.Not.Null);
            Assert.That(role.Id, Is.EqualTo(TestData.Admin_RoleId));
        }

    }
}