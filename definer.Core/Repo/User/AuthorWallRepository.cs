﻿using Dapper;
using Dapper.Contrib.Extensions;
using definer.Core.Interface.User;
using definer.Entity.Helpers;
using definer.Entity.Users;

namespace definer.Core.Repo.User
{
    public class AuthorWallRepository : Connection.dbConnection, IAuthorWall
    {
        public ProcessResult Add(AuthorWall entity)
        {
            ProcessResult result = new ProcessResult();
            try
            {
                using (var con = GetConnection)
                {
                    result.ReturnID = (int)con.Insert(entity);
                    result.Message = "AuthorWall saved successfully";
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
                    con.Delete(new AuthorWall() { ID = ID });
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

        public FilteredList<AuthorWall> FilteredList(FilteredList<AuthorWall> request)
        {
            try
            {
                FilteredList<AuthorWall> result = new FilteredList<AuthorWall>();
                DynamicParameters param = new DynamicParameters();
                param.Add("@Keyword", request.filter.Keyword);
                param.Add("@PageSize", request.filter.pageSize);

                string WhereClause = @" WHERE t.AuthorWallID = @ID AND  (t.Body like '%' + @Keyword + '%')";

                string query_count = $@"  Select Count(t.ID) from AuthorWall t {WhereClause}";

                string query = $@"
                SELECT *
                ,(select Title from AuthorWall where ID=@ID) Title
                ,(select Username from Users where ID=t.UserID) Author
                ,(select count(ID) from AuthorWallAttribute where AuthorWallID=@ID AND Vote=1) Upvotes
                ,(select count(ID) from AuthorWallAttribute where AuthorWallID=@ID AND Vote=0) Downvotes
                ,(select count(ID) from AuthorWallAttribute where AuthorWallID=@ID AND Favourite=1) Favourites
                FROM AuthorWall t
                {WhereClause} 
                ORDER BY t.ID ASC 
                OFFSET @StartIndex ROWS
                FETCH NEXT @PageSize ROWS ONLY";

                using (var connection = GetConnection)
                {
                    result.totalItems = connection.QueryFirstOrDefault<int>(query_count, param);
                    request.filter.pager = new Page(result.totalItems, request.filter.pageSize, request.filter.page);
                    param.Add("@StartIndex", request.filter.pager.StartIndex);
                    result.data = connection.Query<AuthorWall>(query, param);
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

        public AuthorWall Get(int ID)
        {
            try
            {
                using (var con = GetConnection)
                {
                    return con.Get<AuthorWall>(ID);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IEnumerable<AuthorWall> GetAll()
        {
            throw new NotImplementedException();
        }

        public Users GetbyUsername(FilteredList<AuthorWall> request, string Username)
        {
            try
            {
                var model = new Users();
                FilteredList<AuthorWall> entries = new FilteredList<AuthorWall>();
                DynamicParameters param = new DynamicParameters();
                param.Add("@Username", Username);

                string WhereClause = @" WHERE (t.Username like '%' + @Username + '%')";
                string query_count = $@"  Select Count(t.ID) from AuthorWall t WHERE t.UserID = @UserID";

                string userQuery = $@"
                SELECT t.*
                FROM Users t
                {WhereClause}";

                string wallEntries = $@"
                SELECT t.*
                ,(select Username from Users where ID=t.SenderID) Author
                FROM AuthorWall t
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
                    entries.data = connection.Query<AuthorWall>(wallEntries, param);
                    entries.filter = request.filter;
                    entries.filterModel = request.filterModel;
                    model.WallEntries = entries;
                    return model;
                }
            }
            catch (Exception ex)
            {
                //LogsRepository.CreateLog(ex);
                return null;
            }
        }

        public ProcessResult Update(AuthorWall entity)
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
                        result.Message = "AuthorWall updated successfully.";
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