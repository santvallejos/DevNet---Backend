using DevNet_DataAccessLayer.Data;
using DevNet_DataAccessLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevNet_DataAccessLayer.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly DevnetDBContext _context;
        protected readonly IRepository<T> _repository;

        public Repository(DevnetDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }



        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<bool> AddAsync(T entity)
        {
            try
            {
                await _context.Set<T>().AddAsync(entity);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            try
            {
                _context.Set<T>().Update(entity);
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public async Task<bool> DeleteAsync(T entity)
        {
            try
            {
                _context.Set<T>().Remove(entity);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
