using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace Simple.Data.AspNet.Identity {

    /// <summary>
    /// Class that implements the key ASP.NET Identity user store iterfaces
    /// </summary>
    public class UserStore<TUser> : IUserStore<TUser>, IUserRoleStore<TUser>,
        IUserPasswordStore<TUser>, IUserClaimStore<TUser>, IUserLockoutStore<TUser, string>, IUserLoginStore<TUser>,
        IUserSecurityStampStore<TUser>, IUserEmailStore<TUser>, IUserPhoneNumberStore<TUser>, IUserTwoFactorStore<TUser,string> where TUser : IdentityUser
    {
        private readonly Storage<TUser,IdentityRole> _storage;
        private bool _disposed;

        /// <summary>
        /// <see cref="Tables"/> object with names of tables used for identity provider
        /// </summary>
        public Tables Tables { get; set; }

        /// <summary>
        /// Default constructor that initializes a new
        /// instance using the default Simple.Data connection string
        /// from properties.
        /// 
        /// Uses the default AspNet Identity table names
        /// </summary>
        public UserStore():this(null, null) { }

        /// <summary>
        /// Constructor that initializes a new instance using the default Simple.Data connection string
        /// from properties.
        /// 
        /// Uses the table names provided
        /// </summary>
        public UserStore(Tables tables):this(null, tables) { }

        /// <summary>
        /// Constructor that initializes a new
        /// instance using the named connection string
        /// 
        /// Uses the default AspNet Identity table names
        /// </summary>
        public UserStore(string connectionName):this(connectionName, null) { }

        /// <summary>
        /// Constructor that initializes a new
        /// instance using the named connection string
        /// 
        /// Uses the table names provided
        /// </summary>
        public UserStore(string connectionName, Tables tables)
        {
            Tables = tables ?? new Tables();
            _storage = connectionName == null ? new Storage<TUser,IdentityRole>(Tables) : new Storage<TUser,IdentityRole>(connectionName, Tables);
        }

        /// <summary>
        /// Releases all resources used by the current instance of the <see cref="UserStore"/>
        /// </summary>
        public void Dispose()
        {
            _disposed = true;
        }

        /// <summary>
        /// Insert a new user in the users table
        /// </summary>
        /// <param name="user">User to insert</param>
        /// <returns>The task representing the asynchronous operation.</returns>
        public Task CreateAsync(TUser user)
        {
            ThrowIfDisposed();

            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            _storage.UsersTable.Insert(user);

            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// Updates a user in the users table
        /// </summary>
        /// <param name="user">User to update</param>
        /// <returns>The task representing the asynchronous operation.</returns>
        public Task UpdateAsync(TUser user)
        {
            ThrowIfDisposed();

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

        /// <summary>
        /// Deletes a TUser from the users table
        /// </summary>
        /// <param name="user">TUser to delete</param>
        /// <returns>The task representing the asynchronous operation.</returns>
        public Task DeleteAsync(TUser user)
        {
            ThrowIfDisposed();

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

        /// <summary>
        /// Find a user from the specified Id
        /// </summary>
        /// <param name="userId">The user identifier</param>
        /// <returns>Task whose result is the user; otherwise task with result null</returns>
        public Task<TUser> FindByIdAsync(string userId)
        {
            ThrowIfDisposed();

            var result = _storage.UsersTable.GetUserById(userId) as TUser;

            return Task.FromResult(result);
        }

        /// <summary>
        /// Finds a user by the user name
        /// </summary>
        /// <param name="userName">The user name</param>
        /// <returns>Task whose result is the user; otherwise task with result null</returns>
        public Task<TUser> FindByNameAsync(string userName)
        {
            ThrowIfDisposed();

            var result = _storage.UsersTable.GetUserByName(userName) as TUser;

            return Task.FromResult(result);
        }

        /// <summary>
        /// Adds a user to a role
        /// </summary>
        /// <param name="user">The user</param>
        /// <param name="roleName">Name of role</param>
        /// <returns>The task representing the asynchronous operation.</returns>
        public Task AddToRoleAsync(TUser user, string roleName)
        {
            ThrowIfDisposed();

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

        /// <summary>
        /// Removes a user from a role
        /// </summary>
        /// <param name="user">The user</param>
        /// <param name="roleName">Name of the role</param>
        /// <returns>The task representing the asynchronous operation.</returns>
        public Task RemoveFromRoleAsync(TUser user, string roleName)
        {
            ThrowIfDisposed();

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

        /// <summary>
        /// Returns roles for a user
        /// </summary>
        /// <param name="user">The user</param>
        /// <returns>Task whose result is <see cref="IList"><see cref="string"/></see>/></returns>
        public Task<IList<string>> GetRolesAsync(TUser user)
        {
            ThrowIfDisposed();

            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            IList<string> roleNames = _storage.UserRolesTable.FindByUserId(user.Id)
                .Select(role => role.Name)
                .ToList();

            return Task.FromResult(roleNames);
        }

        /// <summary>
        /// Returns whether a user is in a role
        /// </summary>
        /// <param name="user">The user</param>
        /// <param name="roleName">Name of the role</param>
        /// <returns>Task whose result is true if in the role;otherwise Task with result false</returns>
        public Task<bool> IsInRoleAsync(TUser user, string roleName)
        {
            ThrowIfDisposed();   

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

        /// <summary>
        /// Sets the password has for a user
        /// </summary>
        /// <param name="user">The user</param>
        /// <param name="passwordHash">Password hash</param>
        /// <returns>The task representing the asynchronous operation.</returns>
        public Task SetPasswordHashAsync(TUser user, string passwordHash)
        {
            ThrowIfDisposed();

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

        /// <summary>
        /// Returns password hash for a user
        /// </summary>
        /// <param name="user">The user</param>
        /// <returns>Task whose result is users password hash</returns>
        public Task<string> GetPasswordHashAsync(TUser user)
        {
            ThrowIfDisposed();

            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            string passwordHash = _storage.UsersTable.GetPasswordHash(user);

            return Task.FromResult(passwordHash);
        }

        /// <summary>
        /// Returns if user has password hash
        /// </summary>
        /// <param name="user">The user</param>
        /// <returns>Task whose result is true if password hash exists; else task result is false.</returns>
        public Task<bool> HasPasswordAsync(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            var result = !string.IsNullOrEmpty(_storage.UsersTable.GetPasswordHash(user));

            return Task.FromResult(result);
        }

        /// <summary>
        /// Get claims for a user
        /// </summary>
        /// <param name="user">The user</param>
        /// <returns>Task whose result is <see cref="IList"><see cref="Claim"/></see> for the user.</returns>
        public Task<IList<Claim>> GetClaimsAsync(TUser user)
        {
            ThrowIfDisposed();

            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            var result = _storage.UserClaimsTable.FindByUserId(user.Id);

            return Task.FromResult<IList<Claim>>(result.Claims.ToList());
        }

        /// <summary>
        /// Adds a claim for a user
        /// </summary>
        /// <param name="user">The user</param>
        /// <param name="claim">Claim to add</param>
        /// <returns>The task representing the asynchronous operation.</returns>
        public Task AddClaimAsync(TUser user, Claim claim)
        {
            ThrowIfDisposed();

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

        /// <summary>
        /// Removes a claim from a user
        /// </summary>
        /// <param name="user">The user</param>
        /// <param name="claim">Claim to remove</param>
        /// <returns>The task representing the asynchronous operation.</returns>
        public Task RemoveClaimAsync(TUser user, Claim claim)
        {
            ThrowIfDisposed();

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

        /// <summary>
        /// Returns the Lock end date for a user
        /// </summary>
        /// <param name="user">The user</param>
        /// <returns>Task whose result is <see cref="DateTimeOffset"/> of lockout end date or default DateTimeOffset value</returns>
        public Task<DateTimeOffset> GetLockoutEndDateAsync(TUser user)
        {
            ThrowIfDisposed();

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

        /// <summary>
        /// Sets the Lockout End Date for a user
        /// </summary>
        /// <param name="user">The user</param>
        /// <param name="lockoutEnd">The Lockout end.</param>
        /// <returns>The task representing the asynchronous operation.</returns>
        public Task SetLockoutEndDateAsync(TUser user, DateTimeOffset lockoutEnd)
        {
            ThrowIfDisposed();

            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            user.LockoutEndDateUtc = lockoutEnd.UtcDateTime;

            _storage.UsersTable.Update(user);

            return Task.FromResult<int>(0);
        }

        /// <summary>
        /// Increments the access failure count for a user
        /// </summary>
        /// <param name="user">The user</param>
        /// <returns>Task whose result is current count of failed access attempts</returns>
        public Task<int> IncrementAccessFailedCountAsync(TUser user)
        {
            ThrowIfDisposed();

            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            user.AccessFailedCount++;
            _storage.UsersTable.Update(user);

            return Task.FromResult(user.AccessFailedCount);
        }

        /// <summary>
        /// Resets the access failed count for a user
        /// </summary>
        /// <param name="user">The user</param>
        /// <returns>Task whose result is current count of failed access attempts</returns>
        public Task ResetAccessFailedCountAsync(TUser user)
        {
            ThrowIfDisposed();

            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            user.AccessFailedCount = 0;
            _storage.UsersTable.Update(user);

            return Task.FromResult<int>(user.AccessFailedCount);
        }

        /// <summary>
        /// Returns the access failed count for a user
        /// </summary>
        /// <param name="user">The user</param>
        /// <returns>Task whose result is current count of failed access attempts</returns>
        public Task<int> GetAccessFailedCountAsync(TUser user)
        {
            ThrowIfDisposed();

            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return Task.FromResult(user.AccessFailedCount);
        }

        /// <summary>
        /// Returns true if Lockout is enabled for a user
        /// </summary>
        /// <param name="user">The user</param>
        /// <returns>Task whose result is true if lockout enabled; else task result is false</returns>
        public Task<bool> GetLockoutEnabledAsync(TUser user)
        {
            ThrowIfDisposed();

            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return Task.FromResult(user.LockoutEnabled);
        }

        /// <summary>
        /// Sets Lockout enbabled for a user
        /// </summary>
        /// <param name="user">The user</param>
        /// <param name="enabled">true if the user can be locked out; otherwise, false.</param>
        /// <returns></returns>
        public Task SetLockoutEnabledAsync(TUser user, bool enabled)
        {
            ThrowIfDisposed();

            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            user.LockoutEnabled = enabled;
            _storage.UsersTable.Update(user);

            return Task.FromResult<int>(0);
        }

        /// <summary>
        /// Adds login for a user
        /// </summary>
        /// <param name="user">The user</param>
        /// <param name="login">The login information</param>
        /// <returns>The task representing the asynchronous operation.</returns>
        public Task AddLoginAsync(TUser user, UserLoginInfo login)
        {
            ThrowIfDisposed();

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

        /// <summary>
        /// Removes the user login
        /// </summary>
        /// <param name="user">The user</param>
        /// <param name="login">The login information</param>
        /// <returns>The task representing the asynchronous operation.</returns>
        public Task RemoveLoginAsync(TUser user, UserLoginInfo login)
        {
            ThrowIfDisposed();

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

        /// <summary>
        /// Returns the linked accounts for this user
        /// </summary>
        /// <param name="user">The user</param>
        /// <returns>Task whose result is a <see cref="IList"><see cref="UserLogin"/></see> for the user.</returns>
        public Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user)
        {
            ThrowIfDisposed();

            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            var logins = _storage.UserLoginsTable.GetLogins(user);

            return Task.FromResult<IList<UserLoginInfo>>(logins);
        }

        /// <summary>
        /// Returns the user associated with this login
        /// </summary>
        /// <param name="login">The login information</param>
        /// <returns>Task whose result is a user</returns>
        public Task<TUser> FindAsync(UserLoginInfo login)
        {
            ThrowIfDisposed();

            if (login == null)
            {
                throw new ArgumentNullException("login");
            }

            var userId = _storage.UserLoginsTable.FindUserId(login);
            var user = _storage.UsersTable.GetUserById(userId) as TUser;

            return Task.FromResult(user);
        }

        /// <summary>
        /// Sets the security stamp for the user
        /// </summary>
        /// <param name="user">The user</param>
        /// <param name="stamp">The security stamp</param>
        /// <returns>The task representing the asynchronous operation.</returns>
        public Task SetSecurityStampAsync(TUser user, string stamp)
        {
            ThrowIfDisposed();

            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            user.SecurityStamp = stamp;
            _storage.UsersTable.Update(user);

            return Task.FromResult(0);
        }

        /// <summary>
        /// Returns security stamp for a user
        /// </summary>
        /// <param name="user">The user</param>
        /// <returns>Task whose result is users security stamp</returns>
        public Task<string> GetSecurityStampAsync(TUser user)
        {
            ThrowIfDisposed();

            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return Task.FromResult(user.SecurityStamp);
        }

        /// <summary>
        /// Sets the user email
        /// </summary>
        /// <param name="user">The user</param>
        /// <param name="email">Email address</param>
        /// <returns>The task representing the asynchronous operation.</returns>
        public Task SetEmailAsync(TUser user, string email)
        {
            ThrowIfDisposed();

            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            user.Email = email;
            _storage.UsersTable.Update(user);

            return Task.FromResult(0);
        }

        /// <summary>
        /// Returns the email address for a user
        /// </summary>
        /// <param name="user">The user</param>
        /// <returns>Task whose result is the email address for the user</returns>
        public Task<string> GetEmailAsync(TUser user)
        {
            ThrowIfDisposed();

            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return Task.FromResult(user.Email);
        }

        /// <summary>
        /// Returns true if the email is confirmed
        /// </summary>
        /// <param name="user">The user</param>
        /// <returns>Task whose return value is true if email confirmed; otherwise task return value is false</returns>
        public Task<bool> GetEmailConfirmedAsync(TUser user)
        {
            ThrowIfDisposed();

            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return Task.FromResult(user.EmailConfirmed);
        }

        /// <summary>
        /// Sets whether the the user email is confirmed
        /// </summary>
        /// <param name="user">The user</param>
        /// <param name="confirmed">true if the user e-mail is confirmed; otherwise false</param>
        /// <returns>The task representing the asynchronous operation.</returns>
        public Task SetEmailConfirmedAsync(TUser user, bool confirmed)
        {
            ThrowIfDisposed();

            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            user.EmailConfirmed = confirmed;
            _storage.UsersTable.Update(user);

            return Task.FromResult(0);
        }

        /// <summary>
        /// Returns user associated with the email
        /// </summary>
        /// <param name="email">The user e-mail</param>
        /// <returns>Task whose result is the user; if no user found result is null</returns>
        public Task<TUser> FindByEmailAsync(string email)
        {
            ThrowIfDisposed();

            return Task.FromResult(_storage.UsersTable.GetUserByEmail(email) as TUser);
        }

        /// <summary>
        /// Sets the phone number associated with the user.
        /// </summary>
        /// <param name="user">The user</param>
        /// <param name="phoneNumber">The user phone number</param>
        /// <returns>The task representing the asynchronous operation.</returns>
        public Task SetPhoneNumberAsync(TUser user, string phoneNumber)
        {
            ThrowIfDisposed();

            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            user.PhoneNumber = phoneNumber;
            _storage.UsersTable.Update(user);

            return Task.FromResult(0);
        }

        /// <summary>
        /// Gets the user phone number
        /// </summary>
        /// <param name="user">The user</param>
        /// <returns>Task whose result is the users phone number</returns>
        public Task<string> GetPhoneNumberAsync(TUser user)
        {
            ThrowIfDisposed();

            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return Task.FromResult(user.PhoneNumber);
        }

        /// <summary>
        /// Gets whether the user phone number is confirmed.
        /// </summary>
        /// <param name="user">The user</param>
        /// <returns>Task whose result is true if phone number confirmed; otherwise false</returns>
        public Task<bool> GetPhoneNumberConfirmedAsync(TUser user)
        {
            ThrowIfDisposed();

            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return Task.FromResult(user.PhoneNumberConfirmed);
        }

        /// <summary>
        /// Sets whether the user phone number is confirmed.
        /// </summary>
        /// <param name="user">The user</param>
        /// <param name="confirmed">true if the user phone number is confirmed; otherwise, false.</param>
        /// <returns>The task representing the asynchronous operation.</returns>
        public Task SetPhoneNumberConfirmedAsync(TUser user, bool confirmed)
        {
            ThrowIfDisposed();

            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            user.PhoneNumberConfirmed = confirmed;
            _storage.UsersTable.Update(user);

            return Task.FromResult(0);
        }

        /// <summary>
        /// Sets whether two factor authentication is enabled for the user.
        /// </summary>
        /// <param name="user">The user to set for</param>
        /// <param name="enabled">true to enable the two factor authentication; otherwise, false.</param>
        /// <returns>The task representing the asynchronous operation.</returns>
        public Task SetTwoFactorEnabledAsync(TUser user, bool enabled)
        {
            ThrowIfDisposed();

            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            user.TwoFactorEnabled = enabled;
            _storage.UsersTable.Update(user);

            return Task.FromResult(0);
        }

        /// <summary>
        /// Returns whether two factor authentication is enabled for the user.
        /// </summary>
        /// <param name="user">The user to check.</param>
        /// <returns>Task whose result is true if two factor authentiation is enabled; otherwise false.</returns>
        public Task<bool> GetTwoFactorEnabledAsync(TUser user)
        {
            ThrowIfDisposed();

            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return Task.FromResult(user.TwoFactorEnabled);
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }
    }
}