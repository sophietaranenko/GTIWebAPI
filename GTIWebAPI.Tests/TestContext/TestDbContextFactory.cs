using GTIWebAPI.Models.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Tests.TestContext
{
    public class TestDbContextFactory : IDbContextFactory
    {
        private TestDbContext testContext;
        public TestDbContextFactory()
        {
            testContext = new TestDbContext();
        }
        public IAppDbContext CreateDbContext()
        {
            return testContext;
        }
    }
}
