using GymSystem.Models;
using GymSystem.Repository;

namespace GymSystem.Service
{
    public class LoginService
    {
        private readonly CustomerRepository cusRepo;
        private readonly CoachRepository coaRepo;
        private readonly BookingRepository bookRepo;
        private readonly ExerciseDataRepository dataRepo;
        public LoginService(CustomerRepository cusRepo, CoachRepository coaRepo, BookingRepository bookRepo, ExerciseDataRepository dataRepo)
        {
            this.cusRepo = cusRepo;
            this.coaRepo = coaRepo;
            this.bookRepo = bookRepo;
            this.dataRepo = dataRepo;
        }
        public Customer getCustomerById(int id)
        {
            Customer customer = cusRepo.FindCustomerById(id);
            List<Booking> bookings = bookRepo.FindBookingsByCustomerId(id);
            customer.exerciseData=dataRepo.FindExerciseDataByCustomerId(id);
            customer.favoriteType = cusRepo.FindFavoriteType(customer.id);
            customer.bookings=bookings;
            return customer;
        }
        public Customer getCustomerByUsername(string username)
        {
            Customer customer = cusRepo.FindCustomerByName(username);
            if (customer == null) return null; 
            List<Booking> bookings = bookRepo.FindBookingsByCustomerId(customer.id);
            customer.bookings = bookings;
            customer.exerciseData = dataRepo.FindExerciseDataByCustomerId(customer.id);
            customer.favoriteType=cusRepo.FindFavoriteType(customer.id);
            return customer;
        }
        public bool ValidateCustomer(string username,string password)
        {
            Customer customer=getCustomerByUsername(username);
            return password == customer.pwd;
        }
        public Coach getCoachById(int id)
        {
            Coach coach = coaRepo.FindCoachById(id);
            return coach;
        }
        public Coach getCoachByUsername(string username)
        {
            Coach coach = coaRepo.FindCoachByName(username);
            return coach;
        }
        public bool ValidateCoach(string username, string password)
        {
            Coach coach = getCoachByUsername(username);
            if (coach == null) return false;
            return password == coach.pwd;
        }
        
    }
}
