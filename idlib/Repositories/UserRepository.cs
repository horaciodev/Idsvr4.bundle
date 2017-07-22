using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;


using Genesis.idlib.Classes;
using Genesis.idlib.Infrastructure;
using Genesis.idlib.RequestObjects;

namespace Genesis.idlib.Repositories
{
    public class UserRepository : IUserRepository
    {
        private string _connStr;

        public UserRepository(string connStr)
        {
            _connStr = connStr;
        }

        PagedList<UserStub> IUserRepository.GetUsersPage(DataItemPageRequest dataItemPageRequest)
        {
            var retPagedList = new PagedList<UserStub>();
            using(var cn = new SqlConnection(_connStr))
            {   
                using(var cmd = new SqlCommand("uspGetAspNetIdentityUsersPaged",cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PageNum",dataItemPageRequest.PageNumber);
                    cmd.Parameters.AddWithValue("@PageSize",dataItemPageRequest.PageSize);
                    cmd.Parameters.AddWithValue("@SortOrder", dataItemPageRequest.SortBy);

                    cn.Open();
                    using(var reader = cmd.ExecuteReader())
                    {   
                        while(reader.Read())
                        {
                            var usrStub = new UserStub(){
                                Id = reader.GetInt64(0),
                                FirstName = reader.GetString(2),
                                LastName = reader.GetString(3),
                                CompanyName = reader.GetString(5),
                                IsaActive = reader.GetBoolean(6)
                            };
                            retPagedList.ItemList.Add(usrStub);
                        }
                        reader.NextResult();
                        if(reader.Read())
                        {
                            var pagedInfo = new PagedInfo(){
                                TotalPages = reader.GetInt32(0),
                                CurrentPage = reader.GetInt32(1),
                                TotalItems = reader.GetInt32(2),
                                PageSize = reader.GetInt32(3)
                                };
                            retPagedList.PagedListInfo = pagedInfo;
                        }
                    }
                }
            }
            return retPagedList;
        }
    }
}