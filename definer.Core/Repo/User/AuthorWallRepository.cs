using Dapper;
using Dapper.Contrib.Extensions;
using definer.Core.Interface.User;
using definer.Entity.Helpers;
using definer.Entity.Users;

namespace definer.Core.Repo.User
{
    public class AuthorWallRepository : Connection.dbConnection, IAuthorWall
    {
        public ProcessResult Add(AuthorWall entity)
        {
            ProcessResult result = new ProcessResult();
            try
            {
                using (var con = GetConnection)
                {
                    result.ReturnID = (int)con.Insert(entity);
                    result.Message = "AuthorWall saved successfully";
                    result.State = ProcessState.Success;
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.State = ProcessState.Error;
            }
            return result;
        }

        public bool Archive(int ID)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@ID", ID);
                string query = $@"
                UPDATE AuthorWall
                SET IsActive = 0
                WHERE ID = @ID";

                using (var connection = GetConnection)
                {
                    var rows = connection.Execute(query, param);
                    if (rows > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                //LogsRepository.CreateLog(ex);
                return false;
            }
        }

        public bool CheckEntryOwner(int EntryID, int UserID)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@UserID", UserID);
                param.Add("@EntryID", EntryID);
                string query = $@"
                SELECT
	                CASE WHEN EXISTS 
	                    (
		                        SELECT
		                        t.*
		                        FROM AuthorWall t
		                        WHERE ID = @EntryID AND  t.SenderID = @UserID
                          )
	                THEN 'TRUE'
	                ELSE 'FALSE'
                END";

                using (var connection = GetConnection)
                {
                    return connection.QueryFirstOrDefault<bool>(query, param);
                }
            }
            catch (Exception ex)
            {
                //LogsRepository.CreateLog(ex);
                return false;
            }
        }

        public ProcessResult Delete(int ID)
        {
            using (var con = GetConnection)
            {
                ProcessResult pr = new ProcessResult();
                try
                {
                    con.Delete(new AuthorWall() { ID = ID });
                    pr.ReturnID = 0;
                    pr.Message = "Success";
                    pr.State = ProcessState.Success;
                }
                catch (Exception)
                {
                    pr.ReturnID = 0;
                    pr.Message = "Error";
                    pr.State = ProcessState.Error;
                }
                return pr;
            }
        }

        public FilteredList<AuthorWall> FilteredList(FilteredList<AuthorWall> request)
        {
            throw new NotImplementedException();
        }

        public AuthorWall Get(int ID)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@ID", ID);

                string WhereClause = @" WHERE t.ID = @ID";

                string query = $@"
                SELECT *
                ,(select Username from Users where ID=t.SenderID) Author
                FROM AuthorWall t 
                {WhereClause} ";

                using (var connection = GetConnection)
                {
                    return connection.Query<AuthorWall>(query, param).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                //LogsRepository.CreateLog(ex);
                return null;
            }
        }

        public IEnumerable<AuthorWall> GetAll()
        {
            throw new NotImplementedException();
        }

        public Users GetbyUsername(FilteredList<AuthorWall> request, string Username, int CurrentUserID)
        {
            try
            {
                var model = new Users();
                FilteredList<AuthorWall> entries = new FilteredList<AuthorWall>();
                DynamicParameters param = new DynamicParameters();
                param.Add("@Username", Username);
                param.Add("@CurrentUserID", CurrentUserID);

                string WhereClause = @" WHERE (t.Username like '%' + @Username + '%')";
                string query_count = $@" Select Count(t.ID) from AuthorWall t 
                WHERE t.UserID = @UserID
                AND NOT t.SenderID in (select UserID from BlockJunction where BlockerID = @CurrentUserID)";

                string userQuery = $@"
                SELECT t.*
                FROM Users t
                {WhereClause}";

                string wallEntries = $@"
                SELECT t.*
                ,(select Username from Users where ID=t.SenderID) Author
                FROM AuthorWall t
                WHERE t.IsActive = 1 AND t.UserID = @UserID
                AND NOT t.SenderID in (select UserID from BlockJunction where BlockerID = @CurrentUserID)
                ORDER BY t.Date DESC 
                OFFSET @StartIndex ROWS
                FETCH NEXT @PageSize ROWS ONLY";

                using (var connection = GetConnection)
                {
                    model = connection.Query<Users>(userQuery, param).FirstOrDefault();
                    param.Add("@UserID", model.ID);
                    param.Add("@Keyword", request.filter.Keyword);
                    param.Add("@PageSize", request.filter.pageSize);
                    entries.totalItems = connection.QueryFirstOrDefault<int>(query_count, param);
                    request.filter.pager = new Page(entries.totalItems, request.filter.pageSize, request.filter.page);
                    param.Add("@StartIndex", request.filter.pager.StartIndex);
                    entries.data = connection.Query<AuthorWall>(wallEntries, param);
                    entries.filter = request.filter;
                    entries.filterModel = request.filterModel;
                    model.WallEntries = entries;
                    return model;
                }
            }
            catch (Exception ex)
            {
                //LogsRepository.CreateLog(ex);
                return null;
            }
        }

        public ProcessResult Update(AuthorWall entity)
        {
            ProcessResult result = new ProcessResult();
            try
            {
                using (var con = GetConnection)
                {
                    bool res = con.Update(entity);
                    if (res == true)
                    {
                        result.ReturnID = entity.ID;
                        result.Message = "AuthorWall updated successfully.";
                        result.State = ProcessState.Success;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.State = ProcessState.Error;
            }
            return result;
        }
    }
}
