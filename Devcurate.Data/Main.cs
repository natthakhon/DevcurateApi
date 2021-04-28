using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Devcurate.Data
{
    public class Main
    {
        // transaction important here since it concerns multiple table manipulations
        public static async Task<long> Add(string value)
        {
            long result = 1;
            using (SqlConnection con = new SqlConnection(Constring.ConnectionString()))
            {
                await con.OpenAsync();
                
                SqlCommand command = con.CreateCommand();
                SqlTransaction transaction = con.BeginTransaction(IsolationLevel.ReadCommitted);
                command.Connection = con;
                command.Transaction = transaction;

                try
                {
                    string getDelIdquery = @"select top 1 deletedRefId from DeletedRefId";
                    command.CommandText = getDelIdquery;
                    object delIdObj = await command.ExecuteScalarAsync();
                    if (delIdObj != null)
                    {
                        result = long.Parse(delIdObj.ToString());
                        string delRefquery = String.Format("delete from DeletedRefId where deletedRefId = {0}",result);
                        command.CommandText = delRefquery;
                        await command.ExecuteNonQueryAsync();
                    }
                    else
                    {
                        string getcounter = @"select top 1 counter from Counter";
                        command.CommandText = getcounter;
                        object counterObj = await command.ExecuteScalarAsync();
                        if (counterObj != null)
                        {
                            result = long.Parse(counterObj.ToString());
                            string updatecount = String.Format(@"update Counter set counter = {0}", result + 1);
                            command.CommandText = updatecount;
                            await command.ExecuteNonQueryAsync();
                        }
                        else
                        {
                            string insertcount = String.Format(@"insert into Counter(counter) values ({0})", result + 1 );
                            command.CommandText = insertcount;
                            await command.ExecuteNonQueryAsync();
                        }
                    }
                    string insertMain = "insert into Main(RefId,Val) values (" + result + ",'" + value + "')";
                    command.CommandText = insertMain;
                    await command.ExecuteNonQueryAsync();

                    command.Dispose();
                    
                    transaction.Commit();

                    return result;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public static async Task Delete(long refid)
        {
            using (SqlConnection con = new SqlConnection(Constring.ConnectionString()))
            {
                await con.OpenAsync();

                SqlCommand command = con.CreateCommand();
                SqlTransaction transaction = con.BeginTransaction(IsolationLevel.ReadCommitted);
                command.Connection = con;
                command.Transaction = transaction;

                try
                {
                    string updateMaiquery = String.Format("update Main set IsActive = {0} where RefId = {1} and IsActive = {2}", 0,refid,1);
                    command.CommandText = updateMaiquery;
                    await command.ExecuteNonQueryAsync();

                    string insertDelRef = String.Format("insert into DeletedRefId(deletedRefId) values ({0})",refid);
                    command.CommandText = insertDelRef;
                    await command.ExecuteNonQueryAsync();

                    command.Dispose();

                    transaction.Commit();
                }
                catch(Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public static async Task<string> Get(long refid)
        {
            using (SqlConnection con = new SqlConnection(Constring.ConnectionString()))
            {
                await con.OpenAsync();

                SqlCommand command = con.CreateCommand();
                command.Connection = con;

                try
                {
                    string query = String.Format("select Val from Main where RefId = {0} and IsActive = 1", refid);
                    command.CommandText = query;
                    object queryObj = await command.ExecuteScalarAsync();
                    if (queryObj != null)
                    {
                        return queryObj.ToString();
                    }

                    return String.Empty;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
