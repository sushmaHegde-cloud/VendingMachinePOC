﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace VendingMachinePOC
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository iproductRepository;
        private readonly IProductInventoryRepository iproductInventoryRepository;
        public ProductService(IProductRepository productRepository, IProductInventoryRepository productInventoryRepository)
        {
            if (productRepository == null) throw new ArgumentNullException("productRepository parameter is null");
            if (productInventoryRepository == null) throw new ArgumentNullException("productInventoryRepository parameter is null");

            iproductRepository = productRepository;
            iproductInventoryRepository = productInventoryRepository;
        }

        public int GetProductQuantity(string code)
        {
            var quantities = iproductInventoryRepository.GetInventory();
            return quantities[code];
        }

        public Product GetProduct(string code)
        {
            return GetAllProducts().FirstOrDefault(x => x.Code == code);
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return iproductRepository.GetProductList();
        }

        public void UpdateProductQuantity(string code)
        {            
            iproductInventoryRepository.UpdateInventory(code);
        }        
    }
}