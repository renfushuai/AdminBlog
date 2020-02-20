using System;
using Blog.Common.Helper;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;

namespace BlogAdmin.Extemsions
{
    public static class SqlSugarSetup
    {
       public static void AddSqlSugarSetup(this IServiceCollection services)
        {
            if (services==null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddScoped<ISqlSugarClient>(o =>
            {
                var connStr = Appsettings.app("MySqlConnection");
                var config = new ConnectionConfig()
                {
                    //ConfigId = m.ConnId.ObjToString().ToLower(),
                    ConnectionString = connStr,
                    DbType = DbType.MySql,
                    IsAutoCloseConnection = true,
                    IsShardSameThread = false,
                    AopEvents = new AopEvents
                    {
                        OnLogExecuting = (sql, p) =>
                        {
                            // 多库操作的话，此处暂时无效果，在另一个地方有效，具体请查看BaseRepository.cs
                        }
                    },
                    MoreSettings = new ConnMoreSettings()
                    {
                        IsAutoRemoveDataCache = true
                    }
                    //InitKeyType = InitKeyType.SystemTable
                };
                return new SqlSugarClient(config);

            });
        }
    }
}
