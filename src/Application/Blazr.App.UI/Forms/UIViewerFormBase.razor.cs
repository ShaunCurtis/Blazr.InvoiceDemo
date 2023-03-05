/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.App.UI;

public abstract partial class UIViewerFormBase<TRecord, TEntityService> : UIWrapperBase
    where TRecord : class, new()
    where TEntityService : class, IEntityService
{
    [Inject] protected IReadPresenter<TRecord> Presenter { get; set; } = default!;
    [Inject] protected NavigationManager NavManager { get; set; } = default!;
    [Inject] protected IUIEntityService<TEntityService> UIEntityService { get; set; } = default!;

    [CascadingParameter] private IModalDialog? ModalDialog { get; set; }
    [Parameter] public Guid Uid { get; set; }

    protected string ExitUrl { get; set; } = "/";

    public async override Task SetParametersAsync(ParameterView parameters)
    {
        parameters.SetParameterProperties(this);
        if (!initialized)
            await this.Presenter.LoadAsync(Uid);

        await base.SetParametersAsync(ParameterView.Empty);
    }

    protected Task OnExit()
    {
        if (this.ModalDialog is null)
            this.NavManager.NavigateTo(this.ExitUrl);

        ModalDialog?.Close(new ModalResult());
        return Task.CompletedTask;
    }
}
