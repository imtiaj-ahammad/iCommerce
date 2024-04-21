using MediatR;

namespace Product.Query.Application;

public class GetProductByIdQuery : IRequest<Product.Query.Domain.Product>
{
    public int Id { get; set; }
}
