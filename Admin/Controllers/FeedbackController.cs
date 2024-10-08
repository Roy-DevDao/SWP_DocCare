using Admin.Models;
using Microsoft.AspNetCore.Mvc;

namespace Admin.Controllers
{
    public class FeedbackController : Controller
    {
        private readonly DocCareContext _context;

        public FeedbackController(DocCareContext context)
        {
            _context = context;
        }

        // Index action: Fetches the first 3 feedbacks initially
        public IActionResult Index()
        {
            var feedbacks = _context.Feedbacks.OrderByDescending(f => f.DateCmt).Take(3).ToList();
            return View(feedbacks);
        }

        // LoadMoreFeedbacks action: Fetches the next batch of feedbacks
        [HttpPost]
        public IActionResult LoadMoreFeedbacks(int skip)
        {
            var feedbacks = _context.Feedbacks
                .OrderByDescending(f => f.DateCmt)
                .Skip(skip)
                .Take(3)  // Load the next 3 feedbacks
                .ToList();

            if (!feedbacks.Any())
            {
                return Content("");  // Return an empty result when no more feedbacks
            }

            // Render the partial view (_FeedbackPartial.cshtml) with the new feedback batch
            return PartialView("_FeedbackPartial", feedbacks);
        }
    }

}
