using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;


namespace Ziggy.ProductTests
{
    public abstract class Test
    {
        public Task<TestResult> Execute(object target, TestParameters parameters, CancellationToken token = default(CancellationToken))
        {
            return DoExecute(target, parameters, token);
        }
        protected abstract Task<TestResult> DoExecute(object target, TestParameters parameters, CancellationToken token);
    }
    public abstract class Test<TTarget, TParameters, TResult> : Test
        where TParameters: TestParameters
        where TResult: TestResult
    {
        public Task<TResult> Execute(TTarget target, TParameters parameters, CancellationToken token = default(CancellationToken))
        {
            return DoExecute(target, parameters, token);
        }
        protected abstract Task<TResult> DoExecute(TTarget target, TParameters parameters, CancellationToken token = default(CancellationToken));

        protected override async Task<TestResult> DoExecute(object target, TestParameters parameters, CancellationToken token)
        {
            return await DoExecute((TTarget)target, (TParameters)parameters, token);
        }
    }
    public static class TestFactory
    {
        private static Func<Type, Test> Factory { get; set; }
        public static void Initialize(Func<Type, Test> testFactory)
        {
            Factory = testFactory;
        }
        public static Test Create(Type testType)
        {
            return Factory(testType);
        }
    }
}
