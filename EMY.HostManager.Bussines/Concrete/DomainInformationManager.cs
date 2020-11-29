using EMY.HostManager.Bussines.Abstract;
using EMY.HostManager.DataAccess.Abstract;
using EMY.HostManager.DataAccess.Concrete;
using EMY.HostManager.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace EMY.HostManager.Bussines.Concrete
{
    public class DomainInformationManager : AbstractDomainService
    {

        private IAsyncRepository<DomainInformation> repository = null;
        public DomainInformationManager(DbContext context)
        {
            this.repository = new GenericRepository<DomainInformation>(context);
        }
        public override async Task Add(DomainInformation domainInformation, int adderRef)
        {
            await repository.Add(domainInformation, adderRef);
        }

        public override async Task Delete(DomainInformation domainInformation, int deleterRef)
        {
            await repository.Remove(domainInformation.DomainInformationID, deleterRef);
        }

        public override async Task<DomainInformation> GetDomainByName(string DomainName)
        {
            var result = await repository.FirstOrDefault(o => o.DomainName == DomainName && !o.IsDeleted);
            return result;
        }

        public override async Task<DomainInformation> GetDomainInformationByDomainInformationID(int DomainInformationID)
        {
            var result = await repository.GetByPrimaryKey(DomainInformationID);
            return result;
        }

        public override async Task<IEnumerable<DomainInformation>> GetDomainList()
        {
            var result = await repository.GetWhere(o => !o.IsDeleted);
            return result;
        }

        public override async Task Update(DomainInformation DomainInformation, int updaterRef)
        {
            await repository.Update(DomainInformation, updaterRef);
        }
    }
}
