

using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Infrastructure.Interception;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interceptor
{
    public class EFCommandInterceptor : IDbCommandInterceptor
    {

        private readonly static Log4NetHelper logger = Log4NetHelper.Default;
        public void NonQueryExecuting(
            DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
            LogSQLInfo(command, interceptionContext);
        }

        public void NonQueryExecuted(
            DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
      
        }

        public void ReaderExecuting(
            DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
            LogSQLInfo(command, interceptionContext);
        }

        public void ReaderExecuted(
            DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
         
        }

        public void ScalarExecuting(
            DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
            LogSQLInfo(command, interceptionContext);
        }

        public void ScalarExecuted(
            DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
       
        }

        private void LogSQLInfo<TResult>(
            DbCommand command, DbCommandInterceptionContext<TResult> interceptionContext)
        {
            //打印SQL
             logger.WriteInfo($"SQLInfo(EF):{command.CommandText}");

            MySqlParameterCollection param = command.Parameters as MySqlParameterCollection;
            //有参数打印参数
            if (param.Count>0)
            {
                //StringBuilder 减少字符串占用
                StringBuilder paramStr = new StringBuilder("SQLParameter  ==>: ");
                //获取参数
                foreach (MySqlParameter item in param)
                {
                    paramStr.Append($"{item.ParameterName} = {item.Value}({item.DbType})  ");
                }
                paramStr.Append("");
                logger.WriteInfo(paramStr);
            }
            //有异常就打印异常
            if (interceptionContext.Exception!=null)
            {
             logger.WriteError($"出现异常:{interceptionContext.Exception}");
            }


        }
    }
}
