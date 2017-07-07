using System.Collections.Generic;

using userhub.Models;

namespace userhub.Infrastructure.Services
{
    public interface IModelDataService
    {
        IList<CompanyVM> GetCompanies();
    }
}