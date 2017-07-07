using System;
using System.Collections.Generic;
using userhub.Models;

namespace userhub.Infrastructure.Services
{
    public class ModelDataService : IModelDataService
    {
        IList<CompanyVM> IModelDataService.GetCompanies()
        {
            //TODO: retrieve from API call
            return  new List<CompanyVM>{ new CompanyVM(){ CompanyId = 0, CompanyName ="Parent" },
                                         new CompanyVM(){ CompanyId = 1, CompanyName="Child One"} };
        }
    }
}