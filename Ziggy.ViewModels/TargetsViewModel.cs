using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using System.Reflection;
using Ziggy.ProductTests;
using System.Threading;

namespace Ziggy.ViewModels
{
    public class TargetsViewModel : BindableBase
    {
        public ObservableCollection<TargetViewModel> Targets { get; private set; }
        public ObservableCollection<TestViewModel> Tests { get; private set; }
        protected IBindingOperations BindingOperations { get; private set; }
        public DelegateCommand ExecuteTests { get; private set; }
        public DelegateCommand CancelTests { get; private set; }
        private bool isRunning;
        public bool IsRunning
        {
            get { return this.isRunning; }
            set
            {
                if (SetProperty(ref isRunning, value))
                {
                    this.ExecuteTests.RaiseCanExecuteChanged();
                    this.CancelTests.RaiseCanExecuteChanged();
                }
            }
        }
        private CancellationTokenSource TokenSource { get; set; }
        public TargetsViewModel(IBindingOperations bindingOperations)
        {
            this.Targets = new ObservableCollection<TargetViewModel>();
            this.Tests = new ObservableCollection<TestViewModel>();
            bindingOperations.EnableCollectionSynchronization(this.Targets);
            bindingOperations.EnableCollectionSynchronization(this.Tests);
            this.ExecuteTests = new DelegateCommand(async () =>
            {
                this.IsRunning = true;
                if (TokenSource != null)
                    TokenSource.Cancel();
                TokenSource = new CancellationTokenSource();
                try
                {
                    foreach (var test in this.Tests.OrderBy(t => t.SortOrder))
                    {
                        await test.Execute(TokenSource.Token);
                    }
                }
                catch (OperationCanceledException) { }
                catch (Exception)
                {
                    //report
                }
                finally
                {
                    IsRunning = false;
                }
            }, () => !this.IsRunning);
            this.CancelTests = new DelegateCommand(() => { if (this.TokenSource != null)this.TokenSource.Cancel(); }, () => this.IsRunning); 
        }
        public async Task LoadTargets()
        {
            this.Targets.Clear();
            await Task.Run(async() =>
            {
                foreach (var an in Assembly.GetExecutingAssembly().GetReferencedAssemblies())
                {
                    Assembly a = Assembly.Load(an);
                    foreach (var type in a.GetTypes().Where(t => t.GetCustomAttributes<TargetTestAttribute>(true).Count() > 0))
                    {
                        var targetVM = TargetViewModelFactory.Create(TargetFactory.Create(type));
                        Targets.Add(targetVM);
                        targetVM.Tests.CollectionChanged += Tests_CollectionChanged;
                        await targetVM.LoadTests();
                    }
                }
            });
        }

        void Tests_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (var newItem in e.NewItems.Cast<TestViewModel>())
                {
                    newItem.PropertyChanged += newItem_PropertyChanged;
                }
            }
            if (e.OldItems != null)
            {
                foreach (var oldItem in e.OldItems.Cast<TestViewModel>())
                {
                    oldItem.PropertyChanged -= newItem_PropertyChanged;
                }
            }
        }

        void newItem_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Include")
            {
                TestViewModel model = sender as TestViewModel;
                if (model != null)
                {
                    if (model.Include)
                        this.Tests.Add(model);
                    else
                        this.Tests.Remove(model);
                }
            }

        }
    }
}
