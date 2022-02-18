using Dapper;
using Dapper.Contrib.Extensions;
using definer.Core.Interface.User;
using definer.Entity.Helpers;
using definer.Entity.Users;

namespace definer.Core.Repo.User
{
    public class PreferenceJunctionRepository : Connection.dbConnection, IPreferenceJunction
    {
        public ProcessResult Add(PreferenceJunction entity)
        {
            ProcessResult result = new ProcessResult();
            try
            {
                using (var con = GetConnection)
                {
                    result.ReturnID = (int)con.Insert(entity);
                    result.Message = "Prefs saved successfully";
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

        public ProcessResult Delete(int ID)
        {
            using (var con = GetConnection)
            {
                ProcessResult pr = new ProcessResult();
                try
                {
                    con.Delete(new PreferenceJunction() { ID = ID });
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

        public FilteredList<PreferenceJunction> FilteredList(FilteredList<PreferenceJunction> request)
        {
            try
            {
                FilteredList<PreferenceJunction> result = new FilteredList<PreferenceJunction>();
                DynamicParameters param = new DynamicParameters();
                param.Add("@Keyword", request.filter.Keyword);
                param.Add("@PageSize", request.filter.pageSize);

                string WhereClause = @" WHERE t.ThreadID = @ID AND  (t.Body like '%' + @Keyword + '%')";

                string query_count = $@"  Select Count(t.ID) from PreferenceJunction t {WhereClause}";

                string query = $@"
                SELECT *
                ,(select Title from Thread where ID=@ID) Title
                ,(select Username from Users where ID=t.UserID) Author
                ,(select count(ID) from PreferenceJunctionAttribute where PreferenceJunctionID=@ID AND Vote=1) Upvotes
                ,(select count(ID) from PreferenceJunctionAttribute where PreferenceJunctionID=@ID AND Vote=0) Downvotes
                ,(select count(ID) from PreferenceJunctionAttribute where PreferenceJunctionID=@ID AND Favourite=1) Favourites
                FROM PreferenceJunction t
                {WhereClause} 
                ORDER BY t.ID ASC 
                OFFSET @StartIndex ROWS
                FETCH NEXT @PageSize ROWS ONLY";

                using (var connection = GetConnection)
                {
                    result.totalItems = connection.QueryFirstOrDefault<int>(query_count, param);
                    request.filter.pager = new Page(result.totalItems, request.filter.pageSize, request.filter.page);
                    param.Add("@StartIndex", request.filter.pager.StartIndex);
                    result.data = connection.Query<PreferenceJunction>(query, param);
                    result.filter = request.filter;
                    result.filterModel = request.filterModel;
                    return result;
                }
            }
            catch (Exception ex)
            {
                //LogsRepository.CreateLog(ex);
                return null;
            }
        }

        public PreferenceJunction Get(int ID)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@ID", ID);

                string WhereClause = @" WHERE t.UserID = @ID";

                string query = $@"
                SELECT *
                FROM PreferenceJunction t 
                {WhereClause} ";

                using (var connection = GetConnection)
                {
                    return connection.Query<PreferenceJunction>(query, param).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                //LogsRepository.CreateLog(ex);
                return null;
            }
        }

        public IEnumerable<PreferenceJunction> GetAll()
        {
            try
            {
                using (var con = GetConnection)
                {
                    return con.GetAll<PreferenceJunction>();
                }
            }
            catch (Exception ex)
            {
                //LogsRepository.CreateLog(ex);
                return null;
            }
        }

        public PreferenceJunction Manage(PreferenceJunction model)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@ID", model.ID);
                param.Add("@UserID", model.UserID);
                param.Add("@Messaging", model.Messaging);
                param.Add("@PageSize", model.PageSize);

                string query = $@"
                DECLARE  @result table(ID Int, UserID Int, Messaging bit, PageSize int)
                UPDATE PreferenceJunction
                SET Messaging = coalesce(@Messaging, Messaging),
                PageSize = coalesce(@PageSize, PageSize)
                OUTPUT INSERTED.* INTO @result
                WHERE UserID = @UserID                                   
                SELECT *
                FROM @result";

                using (var connection = GetConnection)
                {
                    return connection.Query<PreferenceJunction>(query, param).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                //LogsRepository.CreateLog(ex);
                return null;
            }
        }

        public ProcessResult Update(PreferenceJunction entity)
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
                        result.Message = "Prefs updated successfully.";
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
