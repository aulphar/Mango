using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Mango.Services.ProductAPI.Data;
using Mango.Services.ProductAPI.Models;
using Mango.Services.ProductAPI.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.ProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductAPIController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly ResponseDto _response;
        private readonly IMapper _mapper;

        public ProductAPIController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _response = new ResponseDto();
            _mapper = mapper;
        }

        [HttpGet]
        public ResponseDto GetAllProducts()
        {
            try
            {
                List<Product> products = _db.Products.ToList();
                _response.Result = _mapper.Map<List<ProductDto>>(products);
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.Message = e.Message;
            }

            return _response;
        }

        [HttpGet]
        [Route("getbyname/{name}")]
        public ResponseDto GetProductByName(string name)
        {
            try
            {
                Product product = _db.Products.First(u => u.Name == name);
                _response.Result = _mapper.Map<ProductDto>(product);
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.Message = e.Message;
            }

            return _response;
        }

        [HttpGet]
        [Route("{id:int}")]
        public ResponseDto GetProductById(int id)
        {
            try
            {
                Product product = _db.Products.First(u => u.ProductId == id);
                _response.Result = _mapper.Map<ProductDto>(product);
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.Message = e.Message;
            }

            return _response;
        }

        [HttpPost]
        public ResponseDto CreateProduct([FromBody]ProductDto productDto)
        {
            try
            {
                Product productobj = _mapper.Map<Product>(productDto);
                _db.Products.Add(productobj);
                _db.SaveChanges();
                _response.Result = _mapper.Map<ProductDto>(productobj);

            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.Message = e.Message;
            }

            return _response;
        }

        [HttpDelete]
        [Route("{id:int}")]
        public ResponseDto DeleteProduct(int id)
        {
            try
            {
                Product product = _db.Products.First(u => u.ProductId == id);
                _db.Products.Remove(product);
                _db.SaveChanges();

            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.Message = e.Message;
            }

            return _response;
        }

        [HttpPut]
        public ResponseDto UpdateProduct([FromBody] ProductDto productDto)
        {
            try
            {
                Product productobj = _mapper.Map<Product>(productDto);
                _db.Products.Update(productobj);
                _db.SaveChanges();
                _response.Result = _mapper.Map<ProductDto>(productobj);

            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.Message = e.Message;
            }

            return _response;
        }
    }
}
