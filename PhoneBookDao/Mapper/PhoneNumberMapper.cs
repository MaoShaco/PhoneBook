using System;
using System.Collections.Generic;
using PhoneBookDao.Model;

namespace PhoneBookDao.Mapper
{
    public static class PhoneNumberMapper
    {
        public static PhoneNumber MakePhoneNumber(List<Object> objects)
        {
            return new PhoneNumber
            {
                Number = (string) objects[0],
                ContactId = (int) objects[1],
                Note = (string) objects[2],
                Id = (int) objects[3],
            };
        }
    }
}
