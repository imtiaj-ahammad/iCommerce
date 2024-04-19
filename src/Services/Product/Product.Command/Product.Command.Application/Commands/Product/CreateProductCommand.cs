using MediatR;

namespace Product.Command.Application;

public class CreateProductCommand  : IRequest<int>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public decimal? Price { get; set; }
}
