using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Ziggy.ProductTests
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple=false)]
    public class TestConfigurationAttribute : Attribute
    {
        public Type ParameterType { get; private set; }
        public Type ResultType { get; private set; }
        public Type TargetType { get; private set; }
        public TestConfigurationAttribute(Type targetType, Type parameterType, Type resultType)
        {
            this.ParameterType = parameterType;
            this.ResultType = resultType;
            this.TargetType = targetType;
        }
    }
    [AttributeUsage(AttributeTargets.Class, AllowMultiple=true)]
    public class TargetTestAttribute : Attribute
    {
        public Type TestType { get; private set; }
        public TargetTestAttribute(Type testType)
        {
            this.TestType = testType;
        }
    }
}
