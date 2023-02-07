﻿/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.Core;

public static class RecordUtilities
{
    public static object GetIdentity(object value)
    {
        if (value is IGuidIdentity guidIdentity)
            return guidIdentity.Uid;

        if (value is IIntIdentityRecord intIdentity)
            return intIdentity.Id;

        if (value is ILongIdentityRecord longIdentity)
            return longIdentity.Id;

        if (value is IStringIdentityRecord stringIdentity)
            return stringIdentity.Id;

        return new();
    }
}
