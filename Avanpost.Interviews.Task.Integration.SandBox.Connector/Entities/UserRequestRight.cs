using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Avanpost.Interviews.Task.Integration.SandBox.Connector.Entities
{
    [Table("UserRequestRight", Schema = "AvanpostIntegrationTestTaskSchema")]
    public class UserRequestRight
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
        [Column("rightId")]
        public int RightId
        {
            [return: NotNull]
            get;
            set;
        }
    }
}
