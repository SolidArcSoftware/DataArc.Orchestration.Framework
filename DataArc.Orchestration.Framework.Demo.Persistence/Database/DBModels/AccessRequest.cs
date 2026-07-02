using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataArc.Orchestration.Framework.Demo.Persistence.DbModels
{
    [Table("AccessRequests", Schema = "it")]
    public sealed class AccessRequest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string EmailAddress { get; set; } = string.Empty;

        public string AccessLevel { get; set; } = string.Empty;

        public string RequestStatus { get; set; } = string.Empty;

        public DateTimeOffset RequestedOnUtc { get; set; }

        public DateTimeOffset? CompletedOnUtc { get; set; }
    }
}