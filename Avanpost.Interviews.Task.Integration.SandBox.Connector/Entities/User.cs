using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avanpost.Interviews.Task.Integration.SandBox.Connector.Entities
{
    [Table("User", Schema = "AvanpostIntegrationTestTaskSchema")]
    public class User
    {
        [Key]
        [Column("login")]
        [MaxLength(22)]
        public string Login
        {
            [return: NotNull]
            get;
            set;
        }

        [Column("lastName")]
        [MaxLength(20)]
        public string LastName
        {
            get; [param: AllowNull]
            set;
        }

        [Column("firstName")]
        [MaxLength(20)]
        public string FirstName
        {
            get; [param: AllowNull]
            set;
        }

        [Column("middleName")]
        [MaxLength(20)]
        public string MiddleName
        {
            get; [param: AllowNull]
            set;
        }

        [Column("telephoneNumber")]
        [MaxLength(20)]
        public string TelephoneNumber
        {
            get; [param: AllowNull]
            set;
        }

        [Column("isLead")]
        public bool IsLead
        {
            [return: NotNull]
            get;
            set;
        }
    }
}
