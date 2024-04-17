using MediatR;

namespace Product.Command.Application;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CreateProductCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<int> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            var productObj = new Product.Command.Domain.Product();
            //productObj.Id = Guid.NewGuid();
            productObj.Name = command.Name;
            productObj.Description = command.Description;
            productObj.Price = command.Price;
            _unitOfWork.ProductCommandRepository.Add(productObj);
            await _unitOfWork.SaveChangesAsync();
            return productObj.Id;
        }
    }
