using System.Collections.Generic;

using Genesis.idlib.Infrastructure;
using Genesis.idlib.Classes;
using Genesis.idlib.RequestObjects;

namespace Genesis.idlib.Repositories
{
    public interface IUserRepository
    {
        PagedList<UserStub> GetUsersPage(DataItemPageRequest dataItemPageRequest);
    }

}