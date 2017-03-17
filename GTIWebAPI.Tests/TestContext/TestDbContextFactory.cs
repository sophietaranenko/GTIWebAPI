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
        private IAppDbContext testContext;

        //Sometimes we can test only repo
        public TestDbContextFactory()
        {
            testContext = new TestDbContext();
        }

        //sometimes we need a path straigth to data
        public TestDbContextFactory(IAppDbContext dbContext)
        {
            testContext = dbContext;
        }

        public IAppDbContext CreateDbContext()
        {
            return testContext;
        }
    }
}
