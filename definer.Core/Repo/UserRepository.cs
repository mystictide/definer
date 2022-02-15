using Dapper;
using Dapper.Contrib.Extensions;
using definer.Core.Interface;
using definer.Entity.Helpers;
using definer.Entity.Threads;
using definer.Entity.Users;

namespace definer.Core.Repo
{
    public class UserRepository : Connection.dbConnection, IUsers
    {
        public bool CheckMail(string Mail)
        {
            string Query = @"Select * from Users where Mail=@Mail";
            DynamicParameters p = new DynamicParameters();
            p.Add("@Mail", Mail);
            using (var con = GetConnection)
            {
                return !(con.Query<Users>(Query, p).Count() > 0);
            }
        }
        public bool CheckUsername(string Name)
        {
            string Query = @"Select * from Users where Username=@Username";
            DynamicParameters p = new DynamicParameters();
            p.Add("@Username", Name);
            using (var con = GetConnection)
            {
                return !(con.Query<Users>(Query, p).Count() > 0);
            }
        }

        public ProcessResult Add(Users entity)
        {
            ProcessResult result = new ProcessResult();
            try
            {
                using (var con = GetConnection)
                {
                    result.ReturnID = (int)con.Insert(entity);
                    result.Message = "User saved successfully";
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
                    con.Delete(new Users() { ID = ID });
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

        public Users Get(int ID)
        {
            try
            {
                using (var con = GetConnection)
                {
                    return con.Get<Users>(ID);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ProcessResult Update(Users entity)
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
                        result.Message = "User updated successfully.";
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

        public Users Login(string Mail)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@Mail", Mail);

                string WhereClause = @" WHERE (t.Mail like '%' + @Mail + '%')";

                string query = $@"
                SELECT *
                FROM Users t 
                {WhereClause}";

                using (var connection = GetConnection)
                {
                    return connection.QueryFirstOrDefault<Users>(query, param);
                }
            }
            catch (Exception ex)
            {
                //LogsRepository.CreateLog(ex);
                return null;
            }
        }

        public Users GetbyUsername(FilteredList<Entry> request, string Username)
        {
            try
            {
                var model = new Users();
                FilteredList<Entry> entries = new FilteredList<Entry>();
                DynamicParameters param = new DynamicParameters();
                param.Add("@Username", Username);

                string WhereClause = @" WHERE (t.Username like '%' + @Username + '%')";
                string query_count = $@"  Select Count(t.ID) from Entry t WHERE t.UserID = @UserID";

                string userQuery = $@"
                SELECT t.*
                FROM Users t
                {WhereClause}";

                string userEntries = $@"
                SELECT t.*
                ,(select Title from Thread where ID=t.ThreadID) Title
                ,(select Username from Users where ID=t.UserID) Author
                ,(select count(ID) from EntryAttribute where EntryID=t.ID AND Vote=1) Upvotes
                ,(select count(ID) from EntryAttribute where EntryID=t.ID AND Vote=0) Downvotes
                ,(select count(ID) from EntryAttribute where EntryID=t.ID AND Favourite=1) Favourites
                ,j.*
                FROM Entry t
                LEFT JOIN EntryAttribute as j ON t.ID = j.EntryID AND j.UserID = t.UserID
                WHERE t.UserID = @UserID
                ORDER BY t.ID ASC 
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
                    entries.data = connection.Query<Entry, EntryAttribute, Entry>(userEntries, (a, s) => { a.Attributes = s; return a; }, param, splitOn: "ID");
                    entries.filter = request.filter;
                    entries.filterModel = request.filterModel;
                    model.Entries = entries;
                    return model;
                }
            }
            catch (Exception ex)
            {
                //LogsRepository.CreateLog(ex);
                return null;
            }
        }

        public Users Get(string Username)
        {
            try
            {
                var model = new Users();
                DynamicParameters param = new DynamicParameters();
                param.Add("@Username", Username);

                string WhereClause = @" WHERE (t.Username like '%' + @Username + '%')";

                string userQuery = $@"
                SELECT t.*
                FROM Users t
                {WhereClause}";

                using (var connection = GetConnection)
                {
                    model = connection.Query<Users>(userQuery, param).FirstOrDefault();
                    return model;
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
