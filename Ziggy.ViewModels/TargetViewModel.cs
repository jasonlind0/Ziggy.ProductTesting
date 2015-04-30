using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Mvvm;
using System.Collections.ObjectModel;
using Ziggy.ProductTests;
using System.Reflection;

namespace Ziggy.ViewModels
{
    public class TargetViewModel : BindableBase
    {
        public ObservableCollection<TestViewModel> Tests { get; private set; }
        public object Target { get; private set; }
        public TargetViewModel(IBindingOperations bindingOperations, object target)
        {
            Tests = new ObservableCollection<TestViewModel>();
            bindingOperations.EnableCollectionSynchronization(Tests);
            this.Target = target;
        }
        public async Task LoadTests()
        {
            this.Tests.Clear();
            await Task.Run(() =>
            {
                foreach (var test in Target.GetType().GetCustomAttributes<TargetTestAttribute>(true))
                {
                    Tests.Add(TestViewModelFactory.Create(test.TestType, this.Target));
                }
            });
        }
    }
    public static class TargetViewModelFactory
    {
        private static Func<object, TargetViewModel> Factory { get; set; }
        public static void Initialize(Func<object, TargetViewModel> targetViewModelFactory)
        {
            Factory = targetViewModelFactory;
        }
        public static TargetViewModel Create(object target)
        {
            return Factory(target);
        }
    }
    public static class TargetFactory
    {
        private static Func<Type, object> Factory { get; set; }
        public static void Initialize(Func<Type, object> factory)
        {
            Factory = factory;
        }
        public static object Create(Type targetType)
        {
            return Factory(targetType);
        }
    }
}
