using MediatR;
using Product.Query.Domain;

namespace Product.Query.Application;

public class GetProductsQueryHandler  : IRequestHandler<GetProductsQuery, IEnumerable<Product.Query.Domain.Product>>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetProductsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<IEnumerable<Product.Query.Domain.Product>> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        var productList =  _unitOfWork.ProductQueryRepository.GetAll();
        if(productList == null)
        {
            return null;
        }
        return productList;
    }
}
