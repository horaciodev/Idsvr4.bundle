using userhub.Models;
using Genesis.idlib.Models;

namespace userhub.Infrastructure.Mappers
{
    public static class EditUserViewModelMapper
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
    }
}