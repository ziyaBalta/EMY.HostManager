using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMY.HostManager.Entities
{
    [Table(name: "tblServerInformations")]
    public class ServerInformation : BaseEntity
    {

        [Key]
        public int ServerInformationID { get; set; }
        [Required]
        public string ServerName { get; set; }

        public ServerTypes ServerType { get; set; }
        [Required]
        public string ServerAdress { get; set; }
        [Required]
        public int Port { get; set; }
        [Required]
        public string UserName { get; set; }
        public string Password { get; set; }

    }

}
public enum ServerTypes
{
    [Description("Web Server")]
    WebServer,
    [Description("Database Server")]
    DbServer
}