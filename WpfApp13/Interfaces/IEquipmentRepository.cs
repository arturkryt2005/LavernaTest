using System.Collections.Generic;
using System.Threading.Tasks;

namespace WpfApp13.Interfaces
{
    public interface IEquipmentRepository
    {
        Task<List<Equipment>> GetAllAsync();
        Task<Equipment> GetByIdAsync(int id);
        Task AddAsync(Equipment equipment);
        Task UpdateAsync(Equipment equipment);
        Task DeleteAsync(int id);
        Task SaveChangesAsync();

        Task<List<Type>> GetTypesAsync();
        Task<List<Status>> GetStatusesAsync();
    }
}
