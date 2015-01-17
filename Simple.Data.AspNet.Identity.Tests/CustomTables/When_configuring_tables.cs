using NUnit.Framework;

namespace Simple.Data.AspNet.Identity.Tests.CustomTables
{
    [TestFixture]
    public class When_configuring_tables
    {
        private const string MyUsers = "MyUsers";
        private const string MyRoles = "MyRoles";
        private const string MyUserRoles = "MyUserRoles";
        private const string MyUserClains = "MyUserClaims";
        private const string MyUserLogins = "MyUserLogins";

        private Tables _target;

        [SetUp]
        public void SetUp()
        {
            _target = new Tables();
        }

        [Test]
        public void Should_have_default_user_table_if_not_set()
        {
            Assert.That(_target.Users, Is.EqualTo("AspNetUsers"));
        }

        [Test]
        public void Should_have_default_roles_table_if_not_set()
        {
            Assert.That(_target.Roles, Is.EqualTo("AspNetRoles"));
        }

        [Test]
        public void Should_have_default_userroles_table_if_not_set()
        {
            Assert.That(_target.UsersRoles, Is.EqualTo("AspNetUserRoles"));
        }

        [Test]
        public void Should_set_users_table()
        {
            _target.SetUsersTable(MyUsers);
            Assert.That(_target.Users, Is.EqualTo(MyUsers));
        }

        [Test]
        public void Should_set_roles_table()
        {
            _target.SetRolesTable(MyRoles);
            Assert.That(_target.Roles, Is.EqualTo(MyRoles));
        }

        [Test]
        public void Should_set_userroles_table()
        {
            _target.SetUserRolesTable(MyUserRoles);
            Assert.That(_target.UsersRoles, Is.EqualTo(MyUserRoles));
        }

        [Test]
        public void Should_set_userclaims_table()
        {
            _target.SetUserClaimsTable(MyUserClains);
            Assert.That(_target.UsersClaims, Is.EqualTo(MyUserClains));
        }

        [Test]
        public void Should_set__userLogins_table()
        {
            _target.SetUserLoginsTable(MyUserLogins);
            Assert.That(_target.UsersLogins, Is.EqualTo(MyUserLogins));
        }

        [Test]
        public void Should_be_able_to_set_tables_fluently()
        {
            _target.SetUsersTable(MyUsers)
                   .SetRolesTable(MyRoles)
                   .SetUserRolesTable(MyUserRoles)
                   .SetUserClaimsTable(MyUserClains)
                   .SetUserLoginsTable(MyUserLogins);

            Assert.That(_target.Users, Is.EqualTo(MyUsers));
            Assert.That(_target.Roles, Is.EqualTo(MyRoles));
            Assert.That(_target.UsersRoles, Is.EqualTo(MyUserRoles));
            Assert.That(_target.UsersClaims, Is.EqualTo(MyUserClains));
            Assert.That(_target.UsersLogins, Is.EqualTo(MyUserLogins));
        }
    }
}
