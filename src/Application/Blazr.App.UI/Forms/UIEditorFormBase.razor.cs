/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

namespace Blazr.App.UI;

public abstract partial class UIEditorFormBase<TRecord, TEditContext, TEntityService> : UIWrapperBase, IDisposable
    where TRecord : class, new()
    where TEditContext : class, IEditContext, IRecordEditContext<TRecord>, new()
    where TEntityService : class, IEntityService
{
    [Inject] protected IEditPresenter<TRecord, TEditContext> Presenter { get; set; } = default!;
    [Inject] protected NavigationManager NavManager { get; set; } = default!;
    [Inject] protected IUIEntityService<TEntityService> UIEntityService { get; set; } = default!;

    [CascadingParameter] private IModalDialog? ModalDialog { get; set; }
    [Parameter] public Guid Uid { get; set; }

    protected TEditContext EditContext = default!;
    private IDisposable? _disposable;
    protected string ExitUrl { get; set; } = "/";
    protected virtual RenderFragment? HeaderRightContent {get;} 

    public async override Task SetParametersAsync(ParameterView parameters)
    {
        parameters.SetParameterProperties(this);
        if (!initialized)
        {
            EditContext = (TEditContext)this.Presenter.EditStateContext;
            await this.Presenter.LoadAsync(Uid);
            this.EditContext.EditStateChanged += OnEditStateChanged;
            _disposable = this.NavManager.RegisterLocationChangingHandler(this.OnLocationChanging);
        }

        await base.SetParametersAsync(ParameterView.Empty);
    }

    protected async Task OnSave()
        => await this.Presenter.SaveItemAsync();

    protected Task OnExit()
    {
        if (this.ModalDialog is null)
            this.NavManager.NavigateTo(this.ExitUrl);

        ModalDialog?.Close(new ModalResult());
        return Task.CompletedTask;
    }

    protected void OnEditStateChanged(object? sender, EventArgs e)
        => this.StateHasChanged();

    protected async Task OnReset()
        => await this.Presenter.ResetItemAsync();

    private ValueTask OnLocationChanging(LocationChangingContext context)
    {
        if (Presenter.EditStateContext.IsDirty)
            context.PreventNavigation();

        return ValueTask.CompletedTask;
    }

    public void Dispose()
    {
        _disposable?.Dispose();
        this.EditContext.EditStateChanged -= OnEditStateChanged;
    }
}
