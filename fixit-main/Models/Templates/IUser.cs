namespace fixit_main.Models.Templates
{
    public interface IUser
    {
        public int ID { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public string Nombre { get; set; }
    }
}
