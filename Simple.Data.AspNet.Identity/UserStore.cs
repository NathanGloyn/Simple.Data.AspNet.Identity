using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace Simple.Data.AspNet.Identity {
    public class UserStore<TUser> : IQueryableUserStore<TUser>, IUserStore<TUser>, IUserRoleStore<TUser>,
        IUserPasswordStore<TUser>, IUserClaimStore<TUser>, IUserLockoutStore<TUser, string>, IUserLoginStore<TUser>,
        IUserSecurityStampStore<TUser>, IUserEmailStore<TUser>, IUserPhoneNumberStore<TUser>, IUserTwoFactorStore<TUser,string> where TUser : IdentityUser
    {
        private readonly Storage _storage;

        public Tables Tables { get; set; }


        public UserStore():this(null, null) { }

        public UserStore(Tables tables):this(null, tables) { }

        public UserStore(string connectionName):this(connectionName, null) { }

        public UserStore(string connectionName, Tables tables)
        {
            Tables = tables ?? new Tables();
            _storage = connectionName == null ? new Storage(Tables) : new Storage(connectionName, Tables);
        }

        public void Dispose()
        {
            // we can let normal GC behaviour to clean up
        }

        public Task CreateAsync(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            _storage.UsersTable.Insert(user);

            return Task.FromResult<object>(null);
        }

        public Task UpdateAsync(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (string.IsNullOrWhiteSpace(user.Id))
            {
                throw new ArgumentException("Missing Id", "user");
            }

            _storage.UsersTable.Update(user);

            return Task.FromResult<object>(null);
        }

        public Task DeleteAsync(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (string.IsNullOrWhiteSpace(user.Id))
            {
                throw new ArgumentException("Missing user Id");
            }

            _storage.UsersTable.Delete(user.Id);

            return Task.FromResult<Object>(null);
        }

        public Task<TUser> FindByIdAsync(string userId)
        {
            var result = _storage.UsersTable.GetUserById(userId) as TUser;

            return Task.FromResult(result);
        }

        public Task<TUser> FindByNameAsync(string userName)
        {
            var result = _storage.UsersTable.GetUserByName(userName) as TUser;

            return Task.FromResult(result);
        }

        public IQueryable<TUser> Users
        {
            get { return _storage.UsersTable.AllUsers<TUser>().AsQueryable(); }
        }

        public Task AddToRoleAsync(TUser user, string roleName)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (string.IsNullOrWhiteSpace(roleName))
            {
                throw new ArgumentException("Argument cannot be null or empty: roleName.");
            }

            string roleId = _storage.RolesTable.GetRoleId(roleName);

            if (string.IsNullOrEmpty(roleId))
            {
                throw new ArgumentException("Unknown role: " + roleName);
            }

            if (!string.IsNullOrEmpty(roleId))
            {
                _storage.UserRolesTable.Insert(user, roleId);
            }

            return Task.FromResult<object>(null);
        }

        public Task RemoveFromRoleAsync(TUser user, string roleName)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (string.IsNullOrWhiteSpace(roleName))
            {
                throw new ArgumentException("Argument cannot be null or empty: roleName.");
            }

            string roleId = _storage.RolesTable.GetRoleId(roleName);

            if (string.IsNullOrEmpty(roleId))
            {
                throw new ArgumentException("Unknown role: " + roleName);
            }

            if (!string.IsNullOrEmpty(roleId))
            {
                _storage.UserRolesTable.Delete(user, roleId);
            }

            return Task.FromResult<object>(null);
        }

        public Task<IList<string>> GetRolesAsync(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            IList<string> roleNames = _storage.UserRolesTable.FindByUserId(user.Id)
                .Select(role => role.Name)
                .ToList();

            return Task.FromResult(roleNames);
        }

        public Task<bool> IsInRoleAsync(TUser user, string roleName)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (string.IsNullOrWhiteSpace(roleName))
            {
                throw new ArgumentNullException("roleName");
            }

            var userRoles = _storage.UserRolesTable.FindByUserId(user.Id);

            return Task.FromResult(userRoles.Any(x => x.Name == roleName));

        }

        public Task SetPasswordHashAsync(TUser user, string passwordHash)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (string.IsNullOrWhiteSpace(passwordHash))
            {
                throw new ArgumentException("Invalid password hash value", "passwordHash");
            }

            user.PasswordHash = passwordHash;

            _storage.UsersTable.Update(user);

            return Task.FromResult<object>(null);
        }

        public Task<string> GetPasswordHashAsync(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            string passwordHash = _storage.UsersTable.GetPasswordHash(user);

            return Task.FromResult(passwordHash);
        }

        public Task<bool> HasPasswordAsync(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            var result = !string.IsNullOrEmpty(_storage.UsersTable.GetPasswordHash(user));

            return Task.FromResult(result);
        }

        public Task<IList<Claim>> GetClaimsAsync(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            var result = _storage.UserClaimsTable.FindByUserId(user.Id);

            return Task.FromResult<IList<Claim>>(result.Claims.ToList());
        }

        public Task AddClaimAsync(TUser user, Claim claim)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (claim == null)
            {
                throw new ArgumentNullException("claim");
            }

            var userClaim = new IdentityClaim(user.Id, claim);
            _storage.UserClaimsTable.AddClaim(userClaim);

            return Task.FromResult<object>(null);
        }

        public Task RemoveClaimAsync(TUser user, Claim claim)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (claim == null)
            {
                throw new ArgumentNullException("claim");
            }

            var userClaim = new IdentityClaim(user.Id, claim);

            _storage.UserClaimsTable.RemoveClaim(userClaim);

            return Task.FromResult<object>(null);
        }

        public Task<DateTimeOffset> GetLockoutEndDateAsync(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            var userDetail = _storage.UsersTable.GetUserById(user.Id);

            return
                Task.FromResult(
                    userDetail.LockoutEndDateUtc.HasValue
                        ? new DateTimeOffset(DateTime.SpecifyKind(userDetail.LockoutEndDateUtc.Value, DateTimeKind.Utc))
                        : new DateTimeOffset());
        }

        public Task SetLockoutEndDateAsync(TUser user, DateTimeOffset lockoutEnd)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            user.LockoutEndDateUtc = lockoutEnd.UtcDateTime;

            _storage.UsersTable.Update(user);

            return Task.FromResult<int>(0);
        }

        public Task<int> IncrementAccessFailedCountAsync(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            user.AccessFailedCount++;
            _storage.UsersTable.Update(user);

            return Task.FromResult(user.AccessFailedCount);
        }

        public Task ResetAccessFailedCountAsync(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            user.AccessFailedCount = 0;
            _storage.UsersTable.Update(user);

            return Task.FromResult<int>(user.AccessFailedCount);
        }

        public Task<int> GetAccessFailedCountAsync(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return Task.FromResult(user.AccessFailedCount);
        }

        public Task<bool> GetLockoutEnabledAsync(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return Task.FromResult(user.LockoutEnabled);
        }

        public Task SetLockoutEnabledAsync(TUser user, bool enabled)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            user.LockoutEnabled = enabled;
            _storage.UsersTable.Update(user);

            return Task.FromResult<int>(0);
        }

        public Task AddLoginAsync(TUser user, UserLoginInfo login)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (login == null)
            {
                throw new ArgumentNullException("login");
            }

            _storage.UserLoginsTable.AddLogin(user, login);

            return Task.FromResult<int>(0);
        }

        public Task RemoveLoginAsync(TUser user, UserLoginInfo login)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (login == null)
            {
                throw new ArgumentNullException("login");
            }

            _storage.UserLoginsTable.RemoveLogin(user, login);

            return Task.FromResult<int>(0);
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            var logins = _storage.UserLoginsTable.GetLogins(user);

            return Task.FromResult<IList<UserLoginInfo>>(logins);
        }

        public Task<TUser> FindAsync(UserLoginInfo login)
        {
            if (login == null)
            {
                throw new ArgumentNullException("login");
            }

            var userId = _storage.UserLoginsTable.FindUserId(login);
            var user = _storage.UsersTable.GetUserById(userId) as TUser;

            return Task.FromResult(user);
        }

        public Task SetSecurityStampAsync(TUser user, string stamp)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            user.SecurityStamp = stamp;
            _storage.UsersTable.Update(user);

            return Task.FromResult(0);
        }

        public Task<string> GetSecurityStampAsync(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return Task.FromResult(user.SecurityStamp);
        }

        public Task SetEmailAsync(TUser user, string email)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            user.Email = email;
            _storage.UsersTable.Update(user);

            return Task.FromResult(0);
        }

        public Task<string> GetEmailAsync(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return Task.FromResult(user.EmailConfirmed);
        }

        public Task SetEmailConfirmedAsync(TUser user, bool confirmed)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            user.EmailConfirmed = confirmed;
            _storage.UsersTable.Update(user);

            return Task.FromResult(0);
        }

        public Task<TUser> FindByEmailAsync(string email)
        {
            return Task.FromResult(_storage.UsersTable.GetUserByEmail(email) as TUser);
        }

        public Task SetPhoneNumberAsync(TUser user, string phoneNumber)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            user.PhoneNumber = phoneNumber;
            _storage.UsersTable.Update(user);

            return Task.FromResult(0);
        }

        public Task<string> GetPhoneNumberAsync(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return Task.FromResult(user.PhoneNumber);
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return Task.FromResult(user.PhoneNumberConfirmed);
        }

        public Task SetPhoneNumberConfirmedAsync(TUser user, bool confirmed)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            user.PhoneNumberConfirmed = confirmed;
            _storage.UsersTable.Update(user);

            return Task.FromResult(0);
        }

        public Task SetTwoFactorEnabledAsync(TUser user, bool enabled)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            user.TwoFactorEnabled = enabled;
            _storage.UsersTable.Update(user);

            return Task.FromResult(0);
        }

        public Task<bool> GetTwoFactorEnabledAsync(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return Task.FromResult(user.TwoFactorEnabled);
        }
    }
}