using Microsoft.EntityFrameworkCore;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Genesis.idlib.Data;
using Genesis.idlib.Models;
using Genesis.idlib.Infrastructure;
using Genesis.idlib.Repositories;
using Genesis.idlib.RequestObjects;
using Genesis.idlib.Classes;
using System;

namespace Genesis.idlib.Services
{
    public class UserDataService : IUserDataService
    {
        private readonly IUserRepository _userRepo;

        public UserDataService(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }
        

        public PagedList<UserStub> GetUsersPage(DataItemPageRequest usrPageReq)
        {
            var pagedListUsr = _userRepo.GetUsersPage(usrPageReq);

            return pagedListUsr;
        }
    }
}