using GymSystem.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GymSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly BookingRepository bookRepo;
        public BookingController(BookingRepository bookingRepo)
        {
            this.bookRepo = bookingRepo;
        }
        [HttpPost("cancel")]
        public IActionResult CancelBooking([FromBody] CancelRequest request)
        {
            bookRepo.DeleteBooking(request.id);
            return Ok();
        }
    }
    public class CancelRequest
    {
        public int id { get; set; }
        public int customerId { get; set; }
    }
}
