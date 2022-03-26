using System.Collections.Concurrent;

namespace FORCE.Core.Utilities;

internal class ObjectSemaphoreSlim
{
    private readonly ConcurrentDictionary<object, SemaphoreSlim> _semaphores = new();

    public async Task WaitAsync(object obj)
    {
        await _semaphores.GetOrAdd(obj, new SemaphoreSlim(1, 1)).WaitAsync();
    }

    public void Release(object obj)
    {
        if (_semaphores.TryGetValue(obj, out var semaphore))
        {
            semaphore.Release();
        }
    }
}
