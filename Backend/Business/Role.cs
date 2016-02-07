namespace Backend.Business
{
    public class Role
    {
        public Role(Persistence.Role role)
        {
            Id = role.Id;
            Name = role.Name;
        }

        public int Id { get; private set; }
        public string Name { get; private set; }

        public Persistence.Role ToPersistence()
        {
            return new Persistence.Role
            {
                Id = Id,
                Name = Name
            };
        }
    }
}
