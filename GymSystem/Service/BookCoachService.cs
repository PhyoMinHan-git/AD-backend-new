using GymSystem.Repository;

namespace GymSystem.Service
{
    public class BookCoachService
    {
        private readonly CustomerRepository cusRepo;
        private readonly CoachRepository coaRepo;
        private readonly BookingRepository bookRepo;
        public BookCoachService(CustomerRepository cusRepo, CoachRepository coaRepo, BookingRepository bookRepo)
        {
            this.cusRepo = cusRepo;
            this.coaRepo = coaRepo;
            this.bookRepo = bookRepo;
        }
        
    }
}
