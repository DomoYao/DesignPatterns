using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/*
 实现方式（一）：实现 DatabaseConnectionPool 类。
 如果 Client 调用 ObjectPool 的 AcquireReusable() 方法来获取 Reusable 对象，当在 ObjectPool 中存在可用的 Reusable 对象时，其将一个 Reusable 从池中移除，然后返回该对象。如果池为空，则 ObjectPool 会创建一个新的 Reusable 对象。
 */
namespace DesignPatterns.Business.ObjectPool1
{

    public abstract class ObjectPool<T>
    {
        private readonly TimeSpan _expirationTime;
        private readonly Dictionary<T, DateTime> _unlocked;
        private readonly Dictionary<T, DateTime> _locked;
        private readonly object _sync = new object();

        protected ObjectPool()
        {
            _expirationTime = TimeSpan.FromSeconds(30);
            _locked = new Dictionary<T, DateTime>();
            _unlocked = new Dictionary<T, DateTime>();
        }

        public ObjectPool(TimeSpan expirationTime): this()
        {
            _expirationTime = expirationTime;
        }

        protected abstract T Create();

        public abstract bool Validate(T reusable);

        public abstract void Expire(T reusable);

        // 寻找可以使用的对象实例，如果unlocked对象中存在并没有失效则返回该对象，如果没有找到则创建新的实例并返回
        public T CheckOut()
        {
            lock (_sync)
            {
                T reusable = default(T);

                if (_unlocked.Count > 0)
                {
                    foreach (var item in _unlocked)
                    {
                        if ((DateTime.UtcNow - item.Value) > _expirationTime)
                        {
                            // object has expired
                            _unlocked.Remove(item.Key);
                            Expire(item.Key);
                        }
                        else
                        {
                            if (Validate(item.Key))
                            {
                                // find a reusable object
                                _unlocked.Remove(item.Key);
                                _locked.Add(item.Key, DateTime.UtcNow);
                                reusable = item.Key;
                                break;
                            }

                            // object failed validation
                            _unlocked.Remove(item.Key);
                            Expire(item.Key);
                        }
                    }
                }

                // no object available, create a new one
                if (reusable == null)
                {
                    reusable = Create();
                    _locked.Add(reusable, DateTime.UtcNow);
                }

                return reusable;
            }
        }

        // 释放刚刚利用的对象实例，供其他对象使用(即，从locked对象移除掉，重新加回到unlocked的集合中，这样后来对象就可以使用该对象.)
        public void CheckIn(T reusable)
        {
            lock (_sync)
            {
                _locked.Remove(reusable);
                _unlocked.Add(reusable, DateTime.UtcNow);
            }
        }
    }

    public class DatabaseConnection : IDisposable
    {
        // do some heavy works
        public DatabaseConnection(string connectionString)
        {
        }

        public bool IsOpen { get; set; }

        // release something
        public void Dispose()
        {
        }
    }

    public class DatabaseConnectionPool : ObjectPool<DatabaseConnection>
    {
        private readonly string _connectionString;

        public DatabaseConnectionPool(string connectionString)
            : base(TimeSpan.FromMinutes(1))
        {
            this._connectionString = connectionString;
        }

        protected override DatabaseConnection Create()
        {
            return new DatabaseConnection(_connectionString);
        }

        public override void Expire(DatabaseConnection connection)
        {
            connection.Dispose();
        }

        public override bool Validate(DatabaseConnection connection)
        {
            return connection.IsOpen;
        }
    }

    public class Client
    {
        public static void TestCase1()
        {
            // Create the ConnectionPool:
            var pool = new DatabaseConnectionPool("Data Source=DENNIS;Initial Catalog=TESTDB;Integrated Security=True;");

            // Get a connection:
            DatabaseConnection connection = pool.CheckOut();

            // Use the connection

            // Return the connection:
            pool.CheckIn(connection);
        }
    }
}