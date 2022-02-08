﻿using VendingMachine.ProductCtx.Model.DTOs;
using VendingMachine.Domain.BoundedContext.Products.Model.ValueObjects;
using VendingMachine.Domain.BoundedContext.Products.Model.Entities;
using VendingMachine.Domain.BoundedContext.Products.Model.Enums;

namespace VendingMachine.ProductCtx.Mappings
{
    public class ProductMap : IProductMap
    {
        public Product Map(ProductDTO dto)
        {
            Enum.TryParse(dto.Currency, out Currency currency);

            return new Product()
            {
                Id    = dto.Id,
                Name  = dto.Name,
                Price = new Money(currency, dto.Price)
            };
        }

        public ProductDTO InverseMap(Product entity)
        {
            return new ProductDTO()
            {
                Id       = entity.Id,
                Name     = entity.Name,
                Price    = entity.Price.Amount,
                Currency = Enum.GetName(typeof(Currency), entity.Price.Currency)
            };
        }
    }
}
