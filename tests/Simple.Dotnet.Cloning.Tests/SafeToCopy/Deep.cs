using FluentAssertions;
using System;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Simple.Dotnet.Cloning.Tests.SafeToCopy
{
    public class Deep
    {
        

        [Fact]
        public void ShallowClone_Should_Return_Same_Ref_Instance()
        {
            ShouldBeSame(TimeZoneInfo.Utc);
            ShouldBeSame(new Timer(t => { }));
            ShouldBeSame(new System.Timers.Timer());
            ShouldBeSame("string");
            ShouldBeSame(Task.CompletedTask);
            ShouldBeSame(Task.FromResult(10));
            ShouldBeSame(new Thread(() => { }));
            ShouldBeSame(new TaskCompletionSource<object>());
            ShouldBeSame(new CancellationTokenSource());
            ShouldBeSame(new Lazy<int>(() => 1));
            ShouldBeSame(new AsyncLocal<int>());
            ShouldBeSame(new ThreadLocal<int>());
            ShouldBeSame(new AutoResetEvent(false));
            ShouldBeSame(new Barrier(1));
            ShouldBeSame(new CountdownEvent(1));
            ShouldBeSame(new ManualResetEvent(true));
            ShouldBeSame(new ManualResetEventSlim());
            ShouldBeSame(new Mutex());
            ShouldBeSame(new ReaderWriterLock());
            ShouldBeSame(new SemaphoreSlim(1));
            ShouldBeSame(new ConcurrentBag<int>());
            ShouldBeSame(new ConcurrentDictionary<int, int>());
            ShouldBeSame(new ConcurrentQueue<int>());
            ShouldBeSame(new ConcurrentStack<int>());
            ShouldBeSame(GetType().Assembly);
            ShouldBeSame(GetType());
            ShouldBeSame(typeof(string).GetMembers().First());
            ShouldBeSame(typeof(string).GetConstructors().First());
            ShouldBeSame(typeof(string).GetFields().First());
            ShouldBeSame(ImmutableDictionary<int, int>.Empty);
            ShouldBeSame(ImmutableStack<int>.Empty);
            ShouldBeSame(ImmutableSortedSet<int>.Empty);
            ShouldBeSame(ImmutableSortedSet<int>.Empty);
            ShouldBeSame(ImmutableList<int>.Empty);
            ShouldBeSame(ImmutableHashSet<int>.Empty);
            ShouldBeSame(new Exception());
            ShouldBeSame(new ArgumentException());
            ShouldBeSame(new Action(() => { }));
            ShouldBeSame(new Func<int>(() => 1));
        }

        static void ShouldBeSame<T>(T instance) where T : class
        {
            try
            {
                (instance.DeepClone() == instance).Should().BeTrue();
                (((T)null).DeepClone() == null).Should().BeTrue();

                var wrapper = new Wrapper<T>(instance);
                (wrapper.DeepClone().Value == wrapper.Value).Should().BeTrue();

                var structWrapper = new WrapperStruct<T>(instance);
                (structWrapper.DeepClone().Value == structWrapper.Value).Should().BeTrue();

                var recordWrapper = new WrapperRecord<T>(instance);
                (recordWrapper.DeepClone().Value == recordWrapper.Value).Should().BeTrue();

                var readonlyWrapper = new WrapperReadonly<T>(instance);
                (readonlyWrapper.DeepClone().Value == readonlyWrapper.Value).Should().BeTrue();
            }
            finally
            {
                if (instance is IDisposable d) d.Dispose();
            }
        }
    }
}
