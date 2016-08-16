using System;
using System.Collections.Generic;
using System.Data;
using Dapper;

namespace InvoiceCqrs.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbTransaction _DbTransaction;
        private bool _IsCommitted;
        private bool _IsRolledBack;

        public IDbConnection Connection { get; private set; }

        public UnitOfWork(IDbConnection dbConnection)
        {
            Connection = dbConnection;

            Connection.Open();
            _DbTransaction = Connection.BeginTransaction();
        }

        public void Commit()
        {
            if (_IsCommitted)
            {
                throw new InvalidOperationException("The Unit of Work has already been committed");
            }

            _DbTransaction.Commit();
            _IsCommitted = true;
        }

        public int Execute(string query, object param)
        {
            return Connection.Execute(query, param, _DbTransaction);
        }

        public IEnumerable<TResult> Query<TResult>(string query, object @params)
        {
            return Connection.Query<TResult>(query, @params, _DbTransaction);
        }

        public IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TReturn>(string query, Func<TFirst, TSecond, TThird, TReturn> map, object param = null)
        {
            return Connection.Query(query, map, param, _DbTransaction);
        }

        public IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TReturn>(string query, Func<TFirst, TSecond, TThird, TFourth, TReturn> map, object param = null)
        {
            return Connection.Query(query, map, param, _DbTransaction);
        }

        public SqlMapper.GridReader QueryMultiple(string query, object param = null)
        {
            // TODO: Need to get rid of this method if possible
            return Connection.QueryMultiple(query, param, _DbTransaction);
        }

        public void Rollback()
        {
            if (_IsRolledBack)
            {
                throw new InvalidOperationException("The Unit of Work has already been rolled back");    
            }

            if (_IsCommitted)
            {
                throw new InvalidOperationException("The Unit of Work has already been committed, unable to rollback");
            }

            _DbTransaction.Rollback();
            _IsRolledBack = true;
        }
    }
}
