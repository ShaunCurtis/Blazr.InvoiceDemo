/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.UI;

public class BlazrInput<TValue> : BlazrInputBase<TValue>, IHandleAfterRender
{
    protected RenderFragment BaseInputControl;

    public BlazrInput()
        : base()
        => this.BaseInputControl = BuildControl;

    protected override void BuildRenderTree(RenderTreeBuilder builder)
        => this.BuildControl(builder);

    private void BuildControl(RenderTreeBuilder builder)
    {
        var isTextArea = this.Type.Equals(InputType.TextArea);
        var tag = isTextArea
            ? "textarea"
            : "input";

        var eventName = this.UpdateOnInput
            ? "oninput"
            : "onchange";

        builder.OpenElement(0, tag);
        builder.AddMultipleAttributes(1, this.AdditionalAttributes);
        builder.AddAttributeIfTrue(2, !isTextArea, "type", this.Type);
        builder.AddAttributeIfNotEmpty(3, "class", this.CssClass);
        builder.AddAttribute(4, "value", this.ValueAsString);
        builder.AddAttribute(5, "id", this.ElementId);
        builder.AddAttribute(6, eventName, this.OnChanged);
        builder.AddElementReferenceCapture(7, __inputReference => this.Element = __inputReference);
        builder.CloseElement();
    }
}
