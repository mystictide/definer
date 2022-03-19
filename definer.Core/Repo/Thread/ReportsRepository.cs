using Dapper;
using Dapper.Contrib.Extensions;
using definer.Core.Interface.Thread;
using definer.Entity.Helpers;
using definer.Entity.Threads;

namespace definer.Core.Repo.Thread
{
    public class ReportsRepository : Connection.dbConnection, IReports
    {
        public ProcessResult Add(Reports entity)
        {
            ProcessResult result = new ProcessResult();
            try
            {
                using (var con = GetConnection)
                {
                    result.ReturnID = (int)con.Insert(entity);
                    result.Message = "Reports saved successfully";
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
                    con.Delete(new Reports() { ID = ID });
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

        public FilteredList<Reports> FilteredList(FilteredList<Reports> request)
        {
            try
            {
                FilteredList<Reports> result = new FilteredList<Reports>();
                DynamicParameters param = new DynamicParameters();
                param.Add("@Keyword", request.filter.Keyword);
                //param.Add("@PageSize", request.filter.pageSize);

                string WhereClause = @"WHERE IsActive = 1";

                string query_count = $@" Select Count(t.ID) from Reports t {WhereClause}";

                string query = $@"
                DECLARE @PageSize INT = (select coalesce(PageSize, 10) from PreferenceJunction WHERE UserID = @UserID)
                SELECT t.*
                ,(select Title from Thread where ID in (select ThreadID from Entry where ID = t.EntryID)) Thread
                ,(select Username from Users where ID in (select UserID from Entry where ID = t.EntryID)) Author
                ,(select Username from Users where ID=t.UserID) Reporter
                FROM Reports t
                {WhereClause} 
                ORDER BY t.Date DESC
                OFFSET @StartIndex ROWS
                FETCH NEXT @PageSize ROWS ONLY";

                using (var connection = GetConnection)
                {
                    result.totalItems = connection.QueryFirstOrDefault<int>(query_count, param);
                    request.filter.pager = new Page(result.totalItems, request.filter.pageSize, request.filter.page);
                    param.Add("@StartIndex", request.filter.pager.StartIndex);
                    result.data = connection.Query<Reports>(query, param);
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

        public FilteredList<Reports> FilteredList(FilteredList<Reports> request, int UserID)
        {
            try
            {
                FilteredList<Reports> result = new FilteredList<Reports>();
                DynamicParameters param = new DynamicParameters();
                param.Add("@Keyword", request.filter.Keyword);
                param.Add("@UserID", UserID);
                //param.Add("@PageSize", request.filter.pageSize);

                string WhereClause = @"WHERE IsActive = 1";

                string query_count = $@" Select Count(t.ID) from Reports t {WhereClause}";

                string query = $@"
                DECLARE @PageSize INT = (select coalesce(PageSize, 10) from PreferenceJunction WHERE UserID = @UserID)
                SELECT t.*
                ,(select Title from Thread where ID in (select ThreadID from Entry where ID = t.EntryID)) Thread
                ,(select Username from Users where ID in (select UserID from Entry where ID = t.EntryID)) Author
                ,(select Username from Users where ID=t.UserID) Reporter
                FROM Reports t
                {WhereClause} 
                ORDER BY t.Date DESC
                OFFSET @StartIndex ROWS
                FETCH NEXT @PageSize ROWS ONLY";

                using (var connection = GetConnection)
                {
                    result.totalItems = connection.QueryFirstOrDefault<int>(query_count, param);
                    request.filter.pager = new Page(result.totalItems, request.filter.pageSize, request.filter.page);
                    param.Add("@StartIndex", request.filter.pager.StartIndex);
                    result.data = connection.Query<Reports>(query, param);
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

        public Reports Get(int ID)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@ID", ID);

                string WhereClause = @"WHERE t.ID = @ID";

                string query = $@"
                SELECT t.*
                ,(select Title from Thread where ID in (select ThreadID from Entry where ID = t.EntryID)) Thread
                ,(select Username from Users where ID in (select UserID from Entry where ID = t.EntryID)) Author
                ,(select Username from Users where ID=t.UserID) Reporter
                FROM Reports t
                {WhereClause}  ";

                using (var connection = GetConnection)
                {
                    return connection.Query<Reports>(query, param).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                //LogsRepository.CreateLog(ex);
                return null;
            }
        }

        public IEnumerable<Reports> GetAll()
        {
            try
            {
                using (var con = GetConnection)
                {
                    return con.GetAll<Reports>();
                }
            }
            catch (Exception ex)
            {
                //LogsRepository.CreateLog(ex);
                return null;
            }
        }

        public ProcessResult Update(Reports entity)
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
                        result.Message = "Reports updated successfully.";
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
