Grid columns should either inherit from `BlazrGridColumnBase` or implement it's funtionality i.e.:

1. Register with the parent using the cascaded `Action<IBlazrGridItem<TGridItem>>` on first render.
2. Not render any content.

In the example `BlazorGridColumn` inherits from `BlazorGridColumn` and defines each column in the grid.

```csharp
<BlazrGrid TGridItem="WeatherForecast" Items=this.Presenter.ListContext>
    <BlazrGridColumn TGridItem="WeatherForecast" Title="Date">@context.Date.AsGlobalDate()</BlazrGridColumn>
    <BlazrGridColumn TGridItem="WeatherForecast" Title="Temperature C">@context.TemperatureC</BlazrGridColumn>
    <BlazrGridColumn TGridItem="WeatherForecast" Title="Temperature F">@context.AsTemperatureF()</BlazrGridColumn>
    <BlazrGridColumn TGridItem="WeatherForecast" Title="Summary">@context.Summary</BlazrGridColumn>
</BlazrGrid>
```

`BlazrGridColumnBase` is not a normal component.

Importantly, it's abstract and implements `IComponent`.  It does not inherit from `ComponentBase`.  We have the freedom to implement the necessary functionality cleanly and efficiently.

```csharp
public abstract class BlazrGridColumnBase<TGridItem> : IComponent
{
```

It implements `IComponent.Attach` capturing the provided `RenderHandle` instance to `_renderHandle`. 

```csharp
    private RenderHandle _renderHandle;

    public void Attach(RenderHandle renderHandle)
        => _renderHandle = renderHandle;
```

And `IComponent.SetParametersAsync` by calling the internal method `RegisterComponent`.

```csharp
    public virtual Task SetParametersAsync(ParameterView parameters)
    {
        parameters.SetParameterProperties(this);
        this.RegisterComponent();
        return Task.CompletedTask;
    }
```

`RegisterComponent` is designed to only runs once. `Register` is a cascaded `Action` that it calls, providing an object that implements an instance of `IBlazrGridItem<TGridItem>`.

```csharp
    [CascadingParameter] private Action<IBlazrGridItem<TGridItem>>? Register { get; set; }

    private bool _hasNeverRendered = true;

    private void RegisterComponent()
    {
        if (_hasNeverRendered && Register is not null)
            _renderHandle.Render((builder) =>
            {
                Register.Invoke(this.GridItem);
            });
        _hasNeverRendered = false;
    }
```

`GridItem` is a property with a getter that constructs a `BlazrGridItem<TGridItem>` instance from the component parameters and Render Fragment.

```csharp
[Parameter, EditorRequired] public string Title { get; set; } = "Field";

public readonly Guid ComponentId = Guid.NewGuid();

protected virtual IBlazrGridItem<TGridItem> GridItem
    => new BlazrGridItem<TGridItem>
    {
        Title = this.Title,
        ComponentId = ComponentId,
        ItemContent = this.ItemContent,
    };
```

`ItemContent` is directly mapped to the `ChildContent`.  It's virtual so can be overridden in child inplementations.

```csharp
[Parameter] public RenderFragment<TGridItem>? ChildContent { get; set; }

protected virtual RenderFragment<TGridItem>? ItemContent => ChildContent;
```

Finally it implements a virtual `BuildRenderTree` so it can be inherited by any Razor component.

```csharp
    protected virtual void BuildRenderTree(RenderTreeBuilder builder) { }
```

Note that the component doesn't render any content.  When an instance it created by the Renderer it sinply calls the cascaded `Action` to register it's configuration.

## IBlazrGridItem

`IBlazrGridItem<TGridItem>` defines an Id that ties it to a component and two methods to create the header and row content for the column.

```csharp
public interface IBlazrGridItem<TGridItem>
{
    public Guid ComponentId { get; }
    public void GetItemHeaderContent(RenderTreeBuilder builder);
    public void GetItemRowContent(RenderTreeBuilder builder, TGridItem item);
}
```

The base `BlazrGridItemBase` implements a simple table based structure.

It builds a header cell render fragment using `Title` and the configured `HeaderCss` and a row fragment using the supplied RenderFragment from the component.

```csharp
public abstract class BlazrGridItemBase<TGridItem> : RazorBase, IBlazrGridItem<TGridItem>
{
    public Guid ComponentId { get; set; } = Guid.Empty;
    public string Title { get; set; } = "Not Defined";
    public string HeaderCss { get; set; } = BlazrGridCss.HeaderCss;
    public string ItemRowCss { get; set; } = BlazrGridCss.ItemRowCss;
    public string MaxColumnCss { get; set; } = BlazrGridCss.MaxColumnCss;
    public bool IsMaxRow { get; set; } = false;
    public RenderFragment<TGridItem>? ItemContent { get; set; }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    { }

    public void GetItemRowContent(RenderTreeBuilder builder, TGridItem item)
    {
        var css = new CSSBuilder(this.ItemRowCss)
            .AddClass(this.IsMaxRow, this.MaxColumnCss)
            .Build();

        builder.OpenElement(0, "td");
        builder.AddAttribute(1, "class", css);
        builder.AddAttribute(2, "key", item);
        if (this.ItemContent is not null)
            builder.AddContent(3, ItemContent(item));

        builder.CloseComponent();
    }

    public void GetItemHeaderContent(RenderTreeBuilder builder)
    {
        var css = new CSSBuilder(this.HeaderCss)
            .AddClass(this.IsMaxRow, this.MaxColumnCss)
            .Build();

        builder.OpenElement(0, "th");
        builder.AddAttribute(1, "class", css);
        builder.AddContent(2, this.Title);
        builder.CloseComponent();
    }
}
```

The basic `BlazrGridItem` provides a concrete implementation of the base component.

```csharp
public sealed class BlazrGridItem<TGridItem> : BlazrGridItemBase<TGridItem> {}
```