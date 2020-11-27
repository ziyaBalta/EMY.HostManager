using EMY.HostManager.Bussines.Abstract;
using EMY.HostManager.DataAccess.Abstract;
using EMY.HostManager.DataAccess.Concrete;
using EMY.HostManager.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMY.HostManager.Bussines.Concrete
{
    public class UserManager : AbstractUserService
    {
        private IAsyncRepository<User> repository = null;
        private IAsyncRepository<UserRole> userRoleRepository = null;
        public UserManager(DbContext context)
        {
            this.repository = new GenericRepository<User>(context);
            this.userRoleRepository = new GenericRepository<UserRole>(context);
        }
        public override async Task Add(User newUser, int adderRef)
        {
            await repository.Add(newUser, adderRef);
        }

        public override async Task DeActivate(int UserID, int DeactivaterRef)
        {
            var res = await GetByUserID(UserID);
            res.IsActive = false;
            await repository.Update(res, DeactivaterRef);
        }

        public override async Task Activate(int UserID, int ActivaterRef)
        {
            var res = await GetByUserID(UserID);
            res.IsActive = true;
            await repository.Update(res, ActivaterRef);
        }

        public override async Task<IEnumerable<User>> GetAll()
        {
            var result = await repository.GetAll();
            return result;
        }

        public override async Task<User> GetByUserID(int UserID)
        {
            var result = await repository.GetByPrimaryKey(UserID);
            return result;
        }

        public override async Task Update(User user, int updaterRef)
        {
            await repository.Update(user, updaterRef);
        }

        public override async Task<IEnumerable<string>> GetAllRoles(int UserID)
        {
            var AllRoles = await userRoleRepository.GetWhere(o => !o.IsDeleted && o.UserID == UserID);
            List<string> roles = new List<string>();
            return AllRoles.ToList().Select(o => o.GetAuthCode);
        }

        public override async Task AddRole(UserRole newRole, int adderRef)
        {
            await userRoleRepository.Add(newRole, adderRef);
        }

        public override async Task RemoveRole(int RoleID, int removerRef)
        {
            await userRoleRepository.Remove(RoleID, removerRef);
        }

        public override async Task ClearAllRoles(int UserID, int removerRef)
        {
            var roles = await userRoleRepository.GetWhere(o => !o.IsDeleted && o.UserID == UserID);
            foreach (var role in roles)
            {
                await userRoleRepository.Remove(role.UserRoleID, removerRef);
            }
        }
    }
}
