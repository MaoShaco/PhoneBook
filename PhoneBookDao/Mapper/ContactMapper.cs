using System;
using System.Collections.Generic;
using PhoneBookDao.Model;

namespace PhoneBookDao.Mapper
{
    public static class ContactMapper
    {
        public static Contact MakeContact(List<Object> objects)
        {
            return new Contact
            {
                Name = (string) objects[0],
                Id = (int) objects[1]
            };
        }
    }
}
