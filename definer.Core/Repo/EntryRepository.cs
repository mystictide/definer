﻿using Dapper;
using Dapper.Contrib.Extensions;
using definer.Core.Interface;
using definer.Entity.Helpers;
using definer.Entity.Threads;

namespace definer.Core.Repo
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

        public FilteredList<Entry> FilteredList(FilteredList<Entry> request)
        {
            try
            {
                FilteredList<Entry> result = new FilteredList<Entry>();
                DynamicParameters param = new DynamicParameters();
                param.Add("@Keyword", request.filter.Keyword);
                param.Add("@PageSize", request.filter.pageSize);
                param.Add("@ID", request.filterModel.ThreadID);

                string WhereClause = @" WHERE t.ThreadID = @ID AND  (t.Body like '%' + @Keyword + '%')";

                string query_count = $@"  Select Count(t.ID) from Entry t {WhereClause}";

                string query = $@"
                SELECT *
                ,(select Title from Thread where ID=@ID) Title
                ,(select Username from Users where ID=t.UserID) Author
                FROM Entry t 
                {WhereClause} 
                ORDER BY t.ID ASC 
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

                string WhereClause = @" WHERE t.ID = @ID";

                string query = $@"
                SELECT *
                ,(select Title from Thread where ID=t.ThreadID) Title
                ,(select Username from Users where ID=t.UserID) Author
                FROM Entry t 
                {WhereClause} ";

                using (var connection = GetConnection)
                {
                    return connection.Query<Entry>(query, param).FirstOrDefault();
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