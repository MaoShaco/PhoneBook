using System.Collections.Generic;
using PhoneBookDao.Model;

namespace PhoneBookServices.Services
{
    public interface IPhoneNumberServices
    {
        long InsertOrUpdateNumber(PhoneNumber phoneNumber);

        PhoneNumber FindById(long id);

        List<PhoneNumber> GetNumbersByContact(Contact contact);

        void DeletePhoneNumber(PhoneNumber phoneNumber);
    }
}
