using KafkaEFCore.Producer.EntityTest;
using KafkaEFCore.Producer.Implementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafkaEFCore.Producer.Tests.DbContexts
{
    internal class KafkaDbContextTest : KafkaDbContextBase
    {
        public KafkaDbContextTest(DbContextOptions<KafkaDbContextTest> dbContextOptions) : base(dbContextOptions)
        {
            Database.EnsureCreated();
        }
        public DbSet<Student> Students { get; set; }
    }
}
