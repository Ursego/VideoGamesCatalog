// Boolean values are stored in the DB as 'Y'/'N'.
// This simple enum allows to avoid hardcoding them in C#.

namespace VideoGameCatalog.Api.Util
{
    public enum DbBoolean
    {
        Yes = 'Y',
        No = 'N'
    }
}
