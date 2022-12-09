using Dapper;
using MvcDemoProject.DBContext;
using MvcDemoProject.Models;
using MvcDemoProject.Repository.Interface;

namespace MvcDemoProject.Repository
{
    public class UserRepository:IUserRepository
    {
        private readonly DapperContext _dapperContext;  
        public UserRepository(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        public async Task<int> AddNewUser(UsertRegistrationModel userRegistration)
        {
            User ur = new User();
            var result = 0;
            var query = @"insert into tblUser(userName,userEmail,password,role,createdBy,createdDate,isDeleted) 
           
            values (@userName,@userEmail,@password,@role,@createdBy,getDate(),0);SELECT CAST(SCOPE_IDENTITY() as int)";
            using (var connection = _dapperContext.CreateConnection())
            {
                var userId = await connection.QuerySingleOrDefaultAsync<int>(@"select Id from tblUser where isDeleted=0 and userEmail=@userEmail", new { userEmail = userRegistration.userEmail });
                if (userId > 0)
                {
                    return result;
                }
                else
                    ur.userName = userRegistration.userName;
                ur.userEmail = userRegistration.userEmail;
                ur.password = userRegistration.password;
                ur.role = "Customer";
                    result = await connection.QuerySingleAsync<int>(query, ur);
              
              

              await connection.ExecuteAsync(@"update tblUser set createdBy=@createdBy where Id=@Id", new { Id = result, createdBy = result });

                return result;





            }
        }

        public async Task<int> DeleteUser(int id)
        {

            var query = @"update tblUser set modifiedBy=201,modifiedDate=getDate(),isDeleted=1 where Id=@Id";
            using (var connection = _dapperContext.CreateConnection())
            {
                var result = await connection.ExecuteAsync(query, new {Id= id});
                return result;  
            }
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            var query = @"select * from tblUser where isDeleted=0";
            using(var connection=_dapperContext.CreateConnection())
            {
                var result = await connection.QueryAsync<User>(query);
                return result.ToList();

            }
        }

        public async Task<User> GetById(int id)
        {
            var query = @"select * from tblUser where Id=@Id and IsDeleted=0";
            using(var connection=_dapperContext.CreateConnection())
            {
               var result=await connection.QuerySingleAsync<User>(query,new {Id=id});

                return result;
            }
        }

        public async Task<int> UpdateUser(User user)
        {
            var query = @"update tblUser set userName=@userName,userEmail=@userEmail,password=@password,

                            role=@role,modifiedBy=@modifiedBy,modifiedDate=@modifiedDate  where Id=@Id and isDeleted=0";
            using (var connction = _dapperContext.CreateConnection())
            {
                var result=await connction.ExecuteAsync(query,user);

                return result;
            }
        }

        public async Task<User> UserlogIn(logUserModel userlogin)
        {
            var query = @"select * from tblUser where userName=@userName and password=@password and userName=@userName and isDeleted=0";
            using(var connection=_dapperContext.CreateConnection())
            {
                var user=await connection.QueryAsync<User>(query,userlogin);

                return user.FirstOrDefault();
            }
        }
    }
}
