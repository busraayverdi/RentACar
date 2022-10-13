using Application.Features.Brands.Dtos;
using Application.Features.Brands.Rules;
using Application.Features.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Brands.Commands.CreateBrand
{
    public class CreateBrandCommand:IRequest<CreatedBrandDto>
    {
        public string Name { get; set; }

        public class CreateBrandCommandHandler : IRequestHandler<CreateBrandCommand, CreatedBrandDto>
        {
            private readonly IBrandRepository _brandRepository;
            private readonly IMapper _mapper;
            private readonly BrandBusinessRules _brandBuisnessRules;

            public CreateBrandCommandHandler(IBrandRepository brandRepository, IMapper mapper,
                BrandBusinessRules brandBuisnessRules)
            {
                _brandRepository=brandRepository;
                _mapper=mapper;
                _brandBuisnessRules=brandBuisnessRules;
            }

            public async Task<CreatedBrandDto> Handle(CreateBrandCommand request, CancellationToken cancellationToken)
            {

                await _brandBuisnessRules.BrandNameCanNotBeDuplicatedWhenInserted(request.Name); //exception

                Brand mappedBrand = _mapper.Map<Brand>(request);   //data 
                Brand createdBrand = await _brandRepository.AddAsync(mappedBrand);
                CreatedBrandDto createdBrandDto = _mapper.Map<CreatedBrandDto>(createdBrand);

                return createdBrandDto;//dönüş

            }
        }
    }
}
