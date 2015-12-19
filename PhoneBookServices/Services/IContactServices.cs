using System.Collections.Generic;
using PhoneBookDao.Model;

namespace PhoneBookServices.Services
{
    public interface IContactServices
    {
        long InsertOrUpdateContact(Contact contact);

        Contact FindById(long id);

        List<Contact> GetAllContacts();

        void DeleteContact(Contact contact);

        Contact GetContactByNumber(PhoneNumber number);
    }
}
