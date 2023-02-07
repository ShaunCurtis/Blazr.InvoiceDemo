/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

namespace Blazr.UI;

public class BlazrSelect<TValue> : BlazrSelectBase<TValue>
{
    protected RenderFragment BaseInputControl;

    public BlazrSelect() : base()
        => this.BaseInputControl = BuildControl;

    protected override void BuildRenderTree(RenderTreeBuilder builder)
        => this.BuildControl(builder);

    private void BuildControl(RenderTreeBuilder builder)
    {
        builder.OpenElement(0, "select");
        builder.AddMultipleAttributes(1, this.AdditionalAttributes);
        builder.AddAttributeIfNotEmpty(2, "class", this.CssClass);
        builder.AddAttributeIfTrue(3, this.isMultipleSelect,  "multiple", this.isMultipleSelect);
        builder.AddAttribute(4, "value", this.ValueAsString);
        builder.AddAttribute(5, "onchange", this.OnChanged);
        builder.AddAttribute(5, "id", this.ElementId);
        builder.AddElementReferenceCapture(6, __inputReference => this.Element = __inputReference);
        builder.AddContent(7, this.ChildContent);
        builder.CloseElement();
    }
}
