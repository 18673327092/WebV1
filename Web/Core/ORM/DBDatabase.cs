using PetaPoco;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ORM
{
    public class DBDatabase : Database
    {
        public static DBDatabase CreateInstance(string connectionStringName = null)
        {
            return new DBDatabase(connectionStringName)
            {
                ForceDateTimesToUtc = true
            };
        }

        public DBDatabase(IDbConnection connection)
            : base(connection)
        {
        }

        public DBDatabase(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public DBDatabase(string connectionString, DbProviderFactory provider)
            : base(connectionString, provider)
        {
        }

        public DBDatabase(string connectionStringName)
            : base(connectionStringName)
        {
        }


        Regex rxSelect = new Regex(@"\A\s*(SELECT|EXECUTE|CALL)\s", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Multiline);
        Regex rxFrom = new Regex(@"\A\s*FROM\s", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Multiline);
        string AddSelectClause(string sql, PocoData pd)
        {
            if (sql.StartsWith(";"))
                return sql.Substring(1);

            if (!rxSelect.IsMatch(sql))
            {
                var tableName = EscapeTableName(pd.TableInfo.TableName);
                string cols = string.Join(", ", (from c in pd.QueryColumns select tableName + "." + EscapeSqlIdentifier(c)).ToArray());
                if (!rxFrom.IsMatch(sql))
                    sql = string.Format("SELECT {0} FROM {1} {2}", cols, tableName, sql);
                else
                    sql = string.Format("SELECT {0} {1}", cols, sql);
            }
            return sql;
        }

        public IEnumerable<object> Query(PocoData pd, Sql sql)
        {
            return Query(pd, sql.SQL, sql.Arguments);
        }
        // Return an enumerable collection of pocos
        public IEnumerable<object> Query(PocoData pd, string sql, params object[] args)
        {
            if (EnableAutoSelect)
                sql = AddSelectClause(sql, pd);

            OpenSharedConnection();
            try
            {
                using (var cmd = CreateCommand(this.Connection, sql, args))
                {
                    IDataReader r;
                    try
                    {
                        r = cmd.ExecuteReader();
                        OnExecutedCommand(cmd);
                    }
                    catch (Exception x)
                    {
                        OnException(x);
                        throw;
                    }
                    var factory = pd.GetFactory(cmd.CommandText, Connection.ConnectionString, ForceDateTimesToUtc, 0, r.FieldCount, r) as Func<IDataReader, object>;
                    using (r)
                    {
                        while (true)
                        {
                            object poco = Activator.CreateInstance(pd.type);
                            try
                            {
                                if (!r.Read())
                                    yield break;
                                poco = factory(r);
                            }
                            catch (Exception x)
                            {
                                OnException(x);
                                throw;
                            }

                            yield return poco;
                        }
                    }
                }
            }
            finally
            {
                CloseSharedConnection();
            }
        }

        public IPageOfItems<T> Page<T>(int Page, int PageSize, int? totalCount, string sql, params object[] args)
        {
            string sqlCount, sqlPage;
            BuildPageQueries<T>((Page - 1) * PageSize, PageSize, sql, ref args, out sqlCount, out sqlPage);

            // Save the one-time command time out and use it for both queries
            int saveTimeout = OneTimeCommandTimeout;

            // Setup the paged result
            var result = new PageOfItems<T>();
            result.Page = Page;
            result.PageSize = PageSize;
            result.Total = totalCount.HasValue ? totalCount.Value : ExecuteScalar<int>(sqlCount, args);

            OneTimeCommandTimeout = saveTimeout;
            if (result.Total > 0)
            {
                // Get the records
                result.AddRange(Fetch<T>(sqlPage, args));
            }
            // Done
            return result;
        }

        public PageOfDaTaSet DataSetPage(int Page, int PageSize, int? totalCount, string sql, params object[] args)
        {
            string sqlCount, sqlPage;
            DataSetBuildPageQueries((Page - 1) * PageSize, PageSize, sql, ref args, out sqlCount, out sqlPage);

            // Save the one-time command time out and use it for both queries
            int saveTimeout = OneTimeCommandTimeout;

            // Setup the paged result
            var result = new PageOfDaTaSet();
            result.Page = Page;
            result.PageSize = PageSize;
            result.Data = new DataSet();
            result.Total = totalCount.HasValue ? totalCount.Value : ExecuteScalar<int>(sqlCount, args);
            OneTimeCommandTimeout = saveTimeout;
            if (result.Total > 0)
            {
                Fill(result.Data, sqlPage, args);
                // Get the records
                //    result.AddRange(Fetch<T>(sqlPage, args));
            }
            // Done
            return result;
        }

        public PageOfDaTaSet DataSet( string sql, params object[] args)
        {
            // Save the one-time command time out and use it for both queries
            int saveTimeout = OneTimeCommandTimeout;
            // Setup the paged result
            var result = new PageOfDaTaSet();
            result.Data = new DataSet();
            OneTimeCommandTimeout = saveTimeout;
            Fill(result.Data, sql, args);
                // Get the records
                //    result.AddRange(Fetch<T>(sqlPage, args));
           
            // Done
            return result;
        }



        public IPageOfItems<T> Page<T>(int page, int itemsPerPage, Sql sql)
        {
            return Page<T>(page, itemsPerPage, null, sql.SQL, sql.Arguments);
        }

        public PageOfDaTaSet DataSetPage(int page, int itemsPerPage, Sql sql)
        {
            return DataSetPage(page, itemsPerPage, null, sql.SQL, sql.Arguments);
        }

        public IPageOfItems<T> Page<T>(int page, int itemsPerPage, int? totalCount, Sql sql)
        {
            return Page<T>(page, itemsPerPage, totalCount, sql.SQL, sql.Arguments);
        }
    }
}
