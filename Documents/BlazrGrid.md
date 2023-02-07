`BlazrGrid` is the primary grid component.

It inherits from `UIBase` which is a basic component implementation and implements `IDisposable` as it wires up some event handlers.  Using `UIBase` frees us of the constraints and simplifies the control of the render process.

It can be manually attached to a `IEnumerable<T>` list, but normally uses a cascaded `ListController` to interact with the data pipeline. 

Before examining how the component renders content, we need to look at some the render fragments.  These are defined in the Razor file.

`captureColumnsRenderFragment` renders the child column components.  It clears the column list and cascades the Register delegate to the columns.  Once complete it sets the `_firstRenderTaskManager` Task completion source instance to completed (more about this soon).  The purpose of this fragment is to populate the `_gridColumns` list.  The column components have no visible markup so while they exist in the Render Tree, there's no markup in the DOM. 

```csharp
private RenderFragment captureColumnsRenderFragment => (__builder) =>
{
    _gridColumns.Clear();

    <CascadingValue Value="this.RegisterColumn" IsFixed>
        @ChildContent
    </CascadingValue>

    _firstRenderTaskManager?.TrySetResult();
};
```

`gridRenderFragment` renders the main grid content.  It builds out a standard table.

```csharp
protected virtual RenderFragment gridRenderFragment => (__builder) =>
{
    int rowIndex = 0;

    <table class="@BlazrGridCss.TableCss">
        <thead class="@BlazrGridCss.TableHeaderCss">
            @{
                this.renderHeaderRow(__builder);
            }
        </thead>
        <tbody>
            @foreach (var item in _items)
            {
                this.renderRow(__builder, rowIndex, item);
                rowIndex++;
            }
        </tbody>
    </table>
};
```
`renderHeaderRow` builds out the header columns from the `_gridColumns` collection. It calls the `GetItemHeaderContent` method on the object passing in the `RenderTreeBuilder`.

```csharp
protected virtual void renderHeaderRow(RenderTreeBuilder __builder)
{
    <tr>
        @foreach (var col in _gridColumns)
        {
            col.GetItemHeaderContent(__builder);
        }
    </tr>
}
```
`renderrRow` builds out the header columns from the `_gridColumns` collection. It calls the `GetItemRowContent` method on the object passing in the `RenderTreeBuilder`, the row index number and the specific `TGridItem` orw instance from the datya set.

```csharp
protected virtual void renderRow(RenderTreeBuilder __builder, int rowIndex, TGridItem item)
{
    <tr aria-rowindex="@rowIndex" class="@BlazrGridCss.TableRowCss">
        @foreach (var col in _gridColumns)
        {
            col.GetItemRowContent(__builder, item);
        }
    </tr>
}
```

The class declaration inherits from `UIBase` and implements the `IDisposable`.  There are three parameters that provide three methods to pass data into the componwnt.

```csharp
public partial class BlazrGrid<TGridItem> : UIBase, IDisposable
    where TGridItem : class, new()
{
    [Parameter] public IEnumerable<TGridItem> Items { get; set; } = Enumerable.Empty<TGridItem>();

    [CascadingParameter] private ListComponentController<TGridItem>? CascadedListComponentController { get; set; }

    [Parameter] public ListComponentController<TGridItem>? ListComponentController { get; set; }
```

The Ctor caches the _gridRenderFragment to render the grid.


```csharp
private RenderFragment _gridRenderFragment;

public BlazrGrid()
{
    _gridRenderFragment = (builder) =>
    {
        builder.AddContent(0, this.gridRenderFragment);
        this.hasPendingQueuedRender = false;
    };
}
```





```csharp
public partial class BlazrGrid<TGridItem> : UIBase, IDisposable
    where TGridItem : class, new()
{
    [Parameter] public IEnumerable<TGridItem> Items { get; set; } = Enumerable.Empty<TGridItem>();

    [CascadingParameter] private ListComponentController<TGridItem>? CascadedListComponentController { get; set; }

    [Parameter] public ListComponentController<TGridItem>? ListComponentController { get; set; }

    private bool _initialized;
    private RenderFragment _gridRenderFragment;
    private TaskCompletionSource? _firstRenderTaskManager = null;
    private Task firstRenderTask => _firstRenderTaskManager?.Task ?? Task.CompletedTask;

    private readonly List<IBlazrGridItem<TGridItem>> _gridColumns = new();
    private ListComponentController<TGridItem>? _listComponentController { get; set; }

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
            _listComponentController = ListComponentController ?? CascadedListComponentController;

        // Wires up the component to the controller's ListChanged event if it exists
        if (!_initialized && this._listComponentController is not null)
        {
            _listComponentController.ListChanged += OnListChanged;
            _initialized = true;
        }

        if (hasNeverRendered)
        {
            // set up a new running Task
            _firstRenderTaskManager = new TaskCompletionSource();

            // Render the columns
            this.renderHandle.Render(CaptureColumns);

            // Give the Renderer some processor time
            // basically schedules the continuation to the end of the queue
            await Task.Delay(1);
        }

        // Wait on the first render to complete
        await this.firstRenderTask;

        // Sets the internal TGridItem collection, prioritizing the List Controller if it exists
        _items = _listComponentController ?? this.Items;

        // Render the grid content
        this.RenderGrid();
    }

    public BlazrGrid()
    {
        _gridRenderFragment = (builder) =>
        {
            builder.AddContent(0, this.gridRenderFragment);
            this.hasPendingQueuedRender = false;
        };
    }

    private void RenderGrid()
    {
        if (this.hasPendingQueuedRender)
            return;

        this.hasPendingQueuedRender = true;
        this.renderHandle.Render(_gridRenderFragment);
    }

    public void Refresh()
        => this.RenderGrid();

    public void OnListChanged(object? sender, EventArgs e)
        => RenderGrid();

    public void Dispose()
    {
        if (this._listComponentController is not null)
        {
            _listComponentController.ListChanged -= OnListChanged;
        }
    }
}
```