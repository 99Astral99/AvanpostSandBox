using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Avanpost.Interviews.Task.Integration.SandBox.Connector.Entities
{
    [Table("Passwords", Schema = "AvanpostIntegrationTestTaskSchema")]
    public class Sequrity
    {
        [Key]
        [Column("id")]
        public int? Id
        {
            [return: NotNull]
            get;
            set;
        }

        [Column("userId")]
        [MaxLength(22)]
        public string UserId
        {
            [return: NotNull]
            get;
            set;
        }

        [Column("password")]
        [MaxLength(20)]
        public string Password
        {
            [return: NotNull]
            get;
            set;
        }
    }
}
