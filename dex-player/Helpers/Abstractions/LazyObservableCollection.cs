namespace DexPlayer.Helpers.Abstractions;

using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;

internal abstract class LazyObservableCollection<M, VM>
   : ObservableCollection<VM>, ISupportIncrementalLoading
    where M : new()
{
    public LazyObservableCollection(DispatcherQueue dispatcherQueue)
    {
        this.dispatcherQueue = dispatcherQueue;
    }

    readonly DispatcherQueue dispatcherQueue;
    uint offset = 0;
    int lastNumber = 1;

    protected uint max = uint.MaxValue;

    public bool HasMoreItems { get => offset < max; }

    public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count) =>
        InnerLoadMoreItemsAsync(count > 20 ? 20 : count).AsAsyncOperation();

    public abstract Task<List<M>> LoadModels(uint count, uint offset);

    public abstract VM ToVM(M model, int number);

    async Task<LoadMoreItemsResult> InnerLoadMoreItemsAsync(uint count)
    {
        var models = await LoadModels(count, offset);
        var vms = models.Select(m => ToVM(m, lastNumber++));

        dispatcherQueue.TryEnqueue(() =>
        {
            foreach (var vm in vms)
                Add(vm);
        });

        var loaded = (uint)models.Count;
        offset += loaded;

        return new LoadMoreItemsResult { Count = loaded };
    }
}
