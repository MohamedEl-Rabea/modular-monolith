namespace WT.Customers.Portal.BuildingBlocks.Runtime;

public class ContextUser
{
    public long Id { get; set; }
    public static ContextUser Empty()
    {
        return new ContextUser();
    }
}