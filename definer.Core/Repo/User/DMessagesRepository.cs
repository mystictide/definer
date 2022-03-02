using Dapper;
using Dapper.Contrib.Extensions;
using definer.Core.Interface.User;
using definer.Entity.Helpers;
using definer.Entity.Users;

namespace definer.Core.Repo.User
{
    public class DMessagesRepository : Connection.dbConnection, IDMessages
    {
        public ProcessResult Add(DMessages entity)
        {
            ProcessResult result = new ProcessResult();
            try
            {
                using (var con = GetConnection)
                {
                    result.ReturnID = (int)con.Insert(entity);
                    result.Message = "DMessages saved successfully";
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

        public bool CheckDMOwner(int UserID)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@UserID", UserID);
                string query = $@"
                SELECT
	                CASE WHEN EXISTS 
	                    (
		                        SELECT
		                        t.*
		                        FROM DMessages t
		                        WHERE t.SenderID = @UserID OR t.ReceiverID = @UserID
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
                    con.Delete(new DMessages() { ID = ID });
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

        public FilteredList<DMessages> FilteredList(FilteredList<DMessages> request, int UserID)
        {
            try
            {
                FilteredList<DMessages> result = new FilteredList<DMessages>();
                DynamicParameters param = new DynamicParameters();
                param.Add("@PageSize", request.filter.pageSize);
                param.Add("@UserID", UserID);

                string WhereClause = @" WHERE NOT t.SenderID = b.UserID AND t.ReceiverID = @UserID OR t.SenderID = @UserID";

                string query_count = $@" Select Count(t.ID) from DMessages t LEFT JOIN BlockJunction as b ON @UserID = b.BlockerID {WhereClause}";

                string query = $@"
                SELECT
                t.*
                ,(select Username from Users where ID=t.ReceiverID) Receiver
                ,(select Username from Users where ID=t.SenderID) Sender 
                ,(select count(ID) from DMessagesJunction where DMID=t.ID) MessageCount
                ,j.*
                ,(select Username from Users where ID=j.UserID) LastReplier
                FROM DMessages t
                CROSS APPLY (SELECT TOP 1 * FROM DMessagesJunction WHERE DMID = t.ID ORDER BY Date DESC) j
                LEFT JOIN BlockJunction as b ON @UserID = b.BlockerID
                {WhereClause} 
                ORDER BY t.ID ASC 
                OFFSET @StartIndex ROWS
                FETCH NEXT @PageSize ROWS ONLY";

                using (var connection = GetConnection)
                {
                    result.totalItems = connection.QueryFirstOrDefault<int>(query_count, param);
                    request.filter.pager = new Page(result.totalItems, request.filter.pageSize, request.filter.page);
                    param.Add("@StartIndex", request.filter.pager.StartIndex);
                    result.data = connection.Query<DMessages, DMessagesJunction, DMessages>(query, (a, b) =>
                    {
                        a.LastMessage = b; return a;
                    }, param, splitOn: "ID");
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

        public FilteredList<DMessages> FilteredList(FilteredList<DMessages> request)
        {
            throw new NotImplementedException();
        }

        public DMessages Get(int ID)
        {
            try
            {
                using (var con = GetConnection)
                {
                    return con.Get<DMessages>(ID);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IEnumerable<DMessages> GetAll()
        {
            throw new NotImplementedException();
        }

        public bool UnreadMessages(int UserID)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@UserID", UserID);
                string query = $@"
                SELECT
	                CASE WHEN EXISTS 
	                    (
		                        SELECT
		                        t.*
		                        ,j.*
		                        FROM DMessages t
		                        CROSS APPLY (SELECT * FROM DMessagesJunction WHERE DMID = t.ID AND UserID != @UserID AND IsRead = 0) j
                                LEFT JOIN BlockJunction as b ON @UserID = b.BlockerID
		                        WHERE NOT t.SenderID = b.UserID AND t.ReceiverID = @UserID OR t.SenderID = @UserID
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

        public ProcessResult Update(DMessages entity)
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
                        result.Message = "DMessages updated successfully.";
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
