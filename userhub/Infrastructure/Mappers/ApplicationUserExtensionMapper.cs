using System.Collections.Generic;

using userhub.Models;
using Genesis.idlib.Classes;
using Genesis.idlib.Models;


namespace userhub.Infrastructure.Mappers
{
    public static class ApplicationUserExtensionMapper
    {
        public static EditUserViewModel MapToEditUserViewModel(this ApplicationUser appUser)
        {
            var retModel = new EditUserViewModel
            {
                Id = appUser.Id,
                CompanyId = appUser.CompanyId,
                Email = appUser.Email,
                FirstName = appUser.FirstName,
                LastName = appUser.LastName,
                PhoneNumber = appUser.PhoneNumber
            };

            return retModel;
        }

        public static UserRowViewModel MaptoUserRowViewModel(this UserStub usrStub)
        {
            var retModel = new UserRowViewModel
            {
                Id = usrStub.Id,
                Company = usrStub.CompanyName,
                FirstName = usrStub.FirstName,
                LastName = usrStub.LastName,
                Active = usrStub.IsaActive                
            };
                
            return retModel;
        }

        public static IList<UserRowViewModel> ConverToUserRowVMList(this IList<UserStub> userStubList)
        {
            var retList = new List<UserRowViewModel>();
            
            foreach(var appUser in userStubList)
            {
                retList.Add(appUser.MaptoUserRowViewModel());
            }

            return retList;
        }
    }
}