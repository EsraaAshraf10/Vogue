using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Vogue.Dashboard.Models;
using VogueCore.Entities;
using VogueCore.Repositories;
using static StackExchange.Redis.Role;
using VogueRepository.Data;
using Vogue.Dashboard.Helpers;
using VogueCore.Specifications;
using VogueCore;

namespace Vogue.Dashboard.Controllers
{
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductController(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        //public async Task<IActionResult> Index()
        //{
        //    var products = await _unitOfWork.Repository<Product>().GetAllAsync();
        //    var mappedProducts = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductViewModel>>(products);
        //    return View(mappedProducts);
        //}
        //public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 10)
        //{
        //    var totalProducts = await _unitOfWork.Repository<Product>().CountAsync();
        //    var products = await _unitOfWork.Repository<Product>().GetAllAsync(pageIndex, pageSize);
        //    var mappedProducts = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductViewModel>>(products);

        //    var viewModel = new ProductListViewModel
        //    {
        //        Products = mappedProducts,
        //        PageIndex = pageIndex,
        //        PageSize = pageSize,
        //        TotalProducts = totalProducts
        //    };

        //    return View(viewModel);
        //}
        public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 10)
        {
            // إعداد بارامترات الفلترة
            var productSpecParams = new ProductSpecParams
            {
                PageIndex = pageIndex,
                PageSize = pageSize
            };

            // إعداد الـSpecifications لفلترة المنتجات
            var spec = new ProductWithBrandAndTypeSpecications(productSpecParams);
            var countSpec = new ProductWithFiltrationForCountAsync(productSpecParams);

            // حساب العدد الإجمالي للمنتجات
            var totalProducts = await _unitOfWork.Repository<Product>().CountAsync(countSpec);

            // جلب المنتجات مع النوع والعلامة التجارية
            var products = await _unitOfWork.Repository<Product>().GetAllWithSpecAsync(spec);

            // تحويل البيانات إلى ProductViewModel
            var mappedProducts = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductViewModel>>(products);

            // إعداد ViewModel
            var viewModel = new ProductListViewModel
            {
                Products = mappedProducts,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalProducts = totalProducts
            };

            return View(viewModel);
        }


        public IActionResult Create()
		{
			return View();
		}
        [HttpPost]
        public async Task<IActionResult> Create(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Image != null)
                    model.PictureUrl = PictureSetting.UploadFile(model.Image, "products");
                else
                    model.PictureUrl = "images/products/hat-react2.png";

                var mappedProduct = _mapper.Map<ProductViewModel, Product>(model);
                await _unitOfWork.Repository<Product>().AddAsync(mappedProduct);
                await _unitOfWork.CompleteAsync();
                return RedirectToAction("Index");
            }
            else
            {
                // Optionally log ModelState errors to see what's failing
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Console.WriteLine(error.ErrorMessage);  // Log or debug this
                }
            }
            return View(model);
        }
        public async Task<IActionResult> Edit(int id)
        {
            var spec = new ProductWithBrandAndTypeSpecications(id);

            //var product = await _unitOfWork.Repository<Product>().GetByIdAsync(id);
            var product = await _unitOfWork.Repository<Product>().GetEntityWithSpecAsync(spec);

            var mappedProduct = _mapper.Map<Product, ProductViewModel>(product);
           
            return View(mappedProduct);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, ProductViewModel model)
        {
            if (id != model.Id)
                return NotFound();
            if (ModelState.IsValid)
            {
                if (model.Image != null)
                {
                    if (model.PictureUrl != null)
                    {
                        PictureSetting.DeleteFile(model.PictureUrl, "products");
                        model.PictureUrl = PictureSetting.UploadFile(model.Image, "products");
                    }
                    else
                    {
                        model.PictureUrl = PictureSetting.UploadFile(model.Image, "products");

                    }

                }
                var mappedProduct = _mapper.Map<ProductViewModel, Product>(model);
                _unitOfWork.Repository<Product>().Update(mappedProduct);
                var result = await _unitOfWork.CompleteAsync();
                if (result > 0)
                    return RedirectToAction("Index");
            }
            return View(model);
        }
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _unitOfWork.Repository<Product>().GetByIdAsync(id);
            var mappedProduct = _mapper.Map<Product, ProductViewModel>(product);
            return View(mappedProduct);
        }
		[HttpPost]
		public async Task<IActionResult> Delete(int id, ProductViewModel model)
		{
			if (id != model.Id)
				return NotFound();
			try
			{
				var product = await _unitOfWork.Repository<Product>().GetByIdAsync(id);
				if (product.PictureUrl != null)
					PictureSetting.DeleteFile(product.PictureUrl, "products");

				_unitOfWork.Repository<Product>().Delete(product);
				await _unitOfWork.CompleteAsync();
				return RedirectToAction("Index");

			}
			catch (System.Exception)
			{

				return View(model);
			}
		}


	}
}
