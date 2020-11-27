using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMY.HostManager.Entities
{
    [Table("tblTemplateParameters")]
    public class TemplateParameter : BaseEntity
    {
        [Key]
        public int TemplateParameterID { get; set; }
        public int TemplateID { get; set; }
        [ForeignKey("TemplateID")]
        public Template Template { get; set; }
        public string ParameterName { get; set; }
        public string DefaultValue { get; set; }

    }
}