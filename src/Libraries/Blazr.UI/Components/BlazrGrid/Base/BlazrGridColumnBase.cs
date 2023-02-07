/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.UI.BlazrGrid;

public abstract class BlazrGridColumnBase<TGridItem> : IComponent, IBlazrGridItem<TGridItem>
{
    private RenderHandle _renderHandle;
    private bool _hasNeverRendered = true;

    [Parameter] public RenderFragment<TGridItem>? ChildContent { get; set; }

    [Parameter, EditorRequired] public string Title { get; set; } = "Field";

    [Parameter] public bool IsMaxColumn { get; set; } = false;

    [Parameter] public bool IsNoWrap { get; set; } = false;

    [Parameter] public string? Class { get; set; }

    [CascadingParameter] private Action<IBlazrGridItem<TGridItem>>? Register { get; set; }

    public Guid ComponentId { get; init; } = Guid.NewGuid();

    protected virtual void BuildRenderTree(RenderTreeBuilder builder) { }

    public void Attach(RenderHandle renderHandle)
        => _renderHandle = renderHandle;

    public virtual Task SetParametersAsync(ParameterView parameters)
    {
        parameters.SetParameterProperties(this);
        this.RegisterComponent();

        return Task.CompletedTask;
    }

    private void RegisterComponent()
    {
        if (_hasNeverRendered && Register is not null)
            _renderHandle.Render((builder) =>
            {
                Register.Invoke(this);
            });
        _hasNeverRendered = false;
    }

    public virtual void GetItemHeaderContent(RenderTreeBuilder builder)
    {
        var css = new CSSBuilder(BlazrGridCss.HeaderCss)
            .AddClass("align-baseline")
            .Build();

        builder.OpenElement(0, "th");
        builder.AddAttribute(1, "class", css);
        builder.AddMarkupContent(2, this.Title);
        builder.CloseElement();
    }

    public virtual void GetItemRowContent(RenderTreeBuilder builder, TGridItem item)
    {
        var css = new CSSBuilder(BlazrGridCss.ItemRowCss)
            .AddClass(this.IsMaxColumn, BlazrGridCss.MaxColumnCss)
            .AddClass(!this.IsMaxColumn && IsNoWrap, BlazrGridCss.NoWrapCss)
            .AddClass(this.Class)
            .Build();

        builder.OpenElement(0, "td");
        builder.AddAttribute(1, "class", css);
        builder.AddAttribute(2, "key", item);
        if (this.ChildContent is not null)
            builder.AddContent(10, ChildContent(item));

        builder.CloseComponent();
    }
}
