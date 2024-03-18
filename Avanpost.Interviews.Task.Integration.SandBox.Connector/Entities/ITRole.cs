using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Avanpost.Interviews.Task.Integration.SandBox.Connector.Entities
{
    [Table("ItRole", Schema = "AvanpostIntegrationTestTaskSchema")]
    public class ITRole
    {
        [Key]
        [Column("id")]
        public int? Id
        {
            [return: NotNull]
            get;
            set;
        }

        [Column("name")]
        [MaxLength(100)]
        public string Name
        {
            [return: NotNull]
            get;
            set;
        }

        [Column("corporatePhoneNumber")]
        [MaxLength(4)]
        public string CorporatePhoneNumber
        {
            [return: NotNull]
            get;
            set;
        }
    }
}
