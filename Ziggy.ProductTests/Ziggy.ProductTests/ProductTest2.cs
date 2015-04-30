using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;


namespace Ziggy.ProductTests
{
    public class ProductTest2Parameters: TestParameters
    {
        public int Z { get; set; }
        public bool HasDrag { get; set; }
    }
    public class ProductTest2Result : TestResult
    {
        public int ZDragged { get; set; }
    }
    [TestConfiguration(typeof(IProduct), typeof(ProductTest2Parameters), typeof(ProductTest2Result))]
    public class ProductTest2 : Test<IProduct, ProductTest2Parameters, ProductTest2Result>
    {
        protected override Task<ProductTest2Result> DoExecute(IProduct product, ProductTest2Parameters parameters, CancellationToken token = default(CancellationToken))
        {
            var result = new ProductTest2Result() { Passed = parameters.HasDrag ? parameters.Z > 0 : parameters.Z == 0, ZDragged = parameters.HasDrag ? parameters.Z * -1 : parameters.Z };
            return Task.FromResult(result);
        }
    }
}
