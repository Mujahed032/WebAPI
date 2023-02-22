namespace PhoneWebApi.Models
{
    public class Phone
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Category Category { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
