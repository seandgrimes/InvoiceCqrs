using System;
using System.Collections.Generic;
using Dapper;

namespace InvoiceCqrs.Persistence
{
    public interface IUnitOfWork
    {
        void Commit();

        int Execute(string query, object param);

        IEnumerable<TResult> Query<TResult>(string query, object param = null);

        IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TReturn>(string query,
            Func<TFirst, TSecond, TThird, TReturn> map, object param = null);

        IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TReturn>(string query,
            Func<TFirst, TSecond, TThird, TFourth, TReturn> map, object param = null);

        SqlMapper.GridReader QueryMultiple(string query, object param = null);

        void Rollback();
    }
}
