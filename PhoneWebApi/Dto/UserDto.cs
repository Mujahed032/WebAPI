namespace PhoneWebApi.Dto
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;

        }
    }
}
