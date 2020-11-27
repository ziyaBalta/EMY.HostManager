using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMY.HostManager.Entities
{
    [Table(name: "tblServerInformations")]
    public class ServerInformation : BaseEntity
    {

        [Key]
        public int ServerInformationID { get; set; }
        public string ServerName { get; set; }
        public ServerTypes ServerType { get; set; }
        public string ServerAdress { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

    }
    public enum ServerTypes
    {
        WebServer,
        DbServer
    }
}
