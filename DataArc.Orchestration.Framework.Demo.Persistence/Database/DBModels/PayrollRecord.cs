using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataArc.Orchestration.Framework.Demo.Persistence.DbModels
{
    [Table("PayrollRecords", Schema = "finance")]
    public sealed class PayrollRecord
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public decimal AnnualSalary { get; set; }

        public string CurrencyCode { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        public DateTimeOffset CreatedOnUtc { get; set; }
    }
}