using System;
using System.Threading;
using System.Threading.Tasks;



class Program
{
    static void Main(string[] args)
    {
        Handle(ThrowBeforeAwait);
        Handle(ThrowAfterAwait);
        Handle(AwaitThenDoWork);
    }

    static void Handle(Func<Task> f)
    {
        async Task localHandler()
        {
            try
            {
                await f();
            }
            catch (Exception)
            {
                Console.WriteLine("Caught!");
            }
        }
        localHandler().GetAwaiter().GetResult();
    }

    #pragma warning disable CS0162 // unreachable code 
    static async Task ThrowBeforeAwait()
    {
        throw new Exception();
        await Task.Yield();
    }

    static async Task ThrowAfterAwait()
    {
        await Task.Yield();
        throw new Exception();
    }

    static async Task AwaitThenDoWork()
    {
        Console.WriteLine("On threadpool: " + Thread.CurrentThread.IsThreadPoolThread);
        await Task.Yield();
        Console.WriteLine("On threadpool: " + Thread.CurrentThread.IsThreadPoolThread);
    }
}