# List Forms

List Forms use a templete component to boilerplate most of the code.

`UIPagedListFormBase` consists of a Razor file containing the Ui render fragments and a code behind file containing the code.

The form inherits from the `UIWrapperBase` component.  It defines a `TemplateContent` render fragment that lays out the wrapper markup.  The concrete implementation content is rendered within the grid as `this.Content`.  Note the cascading `ListController`, `BlazrPagingControl` and the Modal Dislog component.  

```csharp
@namespace Blazr.App.UI
@typeparam TRecord where TRecord : class, new()
@typeparam TEntityService where TEntityService : class, IEntityService
@inherits UIWrapperBase

@code {
    protected override RenderFragment TemplatedContent => (__builder) =>
    {
        <PageTitle>List of @this.UIEntityService.PluralDisplayName</PageTitle>

        <CascadingValue Value=this.Presenter.ListController IsFixed>
            <div class="container-fluid mb-2">
                <div class="row">
                    <div class="col-12 mb-2">
                        @this.TitleContent
                    </div>
                </div>
                <div class="row">
                    <div class="col-12 col-lg-8">
                        <BlazrPagingControl TRecord="TRecord" DefaultPageSize="20" />
                    </div>
                </div>
            </div>
            <BlazrGrid TGridItem="TRecord">
                @this.Content
            </BlazrGrid>

        </CascadingValue>
        <BaseModalDialog @ref=modalDialog />
    };

    protected virtual RenderFragment TitleContent => (__builder) =>
    {
        <h4>List of @this.UIEntityService.PluralDisplayName</h4>
    };
}
```



```csharp
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

            if (this.Presenter is null)
                throw new NullReferenceException($"No Presenter cound be created.");

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
```