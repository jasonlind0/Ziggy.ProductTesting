using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Mvvm;
using Ziggy.ProductTests;
using System.Reflection;
using System.Threading;

namespace Ziggy.ViewModels
{
    public class TestViewModel : BindableBase
    {
        public Type TestType { get; private set; }
        public TestParameterViewModel Parameters { get; private set; }
        public object Target { get; private set; }
        protected TestConfigurationAttribute Config { get; private set; }
        
        private bool include;
        public bool Include
        {
            get { return this.include; }
            set { SetProperty(ref include, value); }
        }
        private int sortOrder;
        public int SortOrder
        {
            get { return this.sortOrder; }
            set { SetProperty(ref sortOrder, value); }
        }
        private TestResultViewModel result;
        public TestResultViewModel Result
        {
            get { return this.result; }
            set { SetProperty(ref result, value); }
        }
        public TestViewModel(Type testType, object target)
        {
            this.Target = target;
            this.TestType = testType;
            this.Config = testType.GetCustomAttribute<TestConfigurationAttribute>(true);
            if (this.Config == null)
                throw new ArgumentException("testType must have TestConfigurationAttribute");
            Parameters = TestParameterViewModelFactory.Create(this.Config.ParameterType);
        }
        public async Task Execute(CancellationToken token = default(CancellationToken))
        {
            try
            {
                var test = TestFactory.Create(this.TestType);
                var result = await test.Execute(this.Target, Parameters.GetTestParameters(), token);
                this.Result = TestResultViewModelFactory.Create(this.Config.ResultType, result);
            }
            catch (OperationCanceledException)
            {
                this.Result = null;
                throw;
            }
        }
    }
    public static class TestViewModelFactory
    {
        private static Func<Type, object, TestViewModel> Factory { get; set; }
        public static void Initialize(Func<Type, object, TestViewModel> testViewModelFactory)
        {
            Factory = testViewModelFactory;
        }
        public static TestViewModel Create(Type testType, object target)
        {
            return Factory(testType, target);
        }
    }
}
