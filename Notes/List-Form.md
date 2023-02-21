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

