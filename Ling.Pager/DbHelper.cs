// Github: http://www.github.com/laufin
// Licensed: LGPL-2.1 <http://opensource.org/licenses/lgpl-2.1.php>
// Author: Laufin <laufin@qq.com>
// Copyright (c) 2012-2013 Laufin,all rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace Ling.Pager
{
    /// <summary>
    /// 数据库辅助分页类
    /// </summary>
    public class DbHelper
    {

        /// <summary>
        /// 分页函数
        /// </summary>
        /// <param name="conectionString">连接字符串</param>
        /// <param name="conectionString">连接字符串</param>
        /// <param name="sql">Sql语句</param>
        /// <param name="order">排序字段：ID ASC,Name DESC</param>
        /// <param name="pageIndex">分页索引</param>
        /// <param name="pageSize">分页方法</param>
        /// <param name="records">总记录数</param>
        /// <returns></returns>
        public static DataTable PageList(string conectionString, string sql, string order, int pageIndex, int pageSize, out int records)
        {
            return PageList(conectionString, sql, null, order, pageIndex, pageSize, out records, 0);
        }


        /// <summary>
        /// 分页函数
        /// </summary>
        /// <param name="conectionString">连接字符串</param>
        /// <param name="conectionString">连接字符串</param>
        /// <param name="sql">Sql语句</param>
        /// <param name="sqlParameters">参数</param>
        /// <param name="order">排序字段：ID ASC,Name DESC</param>
        /// <param name="pageIndex">分页索引</param>
        /// <param name="pageSize">分页方法</param>
        /// <param name="records">总记录数</param>
        /// <returns></returns>
        public static DataTable PageList(string conectionString, string sql, SqlParameter[] sqlParameters, string order, int pageIndex, int pageSize, out int records)
        {
            return PageList(conectionString, sql, sqlParameters, order, pageIndex, pageSize, out records, 0);
        }

        /// <summary>
        /// 分页函数
        /// </summary>
        /// <param name="conectionString">连接字符串</param>
        /// <param name="sql">Sql语句</param>
        /// <param name="sqlParameters">参数</param>
        /// <param name="order">排序字段：ID ASC,Name DESC</param>
        /// <param name="pageIndex">分页索引</param>
        /// <param name="pageSize">分页方法</param>
        /// <param name="records">总记录数</param>
        /// <param name="timeout">超时时间</param>
        /// <returns></returns>
        public static DataTable PageList(string conectionString, string sql, SqlParameter[] sqlParameters, string order, int pageIndex, int pageSize, out int records, int timeout)
        {
            records = 0;
            if (conectionString == null || conectionString.Length == 0)
            {
                return null;
            }

            DataTable dtList = null;
            if (pageIndex <= 0)
            {
                pageIndex = 1;
            }
            if (pageSize <= 0)
            {
                pageSize = 20;
            }

            StringBuilder tmpSql = new StringBuilder();
            //Table[0]总记录数
            tmpSql.Append(" SELECT COUNT(*) AS Records FROM  (" + sql + ") AS __VIRTUAL_TABLE0__; ");
            //Table[1]表数据
            if (!string.IsNullOrEmpty(order))
            {
                //构造排序并使用RowNumber进行排序
                tmpSql.Append(string.Format(@"
                          SELECT * FROM( 
	                            SELECT TOP {1} *, ROW_NUMBER() OVER(ORDER BY {0})  AS __VIRTUAL_NUMBER__ 
                                FROM ({2}) AS __VIRTUAL_TABLE1__
                                ORDER BY {0}
                            ) __VIRTUAL_TABLE2__
                            WHERE __VIRTUAL_NUMBER__ BETWEEN {3} AND {1}"
                    , order
                    , pageSize * pageIndex
                    , sql
                    , (pageIndex - 1) * pageSize + 1 //索引下标开始
                    ));

            }
            else
            {
                //构造排序并使用RowNumber进行排序
                tmpSql.Append(string.Format(@"
                            SELECT * FROM( 
	                            SELECT *, ROW_NUMBER() OVER(ORDER BY __VIRTUAL_FIELD__ ASC)  AS __VIRTUAL_NUMBER__ 
                                FROM (
		                            SELECT TOP {0} *, 0 AS __VIRTUAL_FIELD__ FROM ({1}) As __VIRTUAL_TABLE0__
	                            ) AS __VIRTUAL_TABLE1__
                            ) __VIRTUAL_TABLE2__
                            WHERE __VIRTUAL_NUMBER__ BETWEEN {2} AND {0}"
                    , pageSize * pageIndex
                    , sql
                    , (pageIndex - 1) * pageSize + 1 //索引下标开始
                    ));
            }

            DataSet dsList = null;
            using (SqlConnection sqlConn = new SqlConnection(conectionString))
            {
                sqlConn.Open();
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandText = tmpSql.ToString();
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.Connection = sqlConn;
                if (timeout > 0)
                {
                    sqlCmd.CommandTimeout = timeout;
                }
                if (sqlParameters != null)
                {
                    foreach (SqlParameter p in sqlParameters)
                    {
                        if (p != null)
                        {
                            if ((p.Direction == ParameterDirection.InputOutput ||
                                p.Direction == ParameterDirection.Input) &&
                                (p.Value == null))
                            {
                                p.Value = DBNull.Value;
                            }
                            sqlCmd.Parameters.Add(p);
                        }
                    }
                }
                using (SqlDataAdapter da = new SqlDataAdapter(sqlCmd))
                {
                    if (dsList == null)
                    {
                        dsList = new DataSet();
                    }
                    da.Fill(dsList);
                }
            }
            if (dsList == null || dsList.Tables.Count < 2)
            {
                return null;
            }
            //总记录数
            records = Convert.ToInt32(dsList.Tables[0].Rows[0][0]);
            //表数据
            dtList = dsList.Tables[1];
            if (dtList != null)
            {
                //如果没带排序字段，则做事后移除字段处理
                if (dtList.Columns.Contains("__VIRTUAL_FIELD__"))
                {
                    dtList.Columns.Remove("__VIRTUAL_FIELD__");
                }
                if (dtList.Columns.Contains("__VIRTUAL_NUMBER__"))
                {
                    dtList.Columns.Remove("__VIRTUAL_NUMBER__");
                }
            }

            return dtList;
        }
    }
}
