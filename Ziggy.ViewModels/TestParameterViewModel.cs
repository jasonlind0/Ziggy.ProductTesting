using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Mvvm;
using Ziggy.ProductTests;

namespace Ziggy.ViewModels
{
    public abstract class TestParameterViewModel : BindableBase
    {
        protected IBindingOperations BindingOperations { get; private set; }
        public TestParameterViewModel(IBindingOperations bindingOperations)
        {
            this.BindingOperations = bindingOperations;
        }
        public abstract TestParameters GetTestParameters();
    }
    public class ProductTest1ParametersViewModel : TestParameterViewModel
    {
        public ProductTest1ParametersViewModel(IBindingOperations bindingOperations) : base(bindingOperations) { }

        private int x;
        public int X
        {
            get { return this.x; }
            set { SetProperty(ref x, value); }
        }
        private int y;
        public int Y
        {
            get { return this.y; }
            set { SetProperty(ref y, value); }
        }
        public override TestParameters GetTestParameters()
        {
            return new ProductTest1Parameters() { X = this.X, Y = this.Y };
        }
    }
    public class ProductTest2ParametersViewModel : TestParameterViewModel
    {
        public ProductTest2ParametersViewModel(IBindingOperations bindingOperations) : base(bindingOperations) { }
        private int z;
        public int Z
        {
            get { return this.z; }
            set { SetProperty(ref z, value); }
        }
        private bool hasDrag;
        public bool HasDrag
        {
            get { return this.hasDrag; }
            set { SetProperty(ref hasDrag, value); }
        }
        public override TestParameters GetTestParameters()
        {
            return new ProductTest2Parameters() { Z = this.Z, HasDrag = this.HasDrag };
        }
    }
    public static class TestParameterViewModelFactory
    {
        private static Func<Type, TestParameterViewModel> Factory { get; set; }
        public static void Initialize(Func<Type, TestParameterViewModel> testParameterViewModelFactory)
        {
            Factory = testParameterViewModelFactory;
        }
        public static TestParameterViewModel Create(Type testParameterType)
        {
            return Factory(testParameterType);
        }
    }
}
