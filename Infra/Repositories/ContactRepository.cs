using Domain;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories
{
    public class ContactRepository(Context context) : IContactRepository
    {
        private readonly Context _context = context;

        public void Create(Contact contact)
        {
            _context.Contacts.Add(contact);
        }

        public void Delete(Contact contact)
        {
            try
            {
                _context.Contacts.Remove(contact);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<Contact>> FindAllContacts()
        {
            try
            {
                return await _context.Contacts.ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Contact?> FindContact(Guid id)
        {
            try
            {
                return await _context.Contacts.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<Contact>> FindContactsByDDD(int ddd)
        {
            try
            {
                return [.._context.Contacts.Where(e => e.DDDId == ddd)];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DDD?> FindDDD(int dddId)
        {
            return await _context.DDDs.FirstOrDefaultAsync(e => e.Id == dddId);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}