using DataArc.Orchestration.Framework.Demo.Persistence.DbModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataArc.Orchestration.Framework.Demo.Persistence.Database.DBModels
{
    [Table("EmployeeAccessRequest", Schema = "intersection")]
    public class EmployeeAccessRequest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        [ForeignKey("EmployeeId")]
        public Employee Employee { get; set; }
        public int AccessRequestId { get; set; }
        [ForeignKey("AccessRequestId")]
        public AccessRequest AccessRequest { get; set; }
    }
}