using Dapper;
using Dapper.Contrib.Extensions;
using definer.Core.Interface.Thread;
using definer.Entity.Helpers;
using definer.Entity.Threads;

namespace definer.Core.Repo.Thread
{
    public class EntryRepository : Connection.dbConnection, IEntry
    {
        public ProcessResult Add(Entry entity)
        {
            ProcessResult result = new ProcessResult();
            try
            {
                using (var con = GetConnection)
                {
                    result.ReturnID = (int)con.Insert(entity);
                    result.Message = "Entry saved successfully";
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

        public bool Archive(int ID, int State)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@ID", ID);
                param.Add("@State", State);
                string query = $@"
                UPDATE Entry
                SET IsActive = @State
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
		                        FROM Entry t
		                        WHERE ID = @EntryID AND  t.UserID = @UserID
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
                    con.Delete(new Entry() { ID = ID });
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

        public FilteredList<Entry> FilteredList(FilteredList<Entry> request, int UserID)
        {
            try
            {
                FilteredList<Entry> result = new FilteredList<Entry>();
                DynamicParameters param = new DynamicParameters();
                param.Add("@Keyword", request.filter.Keyword);
                //param.Add("@PageSize", request.filter.pageSize);
                param.Add("@ID", request.filterModel.ThreadID);
                param.Add("@UserID", UserID);

                string WhereClause = @" WHERE t.ThreadID = @ID AND t.IsActive = 1 
                AND NOT t.UserID in (select UserID from BlockJunction where BlockerID = @UserID)";

                string query_count = $@" 
                Select Count(t.ID)
                from Entry t 
                {WhereClause}";

                string query = $@"
                DECLARE @PageSize INT = (select coalesce(PageSize, 10) from PreferenceJunction WHERE UserID = @UserID)
                SELECT t.*
                ,(select Title from Thread where ID=@ID) Title
                ,(select Username from Users where ID=t.UserID) Author
                ,(select count(ID) from EntryAttribute where EntryID=t.ID AND Vote=1) Upvotes
                ,(select count(ID) from EntryAttribute where EntryID=t.ID AND Vote=0) Downvotes
                ,(select count(ID) from EntryAttribute where EntryID=t.ID AND Favourite=1) Favourites
                ,j.*
                FROM Entry t
                LEFT JOIN EntryAttribute as j ON t.ID = j.EntryID AND j.UserID = @UserID
                {WhereClause} 
                ORDER BY t.Date DESC 
                OFFSET @StartIndex ROWS
                FETCH NEXT @PageSize ROWS ONLY";

                using (var connection = GetConnection)
                {
                    result.totalItems = connection.QueryFirstOrDefault<int>(query_count, param);
                    request.filter.pager = new Page(result.totalItems, request.filter.pageSize, request.filter.page);
                    param.Add("@StartIndex", request.filter.pager.StartIndex);
                    result.data = connection.Query<Entry, EntryAttribute, Entry>(query, (a, s) => { a.Attributes = s; return a; }, param, splitOn: "ID");
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

        public FilteredList<Entry> FilteredList(FilteredList<Entry> request)
        {
            try
            {
                FilteredList<Entry> result = new FilteredList<Entry>();
                DynamicParameters param = new DynamicParameters();
                param.Add("@Keyword", request.filter.Keyword);
                //param.Add("@PageSize", request.filter.pageSize);
                param.Add("@ID", request.filterModel.ThreadID);

                string WhereClause = @" WHERE t.ThreadID = @ID AND t.IsActive = 1";

                string query_count = $@"  Select Count(t.ID) from Entry t {WhereClause}";

                string query = $@"
                DECLARE @PageSize INT = (select coalesce(PageSize, 10) from PreferenceJunction WHERE UserID = @UserID)
                SELECT *
                ,(select Title from Thread where ID=@ID) Title
                ,(select Username from Users where ID=t.UserID) Author
                ,(select count(ID) from EntryAttribute where EntryID=@ID AND Vote=1) Upvotes
                ,(select count(ID) from EntryAttribute where EntryID=@ID AND Vote=0) Downvotes
                ,(select count(ID) from EntryAttribute where EntryID=@ID AND Favourite=1) Favourites
                FROM Entry t
                {WhereClause} 
                ORDER BY t.Date DESC 
                OFFSET @StartIndex ROWS
                FETCH NEXT @PageSize ROWS ONLY";

                using (var connection = GetConnection)
                {
                    result.totalItems = connection.QueryFirstOrDefault<int>(query_count, param);
                    request.filter.pager = new Page(result.totalItems, request.filter.pageSize, request.filter.page);
                    param.Add("@StartIndex", request.filter.pager.StartIndex);
                    result.data = connection.Query<Entry>(query, param);
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

        public Entry Get(int ID)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@ID", ID);

                string WhereClause = @" WHERE t.ID = @ID AND IsActive = 1";

                string query = $@"
                SELECT t.*
                ,(select Title from Thread where ID=t.ThreadID) Title
                ,(select Username from Users where ID=t.UserID) Author
                ,(select count(ID) from EntryAttribute where EntryID=@ID AND Vote=1) Upvotes
                ,(select count(ID) from EntryAttribute where EntryID=@ID AND Vote=0) Downvotes
                ,(select count(ID) from EntryAttribute where EntryID=@ID AND Favourite=1) Favourites
                ,j.*
                FROM Entry t
                LEFT JOIN EntryAttribute as j ON t.ID = j.EntryID
                {WhereClause}  ";

                using (var connection = GetConnection)
                {
                    return connection.Query<Entry, EntryAttribute, Entry>(query, (a, s) => { a.Attributes = s; return a; }, param, splitOn: "ID").FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                //LogsRepository.CreateLog(ex);
                return null;
            }
        }

        public Entry Get(int ID, int UserID)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@ID", ID);
                param.Add("@UserID", UserID);

                string WhereClause = @" WHERE t.ID = @ID AND IsActive = 1";

                string query = $@"
                SELECT t.*
                ,(select Title from Thread where ID=t.ThreadID) Title
                ,(select Username from Users where ID=t.UserID) Author
                ,(select count(ID) from EntryAttribute where EntryID=@ID AND Vote=1) Upvotes
                ,(select count(ID) from EntryAttribute where EntryID=@ID AND Vote=0) Downvotes
                ,(select count(ID) from EntryAttribute where EntryID=@ID AND Favourite=1) Favourites
                ,j.*
                FROM Entry t
                LEFT JOIN EntryAttribute as j ON t.ID = j.EntryID
                {WhereClause}  ";

                using (var connection = GetConnection)
                {
                    return connection.Query<Entry, EntryAttribute, Entry>(query, (a, s) => { a.Attributes = s; return a; }, param, splitOn: "ID").FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                //LogsRepository.CreateLog(ex);
                return null;
            }
        }

        public IEnumerable<Entry> GetAll()
        {
            try
            {
                using (var con = GetConnection)
                {
                    return con.GetAll<Entry>();
                }
            }
            catch (Exception ex)
            {
                //LogsRepository.CreateLog(ex);
                return null;
            }
        }

        public IEnumerable<Entry> GetTopRandom(int? UserID = null)
        {
            try
            {
                FilteredList<Entry> result = new FilteredList<Entry>();
                DynamicParameters param = new DynamicParameters();
                param.Add("@UserID", UserID);

                string WhereClause = @" WHERE IsActive = 1 AND (select count(ID) from EntryAttribute where EntryID=t.ID AND Vote=1) >= 1 ";
                string query = $@"
                    SELECT TOP (15) t.*
                    ,(select Title from Thread where ID=t.ThreadID) Title
                    ,(select Username from Users where ID=t.UserID) Author
                    ,(select count(ID) from EntryAttribute where EntryID=t.ID AND Vote=1) Upvotes
                    ,(select count(ID) from EntryAttribute where EntryID=t.ID AND Vote=0) Downvotes
                    ,(select count(ID) from EntryAttribute where EntryID=t.ID AND Favourite=1) Favourites
                    ,j.*
                    FROM Entry t
                    LEFT JOIN EntryAttribute j ON t.ID = j.EntryID AND j.UserID = @UserID
                    {WhereClause}";

                using (var connection = GetConnection)
                {
                    return connection.Query<Entry, EntryAttribute, Entry>(query, (a, s) => { a.Attributes = s; return a; }, param, splitOn: "ID");
                }
            }
            catch (Exception ex)
            {
                //LogsRepository.CreateLog(ex);
                return null;
            }
        }

        public ProcessResult Update(Entry entity)
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
                        result.Message = "Entry updated successfully.";
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