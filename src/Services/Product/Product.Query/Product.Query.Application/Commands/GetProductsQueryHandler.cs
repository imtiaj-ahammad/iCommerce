using MediatR;
using Product.Query.Domain;

namespace Product.Query.Application;

public class GetProductsQueryHandler  : IRequestHandler<GetProductsQuery, IEnumerable<Product.Query.Domain.Product>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cacheService;

    public GetProductsQueryHandler(IUnitOfWork unitOfWork, ICacheService cacheService)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<IEnumerable<Product.Query.Domain.Product>> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        //check cache data
        var cacheData = _cacheService.GetData<IEnumerable<Product.Query.Domain.Product>>("products");

        if(cacheData != null && cacheData.Count() > 0)
        {
            return (IEnumerable<Product.Query.Domain.Product>)cacheData;
        }
        cacheData = /*await*/ _unitOfWork.ProductQueryRepository.GetAll();

        //Set Expiry time
        var expiryTime = DateTimeOffset.Now.AddSeconds(30);
        _cacheService.SetData<IEnumerable<Product.Query.Domain.Product>>("products",cacheData, expiryTime);

        return (IEnumerable<Product.Query.Domain.Product>)cacheData;
    }
}
