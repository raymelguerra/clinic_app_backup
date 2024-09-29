using ClinicApp.Infrastructure.Interfaces;
using ClinicApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ClinicApp.Api.Respositories
{

    namespace ClinicApp.Infrastructure.Repositories
    {
        public class Repository : IRepository
        {
            private readonly InsuranceContext _context;

            public Repository(InsuranceContext context)
            {
                _context = context;
            }

            public async Task<IEnumerable<T>> GetAllAsync<T>() where T : class
            {
                return await _context.Set<T>().ToListAsync();
            }

            //public async Task<T> GetByIdAsync<T>(int id) where T : class
            //{
            //    var entity = await _context.Set<T>().FindAsync(id);
            //    if (entity != null)
            //    {
            //        var entry = _context.Entry(entity);
            //        // Load many to many relationships
            //        foreach (var navigation in entry.Navigations)
            //        {
            //            if (navigation.Metadata.IsCollection)
            //            {
            //                await navigation.LoadAsync();
            //            }
            //        }
            //    }
            //    return entity;
            //}

            public async Task<T> GetByIdAsync<T>(int id) where T : class
            {
                var entity = await _context.Set<T>().FindAsync(id);

                if (entity != null)
                {
                    var entry = _context.Entry(entity);
                    await LoadNavigationProperties(entry, 3, new HashSet<object>());
                }

                return entity;
            }

            private async Task LoadNavigationProperties(EntityEntry entry, int level, HashSet<object> loadedEntities)
            {
                if (level <= 0 || entry == null) return;

                // Verifica si la entidad ya ha sido cargada para evitar ciclos
                if (!loadedEntities.Add(entry.Entity)) return;

                foreach (var navigation in entry.Navigations)
                {
                    try
                    {
                        // Carga la navegación si no está ya cargada
                        if (navigation.Metadata.IsCollection)
                        {
                            if (navigation.CurrentValue is IEnumerable<object> collection)
                            {
                                foreach (var item in collection)
                                {
                                    // Carga cada elemento de la colección
                                    await LoadNavigationProperties(_context.Entry(item), level - 1, loadedEntities);
                                }
                            }
                        }
                        else
                        {
                            // Carga la entidad individual
                            await navigation.LoadAsync();
                            await LoadNavigationProperties(_context.Entry(navigation.CurrentValue), level - 1, loadedEntities);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Manejo de excepciones (opcional)
                        // Puedes registrar el error o manejarlo según tu lógica
                        Console.WriteLine($"Error loading navigation property: {ex.Message}");
                    }
                }
            }



            public async Task<T> AddAsync<T>(T entity) where T : class
            {
                _context.Set<T>().Add(entity);

                var entry = _context.Entry(entity);
                // handler relationships many to many before adding
                foreach (var navigation in entry.Navigations)
                {
                    if (navigation.Metadata.IsCollection)
                    {
                        await navigation.LoadAsync();
                    }
                }

                await _context.SaveChangesAsync();
                return entity;
            }

            public async Task<T> UpdateAsync<T>(T entity) where T : class
            {
                _context.Set<T>().Update(entity);

                var entry = _context.Entry(entity);
                // Load many to many relationships before updating
                foreach (var navigation in entry.Navigations)
                {
                    if (navigation.Metadata.IsCollection)
                    {
                        await navigation.LoadAsync();
                    }
                }

                await _context.SaveChangesAsync();
                return entity;
            }

            public async Task<T> DeleteAsync<T>(T entity) where T : class
            {
                _context.Set<T>().Remove(entity);

                var entry = _context.Entry(entity);
                // Load many to many relationships before deleting
                foreach (var navigation in entry.Navigations)
                {
                    if (navigation.Metadata.IsCollection)
                    {
                        await navigation.LoadAsync();
                    }
                }

                await _context.SaveChangesAsync();
                return entity;
            }
        }
    }

}
