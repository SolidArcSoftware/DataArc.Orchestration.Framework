using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demo.Persistence.DbModels
{
    [Table("PayrollRecords", Schema = "finance")]
    public sealed class PayrollRecord
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Precision(18, 2)]
        public decimal AnnualSalary { get; set; }

        public string CurrencyCode { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        public DateTimeOffset CreatedOnUtc { get; set; }

        public int EmployeeId { get; set; }

        [ForeignKey(nameof(EmployeeId))]
        public Employee? Employee { get; set; }
    }
}