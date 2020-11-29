using EMY.HostManager.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EMY.HostManager.Bussines.Abstract
{
    public abstract class AbstractTemplateService
    {
        public abstract Task<Template> GetByTeplateID(int TeplateID);
        public abstract Task<IEnumerable<Template>> GetAll();
        public abstract Task Add(Template newTemplate, int adderRef);
        public abstract Task Update(Template template, int updaterRef);
        public async Task Delete(Template template, int deleterRef)
        {
            if (template != null && template.TemplateID > 0)
                await Delete(template.TemplateID, deleterRef);

            else if (template == null)
            {
                throw new ArgumentNullException("Template is null!");
            }
            else if (!(template.TemplateID > 0))
            {
                throw new MissingFieldException("Template Id value is not real!");
            }
            else
            {
                throw new Exception("Undefined exception!");
            }
        }
        public abstract Task Delete(int ID, int DeleterRef);
        public abstract Task<Template> GetByTemplateName(string templateName);
    }
}
