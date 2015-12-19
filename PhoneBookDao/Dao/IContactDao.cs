using System.Collections.Generic;
using PhoneBookDao.Model;

namespace PhoneBookDao.Dao
{
    public interface IContactDao
    {
        Contact GetById(long id);

        List<Contact> GetContactsByNumber(PhoneNumber phoneNumberEntity);

        long Insert(Contact contactEntity);

        void Update(Contact contactEntity);

        void Delete(Contact contactEntity);

        List<Contact> GetAllContacts();
    }
}
