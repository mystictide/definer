using Dapper;
using Dapper.Contrib.Extensions;
using definer.Core.Interface;
using definer.Entity.Helpers;
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
    }
}
