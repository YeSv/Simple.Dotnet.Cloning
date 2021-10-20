using FluentAssertions;
using System;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Simple.Dotnet.Cloning.Tests.SafeToCopy
{
    public sealed class Shallow
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
                (instance.ShallowClone() == instance).Should().BeTrue();
                (((T)null).ShallowClone() == null).Should().BeTrue();
            }
            finally
            {
                if (instance is IDisposable d) d.Dispose();
            }
        }
    }
}
