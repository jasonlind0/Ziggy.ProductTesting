using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Ziggy.ProductTests
{
    [TargetTest(typeof(ProductTest1))]
    public class ProductA : IProduct
    {
        public string Name
        {
            get { return "A"; }
        }
    }
    [TargetTest(typeof(ProductTest1))]
    [TargetTest(typeof(ProductTest2))]
    public class ProductB : IProduct
    {
        public string Name
        {
            get { return "B"; }
        }
    }
}
