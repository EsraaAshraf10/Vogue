using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Vogue.Dashboard.Models;
using VogueCore;
using VogueCore.Entities;
using VogueCore.Repositories;
using VogueRepository.Data;

namespace Vogue.Dashboard.Controllers
{
    [Authorize(Roles ="Admin")]
	public class BrandController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;

		public BrandController(IUnitOfWork unitOfWork)
        {
			_unitOfWork = unitOfWork;
		}
        public async Task<IActionResult> Index()
		{
			var Brands = await _unitOfWork.Repository<ProductBrand>().GetAllAsync();
			return View(Brands);
		}

        [HttpPost]
        public async Task<IActionResult> Create(ProductBrand brand)
        {
            try
            {
                await _unitOfWork.Repository<ProductBrand>().AddAsync(brand);
                await _unitOfWork.CompleteAsync();
                return RedirectToAction("Index");
            }
            catch (System.Exception)
            {
                ModelState.AddModelError("Name", "Please Enter A New Brand");
                return View("Index", await _unitOfWork.Repository<ProductBrand>().GetAllAsync());
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            var brand = await _unitOfWork.Repository<ProductBrand>().GetByIdAsync(id);
            _unitOfWork.Repository<ProductBrand>().Delete(brand);
            await _unitOfWork.CompleteAsync();
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Edit(int id)
        {
            var brand = await _unitOfWork.Repository<ProductBrand>().GetByIdAsync(id);

           
            return View(brand);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, ProductBrand model)
        {
            if (ModelState.IsValid)
            {
                // التحقق مما إذا كان الاسم موجوداً بالفعل ولكن باستثناء العلامة الحالية التي نقوم بتعديلها
                var existingBrand = await _unitOfWork.Repository<ProductBrand>()
                    .FindAsync(b => b.Name == model.Name && b.Id != id);

                if (existingBrand.Count()>0 )
                {
                    // إذا كانت العلامة التجارية موجودة بالفعل
                    ModelState.AddModelError("Name", "The Brand is Already Exist");
                    return View(model);
                }

                // جلب العلامة التجارية للتعديل
                var brand = await _unitOfWork.Repository<ProductBrand>().GetByIdAsync(id);

                if (brand == null)
                {
                    return NotFound(); 
                }

              
                brand.Name = model.Name;

                _unitOfWork.Repository<ProductBrand>().Update(brand);
                await _unitOfWork.CompleteAsync();

                return RedirectToAction(nameof(Index));
            }

            
            return View(model);
        }


    }
}
