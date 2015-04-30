using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ziggy.ViewModels;
using Ziggy.ProductTests;
using Microsoft.Practices.Unity;
using System.Collections;
using System.Threading.Tasks;
using System.Linq;

namespace Ziggy.ViewModels.Tests
{
    [TestClass]
    public class TargetsTests
    {
        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            IUnityContainer container = new UnityContainer();
            container.RegisterInstance<IBindingOperations>(new MockBindingOperations());
            TestFactory.Initialize(type => (Test)container.Resolve(type));
            ViewModelFactory.Initialize(type => container.Resolve(type));
            TestViewModelFactory.Initialize((testType, target) => new TestViewModel(testType, target));
            TestParameterViewModelFactory.Initialize(type =>
            {
                string name = type.Name;
                string viewModelTypeName = string.Format(System.Globalization.CultureInfo.InvariantCulture, "{1}.{0}ViewModel, {1}", name, "Ziggy.ViewModels");
                return (TestParameterViewModel)container.Resolve(Type.GetType(viewModelTypeName));
            });
            TestResultViewModelFactory.Initialize((type, result) =>
            {
                string name = type.FullName;
                string viewModelTypeName = string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}ViewModel, {1}", name, "Ziggy.ViewModels");
                return (TestResultViewModel)container.Resolve(Type.GetType(viewModelTypeName), new ParameterOverride("result", result));
            });
            TargetViewModelFactory.Initialize(obj => container.Resolve<TargetViewModel>(new ParameterOverride("target", obj)));
            TargetFactory.Initialize(type => container.Resolve(type));
        }
        [TestMethod]
        public async Task ReadTargetsViewModel()
        {
            var vm = ViewModelFactory.Create<TargetsViewModel>();
            await vm.LoadTargets();
            Assert.AreEqual(2, vm.Targets.Count);
            Assert.AreEqual(3, vm.Targets.SelectMany(t => t.Tests).Count());
        }
    }
    public class MockBindingOperations : IBindingOperations
    {
        public void EnableCollectionSynchronization(IEnumerable collection)
        {
            
        }
    }
}
