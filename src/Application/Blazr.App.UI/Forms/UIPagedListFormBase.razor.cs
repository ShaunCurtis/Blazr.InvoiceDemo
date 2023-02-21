/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.App.UI;

public partial class UIPagedListFormBase<TRecord, TEntityService> : UIWrapperBase, IAsyncDisposable
    where TRecord : class, new()
    where TEntityService : class, IEntityService
{
    [Inject] protected IServiceProvider ServiceProvider { get; set; } = default!;
    [Inject] protected IUIEntityService<TEntityService> UIEntityService { get; set; } = default!;
    [Inject] protected NavigationManager NavManager { get; set; } = default!;

    [Parameter] public Guid StateId { get; set; } = Guid.Empty;

    public IListPresenter<TRecord, TEntityService> Presenter { get; set; } = default!;

    protected IModalDialog? modalDialog;

    private IDisposable? _disposable;

    public override Task SetParametersAsync(ParameterView parameters)
    {
        // overries the base as we need to make sure we set up the Presenter Service before any rendering takes place
        parameters.SetParameterProperties(this);

        if (!initialized)
        {
            // Gets an instance of the Presenter from the Service Provider
            this.Presenter = ServiceProvider.GetComponentService<IListPresenter<TRecord, TEntityService>>() ?? default!;

            // Ensure we have  Presenter
            ArgumentNullException.ThrowIfNull(nameof(this.Presenter));

            // assign the Presenter if it impleements IDisposable
            _disposable = this.Presenter as IDisposable;
            Presenter.StateId = this.StateId;
        }

        return base.SetParametersAsync(ParameterView.Empty);
    }

    protected async Task OnEditAsync(TRecord record)
    {
        var id = RecordUtilities.GetIdentity(record);
        var options = new ModalOptions();
        options.ControlParameters.Add("Uid", id);

        if (modalDialog is not null && this.UIEntityService.EditForm is not null)
        {
            await modalDialog.ShowAsync(this.UIEntityService.EditForm, options);
            this.StateHasChanged();
        }
        else
            this.NavManager.NavigateTo($"{this.UIEntityService.Url}/edit/{id}");
    }

    protected async Task OnViewAsync(TRecord record)
    {
        var id = RecordUtilities.GetIdentity(record);
        var options = new ModalOptions();
        options.ControlParameters.Add("Uid", id);

        if (modalDialog is not null && this.UIEntityService.ViewForm is not null)
        {
            await modalDialog.ShowAsync(this.UIEntityService.ViewForm, options);
            this.StateHasChanged();
        }
        else
            this.NavManager.NavigateTo($"{this.UIEntityService.Url}/view/{id}");
    }

    public async ValueTask DisposeAsync()
    {
        _disposable?.Dispose();

        if (this.Presenter is IAsyncDisposable asyncDisposable)
            await asyncDisposable.DisposeAsync();
    }
}
