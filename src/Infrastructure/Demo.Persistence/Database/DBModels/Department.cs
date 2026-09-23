using System.ComponentModel.DataAnnotations.Schema;

namespace Demo.Persistence.DbModels
{
    [Table("Department", Schema = "hr")]
    public class Department
    {

        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}