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
        }

        [Test]
        public void Should_create_UserStore_with_default_connection()
        {
            var target = new UserStore<IdentityUser>();
            var task = target.FindByIdAsync(TestData.John_UserId);
            var user = task.Result;
            Assert.That(user, Is.Not.Null);
            Assert.That(user.Id, Is.EqualTo(TestData.John_UserId));
        }

        [Test]
        public void Should_create_UserStore_with_named_connection()
        {
            var target = new UserStore<IdentityUser>("TestData");
            var task = target.FindByIdAsync(TestData.John_UserId);
            var user = task.Result;
            Assert.That(user, Is.Not.Null);
            Assert.That(user.Id, Is.EqualTo(TestData.John_UserId));
        }
    }
}