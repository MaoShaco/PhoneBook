using System;
using System.Collections.Generic;
using PhoneBookDao.Mapper;
using PhoneBookDao.Model;

namespace PhoneBookDao.Dao.Impl
{
    public class ContactDaoImpl : IContactDao
    {
        private const int ColumnsCount = 2;
        private const string TableName = "contact";


        public Contact GetById(long id)
        {
            return
                ContactMapper.MakeContact(PostgreConnection.QueryOnTableWithParams(TableName,
                    new Dictionary<string, string> {{"id", id.ToString()}})
                    .GetRange(0, ColumnsCount));
        }

        public List<Contact> GetContactsByNumber(PhoneNumber phoneNumberEntity)
        {
            List<Object> objects =
                PostgreConnection.QueryOnTableWithParams(TableName,
                    new Dictionary<string, string> {{"id", phoneNumberEntity.ContactId.ToString()}});

            var contacts = new List<Contact>();

            for (int i = 0; i < objects.Count/ColumnsCount; i++)
                contacts.Add(ContactMapper.MakeContact(objects.GetRange(i, ColumnsCount)));


            return contacts;
        }

        public long Insert(Contact contactEntity)
        {
            return PostgreConnection.InsertOnTable(TableName, contactEntity);
        }

        public void Update(Contact contactEntity)
        {
            PostgreConnection.UpdateOnTable(TableName, contactEntity, (int) contactEntity.Id);
        }

        public void Delete(Contact contactEntity)
        {
            PostgreConnection.DeleteOnTable(TableName, contactEntity, (int) contactEntity.Id);
        }

        public List<Contact> GetAllContacts()
        {
            List<Object> objects = PostgreConnection.QueryAllOnTable(TableName);

            var contacts = new List<Contact>();

            for (int i = 0; i < objects.Count; i += ColumnsCount)
                contacts.Add(ContactMapper.MakeContact(objects.GetRange(i, ColumnsCount)));

            return contacts;
        }
    }
}
