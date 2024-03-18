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
    [Table("UserITRole", Schema = "AvanpostIntegrationTestTaskSchema")]
    public class UserITRole
    {
        [Key]
        [Column("userId")]
        [MaxLength(22)]
        public string UserId
        {
            [return: NotNull]
            get;
            set;
        }

        [Key]
        [Column("roleId")]
        public int RoleId
        {
            [return: NotNull]
            get;
            set;
        }
    }
}
