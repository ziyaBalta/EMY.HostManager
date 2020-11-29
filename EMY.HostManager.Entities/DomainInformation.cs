using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMY.HostManager.Entities
{
    [Table("tblDomainInformations")]
    public class DomainInformation : BaseEntity
    {
        [Key]
        public int DomainInformationID { get; set; }
        [Required]
        public string DomainName { get; set; }
        [Required]
        public string DomainAdress { get; set; }

    }
}
