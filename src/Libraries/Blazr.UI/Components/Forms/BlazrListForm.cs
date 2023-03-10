/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.UI;

public sealed class BlazrListForm<TRecord> : ComponentBase
    where TRecord : class, new()
{
    [Parameter] public RenderFragment? ChildContent { get; set; }

    [Parameter] [EditorRequired] public IListController<TRecord>? ListContext { get; set; }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenComponent<CascadingValue<IListController<TRecord>>>(0);
        builder.AddAttribute(1, "Value", this.ListContext);
        builder.AddAttribute(2, "IsFixed", true);
        builder.AddAttribute(3, "ChildContent", ChildContent);
        builder.CloseComponent();
    }
}

