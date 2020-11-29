using EMY.HostManager.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EMY.HostManager.Bussines.Abstract
{
    public abstract class AbstractDomainService
    {
        public abstract Task<IEnumerable<DomainInformation>> GetDomainList();
        public abstract Task<DomainInformation> GetDomainInformationByDomainInformationID(int DomainInformationID);
        public abstract Task Add(DomainInformation DomainInformation, int adderRef);
        public abstract Task Update(DomainInformation DomainInformation, int updaterRef);
        public abstract Task Delete(DomainInformation DomainInformation, int deleterRef);
        public abstract Task<DomainInformation> GetDomainByName(string DomainName);
    }
}
