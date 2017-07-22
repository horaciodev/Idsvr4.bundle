using System.Collections.Generic;
using System.Threading.Tasks;

using Genesis.idlib.Classes;
using Genesis.idlib.Infrastructure;
using Genesis.idlib.Models;
using Genesis.idlib.RequestObjects;

namespace Genesis.idlib.Services
{
    public interface IUserDataService
    {
            PagedList<UserStub> GetUsersPage(DataItemPageRequest usrPageReq);
    }
}