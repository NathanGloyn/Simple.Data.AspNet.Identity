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
        public async void Should_read_user_from_MyUsers_table()
        {
            TestData.AddUsers(useCustomTables: true);
            var tables = new Tables().SetUsersTable("MyUsers");
            var target = new UserStore<IdentityUser>(tables);

            var identityUser = await target.FindByIdAsync(TestData.John_UserId);

            Assert.That(identityUser, Is.Not.Null);
            Assert.That(identityUser.UserName, Is.EqualTo("John"));
        }


        [Test]
        public async void Should_read_role_from_MyRoles_table()
        {
            TestData.AddRoles(useCustomTables: true);
            var tables = new Tables().SetRolesTable("MyRoles");
            var target = new RoleStore<IdentityRole>(tables);

            var identityRole = await target.FindByNameAsync("Admin");

            Assert.That(identityRole, Is.Not.Null);
            Assert.That(identityRole.Id, Is.EqualTo(TestData.Admin_RoleId));
        }

        [Test]
        public async void Should_read_userrole_from_MyUserRoles_table()
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

            var roles = await target.GetRolesAsync(user);

            var userRoles = roles;

            Assert.That(userRoles[0], Is.EqualTo("Admin"));            
        }

        [Test]
        public async void Should_insert_user_into_custom_user_table()
        {

            var target = new UserStore<IdentityUser>();
            target.Tables.SetUsersTable("MyUsers");

            var newUser = new IdentityUser("Kathy");

            await target.CreateAsync(newUser);

            var db = Database.Open();

            IdentityUser user = await db.MyUsers.FindAllByUserName("Kathy").FirstOrDefault();

            Assert.That(user, Is.Not.Null);
            Assert.That(user.UserName, Is.EqualTo("Kathy"));
        }

        [Test]
        public async void Should_update_user_in_MyUsers_table()
        {
            TestData.AddUsers(useCustomTables: true);

            var target = new UserStore<IdentityUser>();
            target.Tables.SetUsersTable("MyUsers");

            IdentityUser user =  await target.FindByIdAsync(TestData.John_UserId);

            user.PhoneNumber = "1234";
            user.PhoneNumberConfirmed = true;

            await target.UpdateAsync(user);

            var db = Database.Open();

            IdentityUser updatedUser = await db.MyUsers.FindAllById(TestData.John_UserId).First();

            Assert.That(updatedUser.PhoneNumber, Is.EqualTo("1234"));
            Assert.That(updatedUser.PhoneNumberConfirmed, Is.True);

        }
    }
}