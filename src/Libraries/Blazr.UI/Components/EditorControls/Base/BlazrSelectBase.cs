/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.UI;

public abstract class BlazrSelectBase<TValue> : BlazrEditControlBase<TValue>, IHandleAfterRender
{
    protected readonly bool isMultipleSelect;

    public BlazrSelectBase(): base()
        => this.isMultipleSelect = typeof(TValue).IsArray;

    protected string? ValueAsString
        => BlazrInputConverters.GetValueAsString(this.Value, string.Empty);
}
