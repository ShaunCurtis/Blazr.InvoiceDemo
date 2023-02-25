/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.UI.BlazrGrid;

// The Component is configured to get it's data set from one of two sources (in order of precidence):
//   1. The Parameter provided ListComponentController.
//   2. The cascaded cascadedListComponentController instance.

public partial class BlazrGrid<TGridItem> : UIBase, IDisposable
    where TGridItem : class, new()
{
    /// <summary>
    /// One of the three mechanisms for providing 
    /// </summary>
    [CascadingParameter] private IListController<TGridItem>? cascadedListComponentController { get; set; }

    [Parameter] public IListController<TGridItem>? ListComponentController { get; set; }

    private bool _initialized;
    private RenderFragment _gridRenderFragment;
    private TaskCompletionSource? _firstRenderTaskManager = null;
    private Task firstRenderTask => _firstRenderTaskManager?.Task ?? Task.CompletedTask;
    private readonly List<IBlazrGridItem<TGridItem>> _gridColumns = new();
    private IListController<TGridItem>? _listComponentController { get; set; }

    private IEnumerable<TGridItem> _items = Enumerable.Empty<TGridItem>();

    public void RegisterColumn(IBlazrGridItem<TGridItem> column)
    {
        if (!_gridColumns.Any(item => item.ComponentId == column.ComponentId))
            _gridColumns.Add(column);
    }

    public override async Task SetParametersAsync(ParameterView parameters)
    {
        // Sets the component parameters to the supplied values
        parameters.SetParameterProperties(this);

        // Set the list Controller, prioritizing the Parameter Value
        if (!_initialized)
            _listComponentController = ListComponentController ?? cascadedListComponentController;

        // Wires up the component to the controller's ListChanged event if it exists
        if (!_initialized && this._listComponentController is not null)
        {
            _listComponentController.StateChanged += OnListChanged;

            // If we don't have a pager we need to manually initialise the first data set query
            if (!_listComponentController.HasPager)
            {
                DummyPager pager = new();
                _listComponentController.RegisterPager(pager);
                await _listComponentController.NotifyPagingRequestedAsync(pager, new PagingEventArgs { Request = new() });
                _listComponentController.RegisterPager(null);
            }
        }

        // Sets the internal TGridItem collection
        _items = _listComponentController ?? Enumerable.Empty<TGridItem>();


        // Render the grid content
        await this.Render();

        _initialized = true;
    }

    public BlazrGrid()
    {
        _gridRenderFragment = (builder) =>
        {
            builder.AddContent(0, this.gridRenderFragment);
            this.hasPendingQueuedRender = false;
        };
    }

    private async ValueTask Render()
    {
        if (this.hasPendingQueuedRender)
            return;

        this.hasPendingQueuedRender = true;

        if (!_initialized)
        {
            // set up a new running Task
            _firstRenderTaskManager = new TaskCompletionSource();

            // Render the columns
            this.renderHandle.Render(captureColumnsRenderFragment);

            // This is important there's no real async yields so far so we need to add one
            await Task.Yield();
        }

        // Wait on the first render to complete
        await this.firstRenderTask;

        this.renderHandle.Render(_gridRenderFragment);
    }

    public ValueTask RefreshAsync()
        => this.Render();

    public async void OnListChanged(object? sender, EventArgs e)
        => await Render();

    public void Dispose()
    {
        if (this._listComponentController is not null)
        {
            _listComponentController.StateChanged -= OnListChanged;
        }
    }
}
