﻿using Dapper;
using Dapper.Contrib.Extensions;
using definer.Core.Interface;
using definer.Entity.Helpers;
using definer.Entity.Threads;

namespace definer.Core.Repo
{
    public class EntryAttributeRepository : Connection.dbConnection, IEntryAttribute
    {
        public EntryAttribute Insert(EntryAttribute entity)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@EntryID", entity.EntryID);
                param.Add("@UserID", entity.UserID);
                param.Add("@Vote", entity.Vote);
                param.Add("@Favourite", entity.Favourite);

                string query = $@"
                INSERT INTO EntryAttribute (EntryID, UserID, Vote, Favourite)
                VALUES (@EntryID, @UserID, @Vote, @Favourite)
                OUTPUT INSERTED.* ";

                using (var connection = GetConnection)
                {
                    return connection.Query<EntryAttribute>(query, param).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                //LogsRepository.CreateLog(ex);
                return null;
            }
        }

        public ProcessResult Add(EntryAttribute entity)
        {
            ProcessResult result = new ProcessResult();
            try
            {
                using (var con = GetConnection)
                {
                    result.ReturnID = (int)con.Insert(entity);
                    result.Message = "EntryAttribute saved successfully";
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
                    con.Delete(new EntryAttribute() { ID = ID });
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

        public FilteredList<EntryAttribute> FilteredList(FilteredList<EntryAttribute> request)
        {
            try
            {
                FilteredList<EntryAttribute> result = new FilteredList<EntryAttribute>();
                DynamicParameters param = new DynamicParameters();
                param.Add("@Keyword", request.filter.Keyword);
                param.Add("@PageSize", request.filter.pageSize);

                string query_count = $@"  Select Count(t.ID) from EntryAttribute t";

                string query = $@"
                SELECT *
                FROM EntryAttribute t 
                ORDER BY t.ID ASC 
                OFFSET @StartIndex ROWS
                FETCH NEXT @PageSize ROWS ONLY";

                using (var connection = GetConnection)
                {
                    result.totalItems = connection.QueryFirstOrDefault<int>(query_count, param);
                    request.filter.pager = new Page(result.totalItems, request.filter.pageSize, request.filter.page);
                    param.Add("@StartIndex", request.filter.pager.StartIndex);
                    result.data = connection.Query<EntryAttribute>(query, param);
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

        public EntryAttribute Get(int ID)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@ID", ID);

                string WhereClause = @" WHERE t.EntryID = @ID";

                string query = $@"
                SELECT *
                FROM EntryAttribute t 
                {WhereClause} ";

                using (var connection = GetConnection)
                {
                    return connection.Query<EntryAttribute>(query, param).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                //LogsRepository.CreateLog(ex);
                return null;
            }
        }

        public IEnumerable<EntryAttribute> GetAll()
        {
            try
            {
                using (var con = GetConnection)
                {
                    return con.GetAll<EntryAttribute>();
                }
            }
            catch (Exception ex)
            {
                //LogsRepository.CreateLog(ex);
                return null;
            }
        }

        public ProcessResult Update(EntryAttribute entity)
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
                        result.Message = "EntryAttribute updated successfully.";
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

        public EntryAttribute Fav(EntryAttribute entity)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@ID", entity.ID);
                param.Add("@EntryID", entity.EntryID);
                param.Add("@Favourite", entity.Favourite);
                param.Add("@UserID", entity.UserID);

                string query = $@"
                if exists(SELECT * from EntryAttribute where EntryID = @EntryID AND UserID = @UserID)            
                BEGIN            
                UPDATE EntryAttribute
                            SET Favourite = 
							CASE
								WHEN Favourite = 0 THEN 1
								WHEN Favourite = 1 THEN NULL
							END
                                OUTPUT INSERTED.*
                                WHERE EntryID = @EntryID AND UserID = @UserID;
                End                    
                else            
                begin  
                INSERT INTO EntryAttribute (EntryID, UserID, Favourite)
                                OUTPUT INSERTED.* 
                                VALUES (@EntryID, @UserID, 1)
                end
                Delete from EntryAttribute WHERE EntryID = @EntryID AND UserID = @UserID AND Vote IS NULL AND Favourite IS NULL";

                using (var connection = GetConnection)
                {
                    return connection.Query<EntryAttribute>(query, param).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                //LogsRepository.CreateLog(ex);
                return null;
            }
        }

        public EntryAttribute Vote(EntryAttribute entity, bool State)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@ID", entity.ID);
                param.Add("@EntryID", entity.EntryID);
                param.Add("@Vote", entity.Vote);
                param.Add("@UserID", entity.UserID);

                string query;

                if (State)
                {
                    query = $@"
                    if exists(SELECT * from EntryAttribute where EntryID = @EntryID AND UserID = @UserID)            
                    BEGIN            
                    UPDATE EntryAttribute
                            SET Vote = 
							CASE
								WHEN Vote = 2 THEN 1
								WHEN Vote = 1 THEN NULL
							END
                            OUTPUT INSERTED.* 
                            WHERE EntryID = @EntryID AND UserID = @UserID
                    End                    
                    else            
                    begin  
                    INSERT INTO EntryAttribute (EntryID, UserID, Vote)
                                    OUTPUT INSERTED.* 
                                    VALUES (@EntryID, @UserID, @Vote)
                    end
                    Delete from EntryAttribute WHERE EntryID = @EntryID AND UserID = @UserID AND Vote IS NULL AND Favourite IS NULL";
                }
                else
                {
                    query = $@"
                    if exists(SELECT * from EntryAttribute where EntryID = @EntryID AND UserID = @UserID)            
                    BEGIN            
                    UPDATE EntryAttribute
                            SET Vote = 
							CASE
								WHEN Vote = 1 THEN 2
								WHEN Vote = 2 THEN NULL
							END
                            OUTPUT INSERTED.* 
                            WHERE EntryID = @EntryID AND UserID = @UserID
                    End                    
                    else            
                    begin  
                    INSERT INTO EntryAttribute (EntryID, UserID, Vote)
                                    OUTPUT INSERTED.* 
                                    VALUES (@EntryID, @UserID, @Vote)
                    end
                    Delete from EntryAttribute WHERE EntryID = @EntryID AND UserID = @UserID AND Vote IS NULL AND Favourite IS NULL";
                }

                

                using (var connection = GetConnection)
                {
                    return connection.Query<EntryAttribute>(query, param).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                //LogsRepository.CreateLog(ex);
                return null;
            }
        }
    }
}
