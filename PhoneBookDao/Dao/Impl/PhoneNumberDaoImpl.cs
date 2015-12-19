using System;
using System.Collections.Generic;
using PhoneBookDao.Mapper;
using PhoneBookDao.Model;

namespace PhoneBookDao.Dao.Impl
{
    public class PhoneNumberDaoImpl : IPhoneNumberDao
    {
        private const int ColumnsCount = 4;
        private const string TableName = "phone";

        public PhoneNumber GetById(long id)
        {
            return
                PhoneNumberMapper.MakePhoneNumber(PostgreConnection.QueryOnTableWithParams(TableName,
                    new Dictionary<string, string> {{"id", id.ToString()}})
                    .GetRange(0, 3));
        }

        public List<PhoneNumber> GetNumbersByContact(Contact contactEntity)
        {
            List<Object> objects =
                PostgreConnection.QueryOnTableWithParams(TableName,
                    new Dictionary<string, string> {{"contactid", contactEntity.Id.ToString()}});

            var phones = new List<PhoneNumber>();
            for (int i = 0; i < objects.Count; i += ColumnsCount)
                phones.Add(PhoneNumberMapper.MakePhoneNumber(objects.GetRange(i, ColumnsCount)));

            return phones;
        }

        public long Insert(PhoneNumber phoneEntity)
        {
            return PostgreConnection.InsertOnTable(TableName, phoneEntity);
        }

        public void Update(PhoneNumber phoneEntity)
        {
            PostgreConnection.UpdateOnTable(TableName, phoneEntity, (int) phoneEntity.Id);
        }

        public void Delete(PhoneNumber phoneEntity)
        {
            PostgreConnection.DeleteOnTable(TableName, phoneEntity, (int) phoneEntity.Id);
        }
    }
}
