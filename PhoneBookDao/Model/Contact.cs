using System;
using static System.String;

namespace PhoneBookDao.Model
{
    public class Contact : IComparable<Contact>
    {
        public long Id { get; set; }

        public string Name { get; set; }


        public override string ToString()
        {
            return $"{Name}";
        }

        public int CompareTo(Contact obj)
        {
            return Compare(Name, obj.Name, StringComparison.Ordinal);
        }
    }
}
