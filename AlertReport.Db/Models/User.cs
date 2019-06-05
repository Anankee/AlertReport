using AlertReport.Db.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlertReport.Db.Models
{
    public class User
    {
        public User() { }
        public User(string login, string password, string email, string phoneNumber)
        {
            Login = login;
            Password = password;
            Email = email;
            PhoneNumber = phoneNumber;
        }

        [Key]
        public int Id { get; set; }
        public string Login { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsDisabled { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
        public ICollection<Alert> Alerts { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
