using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yape.Transactions.Application.Exeptions;

namespace Yape.Transactions.Test.Application.Exeptions
{
    public class CodeExceptions_Test
    {

        [Fact]
        public void CodeExceptions_Constants_ShouldHaveExpectedValues()
        {
            Assert.Equal("COD200", CodeExceptions.COD200);
            Assert.Equal("COD501", CodeExceptions.COD501);
            Assert.Equal("COD400", CodeExceptions.COD400);
            Assert.Equal("COD500", CodeExceptions.COD500);
            Assert.Equal("COD201", CodeExceptions.COD201);
            Assert.Equal("COD204", CodeExceptions.COD204);
            Assert.Equal("COD205", CodeExceptions.COD205);
            Assert.Equal("COD206", CodeExceptions.COD206);
            Assert.Equal("COD422", CodeExceptions.COD422);
        }
    }
}
