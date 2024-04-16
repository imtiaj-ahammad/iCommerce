namespace Product.Command.Domain;

public class Product : EntityBase
{
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public decimal? Price { get; private set; }
}
