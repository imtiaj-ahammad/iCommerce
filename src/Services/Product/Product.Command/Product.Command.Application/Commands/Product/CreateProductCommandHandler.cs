using AutoMapper;
using MediatR;

namespace Product.Command.Application;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CreateProductCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<int> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {

            var productObj0 = _mapper.Map<Product.Command.Domain.Product>(command);

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
