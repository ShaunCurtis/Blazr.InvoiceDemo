﻿/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

namespace Blazr.Core;

public interface ICustomDataBroker
{
    public ValueTask<ItemQueryResult<TRecord>> GetItemAsync<TRecord>(ItemQueryRequest request) where TRecord : class, new();

    public ValueTask<CommandResult> SaveItemAsync<TRecord>(CommandRequest<TRecord> command) where TRecord : class, new();

    public ValueTask<CommandResult> DeleteItemAsync<TRecord>(CommandRequest<TRecord> command) where TRecord : class, new();
}
