using Dapper;
using definer.Core.Interface.User;
using definer.Entity.Helpers;
using definer.Entity.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace definer.Core.Repo.User
{
    public class FollowJunctionRepository : Connection.dbConnection, IFollowJunction
    {
        public ProcessResult Add(FollowJunction entity)
        {
            throw new NotImplementedException();
        }

        public ProcessResult Delete(int ID)
        {
            throw new NotImplementedException();
        }

        public FilteredList<FollowJunction> FilteredList(FilteredList<FollowJunction> request)
        {
            throw new NotImplementedException();
        }

        public FollowJunction Get(int ID)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<FollowJunction> GetAll()
        {
            throw new NotImplementedException();
        }

        public FollowJunction SetState(FollowJunction entity)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@ID", entity.ID);
                param.Add("@UserID", entity.UserID);
                param.Add("@FollowerID", entity.FollowerID);
                param.Add("@Date", entity.Date);

                string query = $@"
                DECLARE  @result table(ID Int, UserID Int, FollowerID Int, Date datetime)
                if exists(SELECT * from FollowJunction where UserID = @UserID AND FollowerID = @FollowerID)        
                BEGIN            
                Delete from FollowJunction WHERE UserID = @UserID AND FollowerID = @FollowerID
                End                    
                else         
                begin 
                INSERT INTO FollowJunction (UserID, FollowerID, Date)
                                OUTPUT INSERTED.* INTO @result
                                VALUES (@UserID, @FollowerID, @Date)
                end
                SELECT *
				FROM @result";

                using (var connection = GetConnection)
                {
                    return connection.Query<FollowJunction>(query, param).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                //LogsRepository.CreateLog(ex);
                return null;
            }
        }

        public ProcessResult Update(FollowJunction entity)
        {
            throw new NotImplementedException();
        }
    }
}
