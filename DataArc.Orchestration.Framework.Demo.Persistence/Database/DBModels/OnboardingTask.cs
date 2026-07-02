using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataArc.Orchestration.Framework.Demo.Persistence.DbModels
{
    [Table("OnboardingTask", Schema = "operations")]
    public sealed class OnboardingTask
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string TaskName { get; set; } = string.Empty;

        public string TaskStatus { get; set; } = string.Empty;

        public DateTimeOffset? DueDateUtc { get; set; }

        public DateTimeOffset? CompletedOnUtc { get; set; }

        public DateTimeOffset? CreatedOnUtc { get; set; }
    }
}