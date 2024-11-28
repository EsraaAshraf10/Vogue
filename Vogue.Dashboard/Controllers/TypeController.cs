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
    [Authorize(Roles = "Admin")]
    public class TypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public TypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Index()
        {
            var Types = await _unitOfWork.Repository<ProductType>().GetAllAsync();
            return View(Types);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductType type)
        {
            try
            {
                await _unitOfWork.Repository<ProductType>().AddAsync(type);
                await _unitOfWork.CompleteAsync();
                return RedirectToAction("Index");
            }
            catch (System.Exception)
            {
                ModelState.AddModelError("Name", "Please Enter A New Type");
                return View("Index", await _unitOfWork.Repository<ProductType>().GetAllAsync());
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            var type = await _unitOfWork.Repository<ProductType>().GetByIdAsync(id);
            _unitOfWork.Repository<ProductType>().Delete(type);
            await _unitOfWork.CompleteAsync();
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Edit(int id)
        {
            var type = await _unitOfWork.Repository<ProductType>().GetByIdAsync(id);


            return View(type);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, ProductType model)
        {
            if (ModelState.IsValid)
            {
                // التحقق مما إذا كان الاسم موجوداً بالفعل ولكن باستثناء العلامة الحالية التي نقوم بتعديلها
                var existingType = await _unitOfWork.Repository<ProductType>()
                    .FindAsync(b => b.Name == model.Name && b.Id != id);

                if (existingType.Count() > 0)
                {
                    // إذا كانت العلامة التجارية موجودة بالفعل
                    ModelState.AddModelError("Name", "The Type is Already Exist");
                    return View(model);
                }

                // جلب العلامة التجارية للتعديل
                var type = await _unitOfWork.Repository<ProductType>().GetByIdAsync(id);

                if (type == null)
                {
                    return NotFound();
                }


                type.Name = model.Name;

                _unitOfWork.Repository<ProductType>().Update(type);
                await _unitOfWork.CompleteAsync();

                return RedirectToAction(nameof(Index));
            }


            return View(model);
        }


    }
}
