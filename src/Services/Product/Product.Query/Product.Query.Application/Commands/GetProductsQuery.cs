using MediatR;

namespace Product.Query.Application;

public class GetProductsQuery : IRequest<IEnumerable<Product.Query.Domain.Product>>
{

}
