using EMY.HostManager.Bussines.Abstract;
using EMY.HostManager.Bussines.Concrete;
using EMY.HostManager.DataAccess.Concrete.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMY.HostManager.Bussines
{
    public class HostManagerFactory
    {
        DbContext context;

        /// <summary>
        /// Default constructor using Entity Framework
        /// </summary>
        public HostManagerFactory()
        {
#if RELEASE
            throw new Exception("You can not use this constructor in Release Mode");
#endif
            context = new HostManagerContext();
            InitObjects();
        }


        /// <summary>
        /// For release mode
        /// </summary>
        /// <param name="prmContext">Write your selected context</param>
        public HostManagerFactory(DbContext prmContext)
        {
            this.context = prmContext;
            InitObjects();
        }
        private void InitObjects()
        {
            Templates = new TemplateManager(context);
            Users = new UserManager(context);
            ServerInformations = new ServerInformationManager(context);
        }

        public AbstractTemplateService Templates { get; set; }
        public AbstractUserService Users { get; set; }
        public AbstractServerInformationService ServerInformations { get; set; }
    }
}
