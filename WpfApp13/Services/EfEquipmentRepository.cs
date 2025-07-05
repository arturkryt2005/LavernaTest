using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using WpfApp13.Interfaces;

namespace WpfApp13.Services
{
    public class EfEquipmentRepository : IEquipmentRepository
    {
        private readonly OfficeEntities _db = new OfficeEntities();

        public async Task<List<Equipment>> GetAllAsync()
        {
            return await _db.Equipment
                .Include(e => e.Type)
                .Include(e => e.Status)
                .ToListAsync();
        }
        /// <summary>
        /// Получение оборудования по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Equipment> GetByIdAsync(int id)
        {
            return await _db.Equipment
                .Include(e => e.Type)
                .Include(e => e.Status)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task AddAsync(Equipment equipment)
        {
            _db.Equipment.Add(equipment);
            await SaveChangesAsync();
        }

        public async Task UpdateAsync(Equipment equipment)
        {
            var entry = _db.Entry(equipment);
            if (entry.State == EntityState.Detached)
                _db.Equipment.Attach(equipment);

            entry.State = EntityState.Modified;
            await SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var equip = await _db.Equipment.FindAsync(id);
            if (equip != null)
            {
                _db.Equipment.Remove(equip);
                await SaveChangesAsync();
            }
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }

        public async Task<List<Type>> GetTypesAsync() => await _db.Type.ToListAsync();
        public async Task<List<Status>> GetStatusesAsync() => await _db.Status.ToListAsync();
    }
}
