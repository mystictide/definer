using Dapper;
using Dapper.Contrib.Extensions;
using definer.Core.Interface.User;
using definer.Entity.Helpers;
using definer.Entity.Users;

namespace definer.Core.Repo.User
{
    public class DMessagesJunctionRepository : Connection.dbConnection, IDMessagesJunction
    {
        public ProcessResult Add(DMessagesJunction entity)
        {
            ProcessResult result = new ProcessResult();
            try
            {
                using (var con = GetConnection)
                {
                    result.ReturnID = (int)con.Insert(entity);
                    result.Message = "DMessagesJunction saved successfully";
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
                    con.Delete(new DMessagesJunction() { ID = ID });
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

        public FilteredList<DMessagesJunction> FilteredList(FilteredList<DMessagesJunction> request)
        {
            try
            {
                FilteredList<DMessagesJunction> result = new FilteredList<DMessagesJunction>();
                DynamicParameters param = new DynamicParameters();
                param.Add("@Keyword", request.filter.Keyword);
                param.Add("@PageSize", request.filter.pageSize);

                string WhereClause = @" WHERE t.DMessagesJunctionID = @ID AND  (t.Body like '%' + @Keyword + '%')";

                string query_count = $@" Select Count(t.ID) from DMessagesJunction t {WhereClause}";

                string query = $@"
                SELECT *
                ,(select Username from Users where ID=t.UserID) Author
                FROM DMessagesJunction t
                {WhereClause} 
                ORDER BY t.ID ASC 
                OFFSET @StartIndex ROWS
                FETCH NEXT @PageSize ROWS ONLY";

                using (var connection = GetConnection)
                {
                    result.totalItems = connection.QueryFirstOrDefault<int>(query_count, param);
                    request.filter.pager = new Page(result.totalItems, request.filter.pageSize, request.filter.page);
                    param.Add("@StartIndex", request.filter.pager.StartIndex);
                    result.data = connection.Query<DMessagesJunction>(query, param);
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

        public DMessagesJunction Get(int ID)
        {
            try
            {
                using (var con = GetConnection)
                {
                    return con.Get<DMessagesJunction>(ID);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IEnumerable<DMessagesJunction> GetAll()
        {
            throw new NotImplementedException();
        }

        public DMessages GetDMs(FilteredList<DMessagesJunction> request, int ID, int CurrentUserID)
        {
            try
            {
                var DM = new DMessages();
                FilteredList<DMessagesJunction> result = new FilteredList<DMessagesJunction>();
                DynamicParameters param = new DynamicParameters();
                param.Add("@DMID", ID);
                param.Add("@PageSize", request.filter.pageSize);
                param.Add("@CurrentUserID", CurrentUserID);

                string DMQuery = $@"
                SELECT *
                ,(select Username from Users where ID=t.ReceiverID) Receiver
                ,(select Username from Users where ID=t.SenderID) Sender
                FROM DMessages t
                WHERE t.ID = @DMID";

                string WhereClause = @" WHERE t.DMID = @DMID";
                string query_count = $@"  Select Count(t.ID) from DMessagesJunction t {WhereClause}";
                string messagesQuery = $@"
                UPDATE DMessagesJunction SET IsRead = 1 WHERE DMID = @DMID AND UserID != @CurrentUserID
                SELECT *
                ,(select Username from Users where ID=t.UserID) Author
                FROM DMessagesJunction t
                {WhereClause} 
                ORDER BY t.ID ASC 
                OFFSET @StartIndex ROWS
                FETCH NEXT @PageSize ROWS ONLY";

                using (var connection = GetConnection)
                {
                    DM = connection.QueryFirstOrDefault<DMessages>(DMQuery, param);
                    result.totalItems = connection.QueryFirstOrDefault<int>(query_count, param);
                    request.filter.pager = new Page(result.totalItems, request.filter.pageSize, request.filter.page);
                    param.Add("@StartIndex", request.filter.pager.StartIndex);
                    result.data = connection.Query<DMessagesJunction>(messagesQuery, param);
                    result.filter = request.filter;
                    result.filterModel = request.filterModel;
                    DM.Messages = result;
                    return DM;
                }
            }
            catch (Exception ex)
            {
                //LogsRepository.CreateLog(ex);
                return null;
            }
        }

        public ProcessResult Update(DMessagesJunction entity)
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
                        result.Message = "DMessagesJunction updated successfully.";
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
