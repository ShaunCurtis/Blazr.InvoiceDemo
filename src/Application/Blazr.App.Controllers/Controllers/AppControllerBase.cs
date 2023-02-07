/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.App.Controllers;

[ApiController]
public abstract class AppControllerBase<TRecord>
    : ControllerBase
    where TRecord : class, new()
{
    protected IDataBroker _dataBroker;

    public AppControllerBase(IDataBroker dataBroker)
        => _dataBroker = dataBroker;

    [Route("/api/[controller]/listquery")]
    [HttpPost]
    public async Task<ListQueryResult<TRecord>> ListQuery([FromBody] ListQueryRequest query)
        => await _dataBroker.GetItemsAsync<TRecord>(query);

    [Route("/api/[controller]/recordquery")]
    [HttpPost]
    public async Task<ItemQueryResult<TRecord>> RecordQuery([FromBody] ItemQueryRequest query)
        => await _dataBroker.GetItemAsync<TRecord>(query);

    [Route("/api/[controller]/addrecordcommand")]
    [HttpPost]
    public async Task<CommandResult> AddRecordCommand([FromBody] CommandRequest<TRecord> command)
        => await _dataBroker.CreateItemAsync<TRecord>(command);

    [Route("/api/[controller]/updaterecordcommand")]
    [HttpPost]
    public async Task<CommandResult> UpdateRecordCommand([FromBody] CommandRequest<TRecord> command)
        => await _dataBroker.UpdateItemAsync<TRecord>(command);

    [Route("/api/[controller]/deleterecordcommand")]
    [HttpPost]
    public async Task<CommandResult> DeleteRecordCommand([FromBody] CommandRequest<TRecord> command)
        => await _dataBroker.DeleteItemAsync<TRecord>(command);
}