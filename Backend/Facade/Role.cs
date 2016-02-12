namespace Backend.Facade
{
    public class Role
    {
        public Role(Business.Role role)
        {
            Id = role.Id;
            Name = role.Name;
        }

        public int Id { get; set; }
        public string Name { get; set; }
    }
}
