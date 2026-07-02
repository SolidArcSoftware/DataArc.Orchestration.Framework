using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using DataArc.Orchestration.Framework.Demo.Persistence.DbModels;

namespace DataArc.Orchestration.Framework.Demo.Persistence.Database.DBModels
{
    [Table("EmployeeOnboardingTask", Schema = "intersection")]
    public class EmployeeOnboardingTask
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        [ForeignKey("EmployeeId")]
        public Employee Employee { get; set; }
        public int OnBoardingTaskId { get; set; }
        [ForeignKey("OnBoardingTaskId")]
        public OnboardingTask OnboardingTask { get; set; }
    }
}