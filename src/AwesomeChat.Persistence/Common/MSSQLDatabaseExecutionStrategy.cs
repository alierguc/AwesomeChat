using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeChat.Persistence.Common
{
    internal class MSSQLDatabaseExecutionStrategy : ExecutionStrategy
    {
        public MSSQLDatabaseExecutionStrategy(DbContext context, int maxRetryCount, TimeSpan maxRetryDelay) : base(context, maxRetryCount, maxRetryDelay)
        {
        }

        /// <summary>
        /// Execution Strategy ayarla
        /// </summary>
        /// <param name="dependencies"></param>
        /// <param name="maxRetryCount"></param>
        /// <param name="maxRetryDelay"></param>
        public MSSQLDatabaseExecutionStrategy(ExecutionStrategyDependencies dependencies, int maxRetryCount, TimeSpan maxRetryDelay) : base(dependencies, maxRetryCount, maxRetryDelay)
        {
        }

        int connectionRetryCount = 0;
        protected override bool ShouldRetryOn(Exception exception)
        {
            Console.WriteLine($"Veri tabanı bağlantısı {++connectionRetryCount}. kez tekrardan deneniyor.");
            return true;
        }
    }
}
