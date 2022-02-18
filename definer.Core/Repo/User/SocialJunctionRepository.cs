using Dapper;
using Dapper.Contrib.Extensions;
using definer.Core.Interface.User;
using definer.Entity.Helpers;
using definer.Entity.Users;

namespace definer.Core.Repo.User
{
    public class SocialJunctionRepository : Connection.dbConnection, ISocialJunction
    {
        public ProcessResult Add(SocialJunction entity)
        {
            ProcessResult result = new ProcessResult();
            try
            {
                using (var con = GetConnection)
                {
                    result.ReturnID = (int)con.Insert(entity);
                    result.Message = "Socials saved successfully";
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
                    con.Delete(new SocialJunction() { ID = ID });
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

        public FilteredList<SocialJunction> FilteredList(FilteredList<SocialJunction> request)
        {
            try
            {
                FilteredList<SocialJunction> result = new FilteredList<SocialJunction>();
                DynamicParameters param = new DynamicParameters();
                param.Add("@Keyword", request.filter.Keyword);
                param.Add("@PageSize", request.filter.pageSize);

                string WhereClause = @" WHERE t.ThreadID = @ID AND  (t.Body like '%' + @Keyword + '%')";

                string query_count = $@"  Select Count(t.ID) from SocialJunction t {WhereClause}";

                string query = $@"
                SELECT *
                ,(select Title from Thread where ID=@ID) Title
                ,(select Username from Users where ID=t.UserID) Author
                ,(select count(ID) from SocialJunctionAttribute where SocialJunctionID=@ID AND Vote=1) Upvotes
                ,(select count(ID) from SocialJunctionAttribute where SocialJunctionID=@ID AND Vote=0) Downvotes
                ,(select count(ID) from SocialJunctionAttribute where SocialJunctionID=@ID AND Favourite=1) Favourites
                FROM SocialJunction t
                {WhereClause} 
                ORDER BY t.ID ASC 
                OFFSET @StartIndex ROWS
                FETCH NEXT @PageSize ROWS ONLY";

                using (var connection = GetConnection)
                {
                    result.totalItems = connection.QueryFirstOrDefault<int>(query_count, param);
                    request.filter.pager = new Page(result.totalItems, request.filter.pageSize, request.filter.page);
                    param.Add("@StartIndex", request.filter.pager.StartIndex);
                    result.data = connection.Query<SocialJunction>(query, param);
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

        public SocialJunction Get(int ID)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@ID", ID);

                string WhereClause = @" WHERE t.UserID = @ID";

                string query = $@"
                SELECT *
                FROM SocialJunction t 
                {WhereClause} ";

                using (var connection = GetConnection)
                {
                    return connection.Query<SocialJunction>(query, param).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                //LogsRepository.CreateLog(ex);
                return null;
            }
        }

        public IEnumerable<SocialJunction> GetAll()
        {
            try
            {
                using (var con = GetConnection)
                {
                    return con.GetAll<SocialJunction>();
                }
            }
            catch (Exception ex)
            {
                //LogsRepository.CreateLog(ex);
                return null;
            }
        }

        public SocialJunction Manage(SocialJunction model)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@ID", model.ID);
                param.Add("@UserID", model.UserID);
                param.Add("@Twitter", model.Twitter);
                param.Add("@Instagram", model.Instagram);
                param.Add("@Facebook", model.Facebook);
                param.Add("@LinkedIn", model.LinkedIn);
                param.Add("@YouTube", model.YouTube);
                param.Add("@Spotify", model.Spotify);
                param.Add("@Letterboxd", model.Letterboxd);
                param.Add("@GitHub", model.GitHub);

                string query = $@"
                DECLARE  @result table(ID Int, UserID Int, Twitter nvarchar(MAX), Instagram nvarchar(MAX), Facebook nvarchar(MAX), LinkedIn nvarchar(MAX),
                YouTube nvarchar(MAX), Spotify nvarchar(MAX), Letterboxd nvarchar(MAX), GitHub nvarchar(MAX))
                UPDATE SocialJunction
                SET Twitter = @Twitter,
                Instagram = @Instagram,
                Facebook = @Facebook,
                LinkedIn = @LinkedIn,
                YouTube = @YouTube,
                Spotify = @Spotify,
                Letterboxd = @Letterboxd,
                GitHub = @GitHub
                OUTPUT INSERTED.* INTO @result
                WHERE UserID = @UserID                                   
                SELECT *
                FROM @result";

                using (var connection = GetConnection)
                {
                    return connection.Query<SocialJunction>(query, param).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                //LogsRepository.CreateLog(ex);
                return null;
            }
        }

        public ProcessResult Update(SocialJunction entity)
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
                        result.Message = "Socials updated successfully.";
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
