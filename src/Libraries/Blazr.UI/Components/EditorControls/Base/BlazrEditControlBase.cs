/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.UI;

public abstract class BlazrEditControlBase<TValue> : UIControlBase, IHandleAfterRender
{
    [CascadingParameter] protected IEditContext editContext { get; set; } = default!;

    [Parameter] public TValue? Value { get; set; }
    [Parameter] public EventCallback<TValue> ValueChanged { get; set; }
    [Parameter] public Expression<Func<TValue>>? ValueExpression { get; set; }
    [Parameter] public bool IsFirstFocus { get; set; }
    [Parameter(CaptureUnmatchedValues = true)] public IReadOnlyDictionary<string, object> AdditionalAttributes { get; set; } = new Dictionary<string, object>();

    [DisallowNull] public ElementReference? Element { get; protected set; }

    private bool _hasRendered;
    protected bool NoValidation;
    protected FieldReference Field = new FieldReference(new object(), "nothing");
    protected string ElementId = Guid.NewGuid().ToString();

    protected string CssClass
        => new CSSBuilder()
        .AddClassFromAttributes(AdditionalAttributes)
        .AddClass(this.editContext is not null && !this.NoValidation, ValidationCss)
        .Build();

    protected string ValidationCss
    {
        get
        {
            var isInvalid = this.editContext?.HasValidationMessages(Field) ?? false;
            var isChanged = this.editContext?.FieldHasChanged(Field) ?? false;

            if (isChanged && isInvalid)
                return "is-invalid";

            if (isChanged && !isInvalid)
                return "is-valid";

            return string.Empty;
        }
    }

    protected override ValueTask<bool> OnParametersChangedAsync(bool firstRender)
    {
        if (firstRender)
        {
            if (editContext is null)
                throw new NullReferenceException($"EditContext cannot be null.");

            if (this.ValueExpression is null)
                throw new NullReferenceException($"For cannot be null.");

            this.Field = FieldUtilities.ParseAccessor(this.ValueExpression);

            this.editContext.ValidationStateUpdated += this.OnValidationStateUpdated;
        }

        return ValueTask.FromResult(true);
    }

    protected void OnChanged(ChangeEventArgs e)
    {
        // Need to handle empty string on some types
        // so if the tryconvert fails we invoke ValueChanged with the default value of TValue
        // This should handle nullables correctly!
        if (BindConverter.TryConvertTo<TValue?>(e.Value, System.Globalization.CultureInfo.InvariantCulture, out TValue? result))
            this.ValueChanged.InvokeAsync(result);
        else
            this.ValueChanged.InvokeAsync(default);
    }

    protected void OnValidationStateUpdated(object? sender, ValidateStateEventArgs e)
        => this.StateHasChanged();

    protected void OnFieldChanged(object? sender, string? field)
       => this.StateHasChanged();

    public Task OnAfterRenderAsync()
    {
        if (_hasRendered.SetTrue() && this.IsFirstFocus && this.Element.HasValue)
            this.Element.Value.FocusAsync();

        return Task.CompletedTask;
    }

    public void Dispose()
        => this.editContext.ValidationStateUpdated -= this.OnValidationStateUpdated;
}
