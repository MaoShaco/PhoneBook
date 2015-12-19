
namespace PhoneBookDao.Model
{
    public class PhoneNumber
    {
        public long Id { get; set; }

        public string Number { get; set; }

        public long ContactId { get; set; }

        public string Note { get; set; }


        public override string ToString()
        {
            return $"{Number} -- {Note}";
        }
    }
}
