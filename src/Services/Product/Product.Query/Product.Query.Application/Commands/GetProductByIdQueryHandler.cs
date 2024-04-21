using MediatR;

namespace Product.Query.Application;

public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Product.Query.Domain.Product>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetProductByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<Product.Query.Domain.Product> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        var product = /*await*/ _unitOfWork.ProductQueryRepository.GetById(query.Id);
        if(product == null )
        {
            return null;
        }
        return product;
    }
}
