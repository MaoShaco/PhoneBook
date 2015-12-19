using System.Collections.Generic;
using PhoneBookDao.Dao;
using PhoneBookDao.Dao.Impl;
using PhoneBookDao.Model;

namespace PhoneBookServices.Services.Impl
{
    public class PhoneNumberServicesImpl : IPhoneNumberServices
    {
        private static readonly IPhoneNumberDao NumberDao = new PhoneNumberDaoImpl();

        public long InsertOrUpdateNumber(PhoneNumber phoneNumber)
        {
            if (phoneNumber.Id == 0)
                return NumberDao.Insert(phoneNumber);
            NumberDao.Update(phoneNumber);
            return phoneNumber.Id;
        }

        public PhoneNumber FindById(long id)
        {
            return NumberDao.GetById(id);
        }

        public List<PhoneNumber> GetNumbersByContact(Contact contact)
        {
            return NumberDao.GetNumbersByContact(contact);
        }

        public void DeletePhoneNumber(PhoneNumber phoneNumber)
        {
            NumberDao.Delete(phoneNumber);
        }
    }
}