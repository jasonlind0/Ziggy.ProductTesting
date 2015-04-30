using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Mvvm;
using Ziggy.ProductTests;

namespace Ziggy.ViewModels
{
    public class TestResultViewModel : BindableBase
    {
        public bool Passed { get; private set; }
        protected IBindingOperations BindingOperations { get; private set; }
        public TestResultViewModel(IBindingOperations bindingOperations, TestResult result)
        {
            this.BindingOperations = bindingOperations;
            this.Passed = result.Passed;
        }
    }
    public class ProductTest2ResultViewModel : TestResultViewModel
    {
        public int ZDragged { get; private set; }
        public ProductTest2ResultViewModel(IBindingOperations bindingOperations, ProductTest2Result result)
            : base(bindingOperations, result)
        {
            this.ZDragged = result.ZDragged;
        }
    }

    public static class TestResultViewModelFactory
    {
        private static Func<Type, TestResult, TestResultViewModel> Factory { get; set; }
        public static void Initialize(Func<Type, TestResult, TestResultViewModel> testResultViewModelFactory)
        {
            Factory = testResultViewModelFactory;
        }
        public static TestResultViewModel Create(Type resultType, TestResult result)
        {
            return Factory(resultType, result);
        }
    }
}
