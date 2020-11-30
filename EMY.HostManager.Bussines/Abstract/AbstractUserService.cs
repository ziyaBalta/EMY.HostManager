using EMY.HostManager.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EMY.HostManager.Bussines.Abstract
{
    public abstract class AbstractUserService
    {
        public abstract Task<User> GetByUserID(int UserID);
        public abstract Task<IEnumerable<User>> GetAll();
        public abstract Task Add(User newUser, int adderRef);
        public abstract Task Update(User user, int updaterRef);
        public abstract Task DeActivate(int UserID, int DeactivaterRef);
        public abstract Task Activate(int UserID, int ActivaterRef);
        public abstract Task<IEnumerable<string>> GetAllRoles(int UserID);
        public abstract Task AddRole(UserRole newRole, int adderRef);
        public abstract Task RemoveRole(int UserID, string FormName, AuthType type, int removerRef);
        public abstract Task ClearAllRoles(int UserID, int removerRef);
        public abstract Task<ResultModel> CheckLoginUser(string userName, string password);

        public abstract Task ChangePassword(int UserID, string newPassword);

        public abstract Task<bool> CheckRoleIsExist(int UserID, string FormName, AuthType type);
        public abstract Task<User> GetUserByUserName(string userName);
    }
}
