using HomeWork.Data.Domain.ValueObjects;

namespace HomeWork.Data.Repository.Abstract
{
    public interface IBookingRepository
    {
        Task<List<RefDentalProcedures>> GetAllProceduresAsync();
    }
}
