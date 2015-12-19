using System.Collections.Generic;
using PhoneBookDao.Model;

namespace PhoneBookDao.Dao
{
    public interface IPhoneNumberDao
    {
        PhoneNumber GetById(long id);

        List<PhoneNumber> GetNumbersByContact(Contact contactEntity);

        long Insert(PhoneNumber phoneEntity);

        void Update(PhoneNumber phoneEntity);

        void Delete(PhoneNumber phoneEntity);
    }
}
