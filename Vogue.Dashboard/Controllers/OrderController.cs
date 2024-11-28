using Microsoft.AspNetCore.Mvc;
using VogueCore;
using VogueCore.Entities.order_Aggregate;
using VogueCore.Repositories;
using VogueCore.Specifications;
using VogueCore.Specifications.Order_Spec;

namespace Vogue.Dashboard.Controllers
{
    public class OrderController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public OrderController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Index()
        {
            var spec = new OrderSpecifications();
            var orders = await unitOfWork.Repository<Order>().GetAllWithSpecAsync(spec);
            return View(orders);
        }

        public async Task<IActionResult> GetUserOrders(string buyerEmail)
        {
            var spec = new OrderSpecifications(buyerEmail);
            var orders = await unitOfWork.Repository<Order>().GetAllWithSpecAsync(spec);

            return View("Index", orders);
        }
    }
}
