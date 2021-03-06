using Dapper;
using definer.Core.Interface.User;
using definer.Entity.Helpers;
using definer.Entity.Users;

namespace definer.Core.Repo.User
{
    public class BlockJunctionRepository : Connection.dbConnection, IBlockJunction
    {
        public ProcessResult Add(BlockJunction entity)
        {
            throw new NotImplementedException();
        }

        public ProcessResult Delete(int ID)
        {
            throw new NotImplementedException();
        }

        public FilteredList<BlockJunction> FilteredList(FilteredList<BlockJunction> request)
        {
            throw new NotImplementedException();
        }

        public List<BlockJunction> GetBlockedList(int UserID)
        {
            try
            {
                List<BlockJunction> result = new List<BlockJunction>();
                DynamicParameters param = new DynamicParameters();
                param.Add("@UserID", UserID);

                string WhereClause = @" WHERE t.BlockerID = @UserID";

                string query = $@"
                SELECT t.*
                ,(select Username from Users where ID=t.UserID) Author
                FROM BlockJunction t
                {WhereClause} 
                ORDER BY t.ID DESC";

                using (var connection = GetConnection)
                {
                    return connection.Query<BlockJunction>(query, param).ToList();
                }
            }
            catch (Exception ex)
            {
                //LogsRepository.CreateLog(ex);
                return null;
            }
        }

        public BlockJunction Get(int ID)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BlockJunction> GetAll()
        {
            throw new NotImplementedException();
        }

        public BlockJunction SetState(BlockJunction entity)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@ID", entity.ID);
                param.Add("@UserID", entity.UserID);
                param.Add("@BlockerID", entity.BlockerID);

                string query = $@"
                DECLARE  @result table(ID Int, UserID Int, BlockerID Int)
                if exists(SELECT * from BlockJunction where UserID = @UserID AND BlockerID = @BlockerID)        
                BEGIN            
                Delete from BlockJunction WHERE UserID = @UserID AND BlockerID = @BlockerID
                End                    
                else         
                begin 
                INSERT INTO BlockJunction (UserID, BlockerID)
                                OUTPUT INSERTED.* INTO @result
                                VALUES (@UserID, @BlockerID)
                end
                SELECT *
				FROM @result";

                using (var connection = GetConnection)
                {
                    return connection.Query<BlockJunction>(query, param).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                //LogsRepository.CreateLog(ex);
                return null;
            }
        }

        public ProcessResult Update(BlockJunction entity)
        {
            throw new NotImplementedException();
        }
    }
}
