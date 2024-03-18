using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Avanpost.Interviews.Task.Integration.SandBox.Connector.Entities
{
    [Table("RequestRight", Schema = "AvanpostIntegrationTestTaskSchema")]
    public class RequestRight
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
        public string Name
        {
            [return: NotNull]
            get;
            set;
        }
    }
}
