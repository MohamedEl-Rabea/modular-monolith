namespace WT.B2C.API.BuildingBlocks.Runtime;

public class ContextUser
{
    public long Id { get; set; }
    public static ContextUser Empty()
    {
        return new ContextUser();
    }
}