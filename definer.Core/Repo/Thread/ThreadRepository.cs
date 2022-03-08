using Dapper;
using Dapper.Contrib.Extensions;
using definer.Core.Interface.Thread;
using definer.Entity.Helpers;
using definer.Entity.Threads;

namespace definer.Core.Repo.Thread
{
    public class ThreadsRepository : Connection.dbConnection, IThread
    {
        public ProcessResult Add(Threads entity)
        {
            ProcessResult result = new ProcessResult();
            try
            {
                using (var con = GetConnection)
                {
                    result.ReturnID = (int)con.Insert(entity);
                    result.Message = "Thread saved successfully";
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
                    con.Delete(new Threads() { ID = ID });
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

        public FilteredList<Threads> FilteredList(FilteredList<Threads> request)
        {
            try
            {
                FilteredList<Threads> result = new FilteredList<Threads>();
                DynamicParameters param = new DynamicParameters();
                param.Add("@Keyword", request.filter.Keyword);
                param.Add("@PageSize", request.filter.pageSize);
                string WhereClause = @" WHERE (t.Title like '%' + @Keyword + '%')";

                string query_count = $@"  Select Count(t.ID) from Thread t {WhereClause}";

                string query = $@"
                SELECT *
                ,(select count(ID) from Entry where ThreadID = t.ID AND IsActive = 1) Entries
                FROM Thread t
                {WhereClause} 
                ORDER BY t.ID ASC 
                OFFSET @StartIndex ROWS
                FETCH NEXT @PageSize ROWS ONLY";

                using (var connection = GetConnection)
                {
                    result.totalItems = connection.QueryFirstOrDefault<int>(query_count, param);
                    request.filter.pager = new Page(result.totalItems, request.filter.pageSize, request.filter.page);
                    param.Add("@StartIndex", request.filter.pager.StartIndex);
                    result.data = connection.Query<Threads>(query, param);
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

        public Threads Get(int ID)
        {
            try
            {
                using (var con = GetConnection)
                {
                    return con.Get<Threads>(ID);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IEnumerable<Threads> GetAll()
        {
            try
            {
                using (var con = GetConnection)
                {
                    return con.GetAll<Threads>();
                }
            }
            catch (Exception ex)
            {
                //LogsRepository.CreateLog(ex);
                return null;
            }
        }

        public Threads GetbyTitle(string Title)
        {
            try
            {
                Threads model = new Threads();
                DynamicParameters param = new DynamicParameters();
                param.Add("@Title", Title);

                string WhereClause = @" WHERE (t.Title like '%' + @Title + '%')";

                string query = $@"
                SELECT *
                FROM Thread t 
                {WhereClause}";

                using (var connection = GetConnection)
                {
                    model = connection.QueryFirstOrDefault<Threads>(query, param);
                    return model;
                }
            }
            catch (Exception ex)
            {
                //LogsRepository.CreateLog(ex);
                return null;
            }
        }

        public ProcessResult Update(Threads entity)
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
                        result.Message = "Thread updated successfully.";
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
