using KafkaEfCore.DbContexts;
using KafkaEFCore.Producer.Tests.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafkaEFCore.Producer.Tests
{
    public class HelperTest
    {
        [Fact]
        public void GetAllTableTypeTest()
        {
            var service = HostServie.GetServices();
            service.AddDbContext<NoSqlAtmTargetContext>(option=> option.UseSqlServer(HostServie.ConnectionStringTarget));
            var provider = service.BuildServiceProvider();
            var dbcontext = provider.GetService<NoSqlAtmTargetContext>();
            var test = dbcontext.GetAllTableType();
        }

    }
}
