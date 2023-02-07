/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.UI;

public abstract class BlazrInputBase<TValue> : BlazrEditControlBase<TValue>, IHandleAfterRender
{
    [Parameter] public InputType Type { get; set; } = InputType.Text;
    [Parameter] public bool UpdateOnInput { get; set; }

    protected string? ValueAsString
        => BlazrInputConverters.GetValueAsString(this.Value, GetTypeString(this.Type));

    public static string GetTypeString(InputType inputType)
    {
        return inputType switch
        {
            InputType.Number => "number",
            InputType.Image => "image",
            InputType.Date => "date",
            InputType.File => "file",
            InputType.Email => "email",
            InputType.Checkbox => "checkbox",
            InputType.Radio => "radio",
            InputType.Color => "color",
            InputType.Password => "password",
            InputType.DatetimeLocal => "datetime-local",
            InputType.Hidden => "hidden",
            InputType.Month => "month",
            InputType.Range => "range",
            InputType.Search => "search",
            InputType.Tel => "tel",
            InputType.Url => "url",
            InputType.Week => "week",
            _ => "text"
        };
    }
}
