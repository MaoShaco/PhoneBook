using System.Collections.Generic;
using System.Linq;
using PhoneBookDao.Dao;
using PhoneBookDao.Dao.Impl;
using PhoneBookDao.Model;

namespace PhoneBookServices.Services.Impl
{
    public class ContactServicesImpl : IContactServices
    {
        private static readonly IContactDao ContactDao = new ContactDaoImpl();

        public long InsertOrUpdateContact(Contact contact)
        {
            if (contact.Id == 0)
                return ContactDao.Insert(contact);
            ContactDao.Update(contact);
            return contact.Id;
        }

        public Contact FindById(long id)
        {
            return ContactDao.GetById(id);
        }

        public List<Contact> GetAllContacts()
        {
            return ContactDao.GetAllContacts();
        }

        public void DeleteContact(Contact contact)
        {
            ContactDao.Delete(contact);
        }

        public Contact GetContactByNumber(PhoneNumber number)
        {
            return ContactDao.GetContactsByNumber(number).First();
        }
    }
}