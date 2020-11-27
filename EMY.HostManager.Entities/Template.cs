using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMY.HostManager.Entities
{
    [Table("tblTemplates")]
    public class Template : BaseEntity
    {
        [Key]
        public int TemplateID { get; set; }
        public string TemplateName { get; set; }
        public string TemplateCode { get; set; }
        public ICollection<TemplateParameter> Parameters { get; set; }

    }
}
