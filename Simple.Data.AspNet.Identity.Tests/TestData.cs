using Microsoft.AspNet.Identity;

namespace Simple.Data.AspNet.Identity.Tests
{
    public class TestData
    {
        public const string John_UserId = "4455E2EB-B7F8-4C17-940B-199922298A02";
        public const string Admin_RoleId = "57384BB3-3D5F-4183-A03D-77408D8F225B";
        public const string Sue_UserId = "30222D63-8AD0-4A21-9B68-32ADC4FF3F45";
        public const string UserNoRoles_UserId = "03EAAF79-FCF0-4DB6-9D92-B53353A452F2";
        public const string UserNoPasswordHash_UserId = "F706973B-4E48-4353-862D-83E0D7EFE7FE";
        private const string User_RoleId = "259591EC-A59C-4C16-AD1E-1A24AB445463";
        

        private static readonly dynamic Db = Database.Open();

        public static void AddUsers()
        {
            Db.AspNetUsers.Insert(Id: John_UserId, UserName: "John", Email: "John@test.com", EmailConfirmed: false, PhoneNumberConfirmed: false, TwoFactorEnabled: false, LockoutEnabled: false, AccessFailedCount: 0, PasswordHash: "sa;ldfkjsldfjlajte");
            Db.AspNetUsers.Insert(Id: Sue_UserId, UserName: "Sue", Email: "Sue@test.com", EmailConfirmed: false, PhoneNumberConfirmed: false, TwoFactorEnabled: false, LockoutEnabled: false, AccessFailedCount: 0, PasswordHash: "0uptj0bqoweojf");
            Db.AspNetUsers.Insert(Id: UserNoRoles_UserId, UserName: "Fred", Email: "Fred@test.com", EmailConfirmed: false, PhoneNumberConfirmed: false, TwoFactorEnabled: false, LockoutEnabled: false, AccessFailedCount: 0);
            Db.AspNetUsers.Insert(Id: UserNoPasswordHash_UserId, UserName: "Jayne", Email: "jayne@test.com", EmailConfirmed: false, PhoneNumberConfirmed: false, TwoFactorEnabled: false, LockoutEnabled: false, AccessFailedCount: 0);
        }

        public static void AddRoles()
        {
            Db.AspNetRoles.Insert(Id: Admin_RoleId, Name: "Admin");
            Db.AspNetRoles.Insert(Id: User_RoleId, Name: "User");
        }

        public static void AddRolesToUsers()
        {
            Db.AspNetUserRole.Insert(UserId: John_UserId, RoleId: Admin_RoleId);
            Db.AspNetUserRole.Insert(UserId: Sue_UserId, RoleId: User_RoleId);
        }
    }
}