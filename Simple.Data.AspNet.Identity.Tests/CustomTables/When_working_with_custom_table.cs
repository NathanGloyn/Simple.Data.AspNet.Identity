using NUnit.Framework;

namespace Simple.Data.AspNet.Identity.Tests.CustomTables
{
    [TestFixture]
    public class When_working_with_custom_table
    {
        [SetUp]
        public void SetUp()
        {
            DatabaseHelper.Reset();

        }

        [Test]
        public void Should_read_user_from_MyUsers_table()
        {
            TestData.AddUsers(useCustomTables: true);
            var tables = new Tables().SetUsersTable("MyUsers");
            var target = new UserStore<IdentityUser>(tables);

            var task = target.FindByIdAsync(TestData.John_UserId);

            task.Wait();

            Assert.That(task.Result, Is.Not.Null);
            Assert.That(task.Result.UserName, Is.EqualTo("John"));
        }


        [Test]
        public void Should_read_role_from_MyRoles_table()
        {
            TestData.AddRoles(useCustomTables: true);
            var tables = new Tables().SetRolesTable("MyRoles");
            var target = new RoleStore<IdentityRole>(tables);

            var task = target.FindByNameAsync("Admin");

            task.Wait();

            Assert.That(task.Result, Is.Not.Null);
            Assert.That(task.Result.Id, Is.EqualTo(TestData.Admin_RoleId));
        }

        [Test]
        public void Should_read_userrole_from_MyUserRoles_table()
        {
            TestData.AddUsers(useCustomTables: true);
            TestData.AddRoles(useCustomTables: true);
            TestData.AddRolesToUsers(useCustomTables: true);
            var tables = new Tables();
            tables.SetUsersTable("MyUsers")
                  .SetRolesTable("MyRoles")
                  .SetUserRolesTable("MyUserRoles");

            var user = TestData.GetTestUserJohn();

            var target = new UserStore<IdentityUser>(tables);

            var task = target.GetRolesAsync(user);

            task.Wait();

            var userRoles = task.Result;

            Assert.That(userRoles[0], Is.EqualTo("Admin"));            
        }

        //[Test]
        //public void Should_insert_user_into_custom_user_table()
        //{
        //    var newUser = new IdentityUser("Kathy");

        //    var task = _target.CreateAsync(newUser);

        //    task.Wait();

        //    var db = Database.Open();

        //    var user = (IdentityUser) db.MyUsers.FindAllByUserName("Kathy").FirstOrDefault();

        //    Assert.That(user, Is.Not.Null);
        //    Assert.That(user.UserName, Is.EqualTo("Kathy"));
        //}

        //[Test]
        //public void Should_update_user_in_MyUsers_table()
        //{
        //    var task = _target.FindByIdAsync(TestData.John_UserId);

        //    task.Wait();

        //    var user = task.Result;

        //    user.PhoneNumber = "1234";
        //    user.PhoneNumberConfirmed = true;

        //    var updateTask = _target.UpdateAsync(user);

        //    updateTask.Wait();

        //    var db = Database.Open();

        //    var updatedUser = (IdentityUser) db.MyUsers.FindAllById(TestData.John_UserId).First();

        //    Assert.That(updatedUser.PhoneNumber, Is.EqualTo("1234"));
        //    Assert.That(updatedUser.PhoneNumberConfirmed, Is.True);

        //}
    }
}