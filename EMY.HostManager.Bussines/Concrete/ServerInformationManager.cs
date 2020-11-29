using EMY.HostManager.Bussines.Abstract;
using EMY.HostManager.DataAccess.Abstract;
using EMY.HostManager.DataAccess.Concrete;
using EMY.HostManager.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EMY.HostManager.Bussines.Concrete
{
    public class ServerInformationManager : AbstractServerInformationService
    {
        private IAsyncRepository<ServerInformation> repository = null;
        public ServerInformationManager(DbContext context)
        {
            this.repository = new GenericRepository<ServerInformation>(context);
        }

        public override async Task Add(ServerInformation serverInformation, int adderRef)
        {
            await repository.Add(serverInformation, adderRef);
        }

        public async override Task Delete(ServerInformation serverInformation, int deleterRef)
        {
            await repository.Remove(serverInformation.ServerInformationID, deleterRef);
        }

        public override async Task<ServerInformation> GetServerByName(string serverName)
        {
            var result = await repository.FirstOrDefault(o => o.ServerName == serverName && !o.IsDeleted);
            return result;
        }

        public override async Task<ServerInformation> GetServerInformationByServerInformationID(int ServerInformationID)
        {
            var Result = await repository.GetByPrimaryKey(ServerInformationID);
            return Result;
        }

        public override async Task<IEnumerable<ServerInformation>> GetServerList()
        {
            var Result = await repository.GetWhere(o => !o.IsDeleted);
            return Result;
        }

        public override async Task Update(ServerInformation serverInformation, int updaterRef)
        {
            await repository.Update(serverInformation, updaterRef);
        }
    }
}
