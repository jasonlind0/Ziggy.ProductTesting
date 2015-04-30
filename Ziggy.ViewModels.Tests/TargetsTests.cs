using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ziggy.ViewModels;
using Ziggy.ProductTests;
using Microsoft.Practices.Unity;
using System.Collections;
using System.Threading.Tasks;
using System.Linq;
using System.Threading;

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
                string name = type.Name;
                string viewModelTypeName = string.Format(System.Globalization.CultureInfo.InvariantCulture, "{1}.{0}ViewModel, {1}", name, "Ziggy.ViewModels");
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
        [TestMethod]
        public async Task ReadSetupExecuteTest()
        {
            var vm = ViewModelFactory.Create<TargetsViewModel>();
            await vm.LoadTargets();
            Assert.AreEqual(2, vm.Targets.Count);
            Assert.AreEqual(3, vm.Targets.SelectMany(t => t.Tests).Count());
            int sortOrder = 1;
            foreach (var target in vm.Targets)
            {
                var test = target.Tests.First();
                test.Include = true;
                test.SortOrder = sortOrder;
                sortOrder++;
            }
            Assert.AreEqual(2, vm.Tests.Count);
            SemaphoreSlim s = new SemaphoreSlim(0);
            var waitS = s.WaitAsync();
            vm.PropertyChanged += (sender, e) => { if (e.PropertyName == "IsRunning" && !vm.IsRunning)s.Release(); };
            Task waitET = vm.ExecuteTests.Execute();
            await waitET;
            await waitS;
            Assert.AreEqual(2, vm.Tests.Select(t => t.Result).Where(r => r != null).Count());
        }
    }
    public class MockBindingOperations : IBindingOperations
    {
        public void EnableCollectionSynchronization(IEnumerable collection)
        {
            
        }
    }
}
