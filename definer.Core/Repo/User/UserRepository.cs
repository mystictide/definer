using Dapper;
using Dapper.Contrib.Extensions;
using definer.Core.Interface.User;
using definer.Entity.Helpers;
using definer.Entity.Threads;
using definer.Entity.Users;

namespace definer.Core.Repo.User
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
                LEFT JOIN PreferenceJunction as j ON t.ID = j.UserID
                {WhereClause}";

                using (var connection = GetConnection)
                {
                    return connection.Query<Users, PreferenceJunction, Users>(query, (a, b) =>
                    {
                        a.UserSettings = b; return a;
                    }, param, splitOn: "ID").FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                //LogsRepository.CreateLog(ex);
                return null;
            }
        }

        public Users GetbyUsername(FilteredList<Entry> request, string Username, int CurrentUserID)
        {
            try
            {
                var model = new Users();
                FilteredList<Entry> entries = new FilteredList<Entry>();
                DynamicParameters param = new DynamicParameters();
                param.Add("@Username", Username);
                param.Add("@CurrentUserID", CurrentUserID);

                string WhereClause = @" WHERE (t.Username like '%' + @Username + '%')";
                string query_count = $@"  Select Count(t.ID) from Entry t WHERE t.UserID = @UserID AND t.IsActive = 1";

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
                LEFT JOIN EntryAttribute as j ON t.ID = j.EntryID AND j.UserID = @CurrentUserID
                WHERE t.UserID = @UserID AND t.IsActive = 1
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

        public Users Get(string Username, int CurrentUserID)
        {
            try
            {
                var model = new Users();
                DynamicParameters param = new DynamicParameters();
                param.Add("@Username", Username);
                param.Add("@CurrentUserID", CurrentUserID);

                string WhereClause = @" WHERE (t.Username like '%' + @Username + '%')";

                string userQuery = $@"
                SELECT t.*
                ,(select count(ID) from Entry where UserID=t.ID AND t.IsActive = 1) EntryCount
                ,(select count(ID) from FollowJunction where UserID=t.ID) FollowerCount
                ,(select count(ID) from FollowJunction where FollowerID=t.ID) FollowingCount
                ,j.*
                ,b.*
                ,s.*
                FROM Users t
                LEFT JOIN FollowJunction as j ON t.ID = j.UserID AND j.FollowerID = @CurrentUserID
                LEFT JOIN BlockJunction as b ON t.ID = b.UserID AND b.BlockerID = @CurrentUserID
                LEFT JOIN SocialJunction as s ON t.ID = s.UserID
                {WhereClause}";

                using (var connection = GetConnection)
                {
                    model = connection.Query<Users, FollowJunction, BlockJunction, SocialJunction, Users>(userQuery, (a, b, c, d) =>
                    {
                        a.Interactions = new Interactions();
                        a.Interactions.Follow = new FollowJunction();
                        a.Interactions.Blocked = new BlockJunction();
                        a.Social = new SocialJunction();
                        a.Interactions.Follow = b;
                        a.Interactions.Blocked = c;
                        a.Social = d;  return a;
                    }, param, splitOn: "ID").FirstOrDefault();
                    return model;
                }
            }
            catch (Exception ex)
            {
                //LogsRepository.CreateLog(ex);
                return null;
            }
        }

        public Users GetFavouritesbyUsername(FilteredList<Entry> request, string Username, int CurrentUserID)
        {
            try
            {
                var model = new Users();
                FilteredList<Entry> entries = new FilteredList<Entry>();
                DynamicParameters param = new DynamicParameters();
                param.Add("@Username", Username);
                param.Add("@CurrentUserID", CurrentUserID);

                string WhereClause = @" WHERE (t.Username like '%' + @Username + '%')";
                string query_count = $@" 
                Select Count(t.ID) from Entry t 
                LEFT JOIN EntryAttribute as j ON t.ID = j.EntryID
                WHERE j.UserID = @UserID AND j.Favourite = 1 AND IsActive = 1";

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
                LEFT JOIN EntryAttribute as j ON t.ID = j.EntryID AND j.UserID = @CurrentUserID
                WHERE j.UserID = @UserID AND j.Favourite = 1 AND IsActive = 1
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

        public string ManageBio(int ID, string? text)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@ID", ID);
                param.Add("@Bio", text);

                string query = $@"
                DECLARE  @result table(Bio nvarchar(250))
                UPDATE Users
                SET Bio = @Bio
                OUTPUT INSERTED.Bio INTO @result
                WHERE ID = @ID                                   
                SELECT *
                FROM @result";

                using (var connection = GetConnection)
                {
                    return connection.Query<string>(query, param).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                //LogsRepository.CreateLog(ex);
                return null;
            }
        }

        public string GetBio(int ID)
        {
            string query = @"Select Bio from Users where ID=@ID";
            DynamicParameters param = new DynamicParameters();
            param.Add("@ID", ID);
            using (var connection = GetConnection)
            {
                return connection.Query<string>(query, param).FirstOrDefault();
            }
        }

        public ProcessResult UpdateUsername(int ID, string Username)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@ID", ID);
                param.Add("@Username", Username);

                string query = $@"
                UPDATE Users
                SET Username = @Username
                WHERE ID = @ID";

                using (var connection = GetConnection)
                {
                    return connection.Query<ProcessResult>(query, param).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                //LogsRepository.CreateLog(ex);
                return null;
            }
        }

        public ProcessResult UpdatePassword(int ID, string Password)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@ID", ID);
                param.Add("@Password", Password);

                string query = $@"
                UPDATE Users
                SET Password = @Password
                WHERE ID = @ID";

                using (var connection = GetConnection)
                {
                    return connection.Query<ProcessResult>(query, param).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                //LogsRepository.CreateLog(ex);
                return null;
            }
        }

        public ProcessResult UpdateEmail(int ID, string Mail)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@ID", ID);
                param.Add("@Mail", Mail);

                string query = $@"
                UPDATE Users
                SET Mail = @Mail
                WHERE ID = @ID";

                using (var connection = GetConnection)
                {
                    return connection.Query<ProcessResult>(query, param).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                //LogsRepository.CreateLog(ex);
                return null;
            }
        }

        public ProcessResult DeactivateAccount(int ID)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@ID", ID);

                string query = $@"
                UPDATE Users
                SET IsActive = 0
                WHERE ID = @ID";

                using (var connection = GetConnection)
                {
                    return connection.Query<ProcessResult>(query, param).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                //LogsRepository.CreateLog(ex);
                return null;
            }
        }

        public Users GetEntryArchivebyUsername(FilteredList<Entry> request, string Username)
        {
            try
            {
                var model = new Users();
                FilteredList<Entry> entries = new FilteredList<Entry>();
                DynamicParameters param = new DynamicParameters();
                param.Add("@Username", Username);

                string WhereClause = @" WHERE (t.Username like '%' + @Username + '%')";
                string query_count = $@"  Select Count(t.ID) from Entry t WHERE t.UserID = @UserID AND t.IsActive = 0";

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
                LEFT JOIN EntryAttribute as j ON t.ID = j.EntryID
                WHERE t.UserID = @UserID AND t.IsActive = 0
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

        public List<Users> GetSearchResults(string Username)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@Username", Username);

                string WhereClause = @" WHERE (t.Username like '%' + @Username + '%')";

                string query = $@"
                SELECT TOP (10) ID, Username
                FROM Users t 
                {WhereClause}";

                using (var connection = GetConnection)
                {
                    return connection.Query<Users>(query, param).ToList();
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
