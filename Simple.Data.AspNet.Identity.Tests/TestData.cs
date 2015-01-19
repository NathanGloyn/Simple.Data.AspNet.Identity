using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNet.Identity;

namespace Simple.Data.AspNet.Identity.Tests
{
    public class TestData
    {
        public const string John_UserId = "4455E2EB-B7F8-4C17-940B-199922298A02";
        public const string Admin_RoleId = "57384BB3-3D5F-4183-A03D-77408D8F225B";
        public const string Sue_UserId = "30222D63-8AD0-4A21-9B68-32ADC4FF3F45";
        public const string LockedOut_UserId = "47421B7B-4E9C-43C8-B9B1-AF63C648B9E4";
        public const string UserNoRoles_UserId = "03EAAF79-FCF0-4DB6-9D92-B53353A452F2";
        public const string UserNoPasswordHash_UserId = "F706973B-4E48-4353-862D-83E0D7EFE7FE";
        private const string User_RoleId = "259591EC-A59C-4C16-AD1E-1A24AB445463";
        

        private static readonly dynamic Db = Database.Open();


        public static IdentityUser GetTestUserJohn()
        {
            return new IdentityUser{Id = John_UserId, UserName = "John", Email = "John@test.com", SecurityStamp = "securityStamp", EmailConfirmed = true, PhoneNumberConfirmed = true, PhoneNumber = "123-456", TwoFactorEnabled = true};
        }

        public static IdentityUser GetTestUserSue()
        {
            return new IdentityUser{Id = Sue_UserId, UserName = "Sue", Email = "Sue@test.com"};
        }

        public static IdentityUser GetTestUserLockedOut()
        {
            return new IdentityUser{Id = LockedOut_UserId, UserName = "Tony", Email = "Tony@test.com",AccessFailedCount = 5, LockoutEnabled = true};
        }

        public static void AddUsers(bool useCustomTables = false)
        {
            string tableName = useCustomTables ? "MyUsers" : "AspNetUsers";

            Db[tableName].Insert(Id: John_UserId, UserName: "John", Email: "John@test.com", EmailConfirmed: false, PhoneNumberConfirmed: false, TwoFactorEnabled: false, LockoutEnabled: false, AccessFailedCount: 0, PasswordHash: "sa;ldfkjsldfjlajte");
            Db[tableName].Insert(Id: Sue_UserId, UserName: "Sue", Email: "Sue@test.com", EmailConfirmed: false, PhoneNumberConfirmed: false, TwoFactorEnabled: false, LockoutEnabled: false, AccessFailedCount: 0, PasswordHash: "0uptj0bqoweojf");
            Db[tableName].Insert(Id: UserNoRoles_UserId, UserName: "Fred", Email: "Fred@test.com", EmailConfirmed: false, PhoneNumberConfirmed: false, TwoFactorEnabled: false, LockoutEnabled: false, AccessFailedCount: 0);
            Db[tableName].Insert(Id: UserNoPasswordHash_UserId, UserName: "Jayne", Email: "jayne@test.com", EmailConfirmed: false, PhoneNumberConfirmed: false, TwoFactorEnabled: false, LockoutEnabled: false, AccessFailedCount: 0);
            Db[tableName].Insert(Id: LockedOut_UserId, UserName: "Tony", Email: "Tony@test.com", EmailConfirmed: false, PhoneNumberConfirmed: false, TwoFactorEnabled: false, LockoutEnabled: true, AccessFailedCount: 5, PasswordHash: "a8weyraweghadh", LockoutEndDateUtc: DateTime.Now.AddHours(1));
        }

        public static void AddRoles(bool useCustomTables = false)
        {
            string tableName = useCustomTables ? "MyRoles" : "AspNetRoles";
            Db[tableName].Insert(Id: Admin_RoleId, Name: "Admin");
            Db[tableName].Insert(Id: User_RoleId, Name: "User");
        }

        public static void AddRolesToUsers(bool useCustomTables = false)
        {
            string tableName = useCustomTables ? "MyUserRole" : "AspNetUserRole";
            Db[tableName].Insert(UserId: John_UserId, RoleId: Admin_RoleId);
            Db[tableName].Insert(UserId: Sue_UserId, RoleId: User_RoleId);
        }

        public static void AddClaimsToUsers(bool useCustomTables = false)
        {
            string tableName = useCustomTables ? "MyUserClaims" : "AspNetUserClaims";
            Db[tableName].Insert(UserId: John_UserId, ClaimType: ClaimTypes.Email, ClaimValue: "John@test.com");
            Db[tableName].Insert(UserId: John_UserId, ClaimType: ClaimTypes.Country, ClaimValue: "UK");
        }

        public static void AdLoginsForUsers(bool useCustomTables = false)
        {
            string tableName = useCustomTables ? "MyUserLogins" : "AspNetUserLogins";

            Db[tableName].Insert(LoginProvider: "Google", ProviderKey: "123", UserId: John_UserId);
            Db[tableName].Insert(LoginProvider: "GitHub", ProviderKey: "abc", UserId: John_UserId);
            Db[tableName].Insert(LoginProvider: "Facebook", ProviderKey: "xyz-123", UserId: John_UserId);
        }
    }
}