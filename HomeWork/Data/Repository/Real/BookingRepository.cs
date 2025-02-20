using HomeWork.Data.Domain;
using Microsoft.AspNetCore.Identity;
using HomeWork.Data.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace HomeWork.Data.Repository.Real
{
    public class BookingRepository
    {
        private readonly DentalDbContext _dbContext;
        private readonly UserManager<UserRegistration> _userManager;
        private readonly SignInManager<UserRegistration> _signInManager;
        private readonly IConfiguration config;
        public BookingRepository(DentalDbContext dbContext, IConfiguration config, UserManager<UserRegistration> userManager, SignInManager<UserRegistration> signInManager)
        {
            _dbContext = dbContext;
            _signInManager = signInManager;
            _userManager = userManager;
            this.config = config;
        }

        public async Task<List<RefDentalProcedures>> GetAllProceduresAsync()
        {
            return await _dbContext.RefDentalProcedures.ToListAsync();
        }
    }
}
