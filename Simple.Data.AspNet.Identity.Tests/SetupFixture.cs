using System.Diagnostics;
using NUnit.Framework;

namespace Simple.Data.AspNet.Identity.Tests {

    [SetUpFixture]
    public class SetupFixture {
        private DatabaseSupport _dbSupport;

        public SetupFixture() {
            _dbSupport = new DatabaseSupport(DatabaseHelper.ConnectionString);
        }

        [SetUp]
        public void CreateStoredProcedures()
        {
            _dbSupport.DropDB("IdentityTest");
            _dbSupport.CreateDB("IdentityTest");
            _dbSupport.RunScript(@"..\..\Database.txt");
            Trace.Write("created database");
        }
    }
}