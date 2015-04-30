using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;


namespace Ziggy.ProductTests
{
    public class ProductTest1Parameters : TestParameters
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
    [TestConfiguration(typeof(IProduct), typeof(ProductTest1Parameters), typeof(TestResult))]
    public class ProductTest1 : Test<IProduct, ProductTest1Parameters, TestResult>
    {
        protected override Task<TestResult> DoExecute(IProduct product, ProductTest1Parameters parameters, CancellationToken token = default(CancellationToken))
        {
            TestResult result = new TestResult() { Passed = parameters.X >= parameters.Y };
            return Task.FromResult(result);
        }
    }
}
