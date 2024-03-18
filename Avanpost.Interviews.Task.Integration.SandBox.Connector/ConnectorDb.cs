using Avanpost.Interviews.Task.Integration.Data.Models;
using Avanpost.Interviews.Task.Integration.Data.Models.Models;
using Avanpost.Interviews.Task.Integration.SandBox.Connector.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Avanpost.Interviews.Task.Integration.SandBox.Connector
{
    public class ConnectorDb : DbContext, IConnector
    {
        private string _connectionString { get; set; }

        public ConnectorDb()
        {

        }

        public void StartUp(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void CreateUser(UserToCreate user)
        {
            using var context = new ConnectorDb();

            if (IsUserExists(user.Login))
            {
                Logger.Error("User already exists");
                throw new ArgumentException("User already exists");
            }

            var newUser = new User()
            {
                Login = user.Login,
            };

            context.Users.Add(newUser);
            context.SaveChanges();

            Logger.Debug($"User with login {newUser.Login} has been created");
        }

        public IEnumerable<Property> GetAllProperties()
        {
            using var context = new ConnectorDb();

            var properties = context.Users
                    .Join(context.Passwords,
                        user => user.Login,
                        security => security.UserId,
                        (user, security) => new { User = user, Security = security })
                    .Select(result => new Property(
                        $"User: {result.User.Login}",
                        $"Last Name: {result.User.LastName}, " +
                        $"First Name: {result.User.FirstName}, " +
                        $"Middle Name: {result.User.MiddleName}, " +
                        $"Telephone Number: {result.User.TelephoneNumber}, " +
                        $"Is Lead: {result.User.IsLead}, " +
                        $"Password: {result.Security.Password}")
                    )
                    .AsNoTracking()
                    .ToList();

            Logger.Debug($"GetAllProperties response: {JsonSerializer.Serialize(properties)}");
            return properties;
        }

        public IEnumerable<UserProperty> GetUserProperties(string userLogin)
        {
            using var context = new ConnectorDb();

            var userProperties = context.Users
                    .Select(result => new UserProperty(
                        $"User: {result.Login}",
                        $"Last Name: {result.LastName}, " +
                        $"First Name: {result.FirstName}, " +
                        $"Middle Name: {result.MiddleName}, " +
                        $"Telephone Number: {result.TelephoneNumber}, " +
                        $"Is Lead: {result.IsLead}")
                    )
                    .AsNoTracking()
                    .ToList();

            Logger.Debug($"GetUserProperties response: {JsonSerializer.Serialize(userProperties)}");
            return userProperties;
        }

        public bool IsUserExists(string userLogin)
        {
            using var context = new ConnectorDb();

            return context.Users.Any(x => x.Login == userLogin);
        }

        public void UpdateUserProperties(IEnumerable<UserProperty> properties, string userLogin)
        {
            using var context = new ConnectorDb();

            var user = context.Users.FirstOrDefault(u => u.Login == userLogin);

            if (user == null)
                throw new ArgumentException("User not found");

            foreach (var property in properties)
            {
                switch (property.Name)
                {
                    case "LastName":
                        user.LastName = property.Value;
                        break;
                    case "FirstName":
                        user.FirstName = property.Value;
                        break;
                    case "MiddleName":
                        user.MiddleName = property.Value;
                        break;
                    case "TelephoneNumber":
                        user.TelephoneNumber = property.Value;
                        break;
                    case "IsLead":
                        user.IsLead = bool.Parse(property.Value);
                        break;
                    default:
                        break;
                }
            }
            context.SaveChanges();

            Logger.Debug($"User with the username {userLogin} has properties updated: {JsonSerializer.Serialize(properties)}");
        }

        public IEnumerable<Permission> GetAllPermissions()
        {
            List<Permission> permissions = new();

            using var context = new ConnectorDb();

            var itRoles = context.ITRoles
                .AsNoTracking()
                .Select(r => new Permission(r.Id.ToString()!, r.Name, "IT Role"))
                .ToList();

            var requestRights = context.RequestRights
                .AsNoTracking()
                .Select(r => new Permission(r.Id.ToString()!, r.Name, "Request Right"))
                .ToList();

            permissions.AddRange(itRoles);
            permissions.AddRange(requestRights);

            Logger.Debug($"GetAllPermissions response: {JsonSerializer.Serialize(permissions)}");
            return permissions;
        }


        public void AddUserPermissions(string userLogin, IEnumerable<string> rightIds)
        {
            using var context = new ConnectorDb();

            if (!IsUserExists(userLogin))
            {
                Logger.Error("User not found");
                throw new ArgumentException("User not found");
            }

            var userRequestRights = rightIds
                .Select(rightId => new UserRequestRight()
                {
                    UserId = userLogin,
                    RightId = int.Parse(rightId)
                })
                .ToList();

            context.UserRequestRights.AddRange(userRequestRights);

            Logger.Debug($"User with the username {userLogin} has permissions added from the rights id: {JsonSerializer.Serialize(rightIds)}");
        }

        public void RemoveUserPermissions(string userLogin, IEnumerable<string> rightIds)
        {
            using var context = new ConnectorDb();

            if (!IsUserExists(userLogin))
            {
                Logger.Error("User not found");
                throw new ArgumentException("User not found");
            }

            var userRequestRights = rightIds
                .Select(rightId => new UserRequestRight()
                {
                    UserId = userLogin,
                    RightId = int.Parse(rightId)
                })
                .ToList();

            context.UserRequestRights.RemoveRange(userRequestRights);
            context.SaveChanges();

            Logger.Debug($"User with the username {userLogin} has permissions removed from the rights id: {JsonSerializer.Serialize(rightIds)}");
        }

        public IEnumerable<string> GetUserPermissions(string userLogin)
        {
            using var context = new ConnectorDb();

            var userPermissions = context.Users
            .Where(u => u.Login == userLogin)
            .SelectMany(u => context.UserITRoles.Where(ur => ur.UserId == userLogin)
                .SelectMany(ur => context.ITRoles.Where(r => r.Id == ur.RoleId).Select(r => r.Name)))
            .Union(context.UserRequestRights.Where(urr => urr.UserId == userLogin)
                .SelectMany(ur => context.RequestRights.Where(rr => rr.Id == ur.RightId).Select(rr => rr.Name)))
            .AsNoTracking()
            .ToList();

            Logger.Debug($"GetUserPermissions response: {JsonSerializer.Serialize(userPermissions)}");
            return userPermissions;
        }
        public ILogger Logger { get; set; }
        public DbSet<User> Users => Set<User>();

        public DbSet<RequestRight> RequestRights => Set<RequestRight>();

        public DbSet<UserRequestRight> UserRequestRights => Set<UserRequestRight>();

        public DbSet<UserITRole> UserITRoles => Set<UserITRole>();

        public DbSet<ITRole> ITRoles => Set<ITRole>();

        public DbSet<Sequrity> Passwords => Set<Sequrity>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRequestRight>().HasKey("RightId", "UserId");
            modelBuilder.Entity<UserITRole>().HasKey("RoleId", "UserId");
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_connectionString);
        }
    }
}