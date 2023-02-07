/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

namespace Blazr.UI.BlazrGrid;

public interface IBlazrGridItem<TGridItem>
{
    public Guid ComponentId { get; }

    public void GetItemHeaderContent(RenderTreeBuilder builder);

    public void GetItemRowContent(RenderTreeBuilder builder, TGridItem item);
}
