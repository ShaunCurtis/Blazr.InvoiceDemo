/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.UI;

public sealed class BlazrEditForm<TRecord> : ComponentBase
    where TRecord : class, new()
{
    [Parameter] public RenderFragment? ChildContent { get; set; }

    [Parameter][EditorRequired] public IEditContext? EditContext { get; set; }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenComponent<CascadingValue<IEditContext>>(0);
        builder.AddAttribute(1, "Value", this.EditContext);
        builder.AddAttribute(2, "IsFixed", true);
        builder.AddAttribute(3, "ChildContent", ChildContent);
        builder.CloseComponent();
    }
}

