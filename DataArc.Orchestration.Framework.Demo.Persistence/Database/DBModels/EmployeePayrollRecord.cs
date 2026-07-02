using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using DataArc.Orchestration.Framework.Demo.Persistence.DbModels;

namespace DataArc.Orchestration.Framework.Demo.Persistence.Database.DBModels
{
    [Table("EmployeePayrollRecord", Schema = "intersection")]
    public class EmployeePayrollRecord
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        [ForeignKey("EmployeeId")]
        public Employee Employee { get; set; }
        public int PayrollRecordId { get; set; }
        [ForeignKey("PayrollRecordId")]
        public PayrollRecord PayrollRecord { get; set; }
    }
}