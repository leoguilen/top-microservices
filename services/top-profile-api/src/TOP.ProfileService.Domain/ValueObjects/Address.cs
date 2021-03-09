namespace TOP.ProfileService.Domain.ValueObject
{
    public class Address : ValueObject<Address>
    {
        public Address() { }
        public Address(string street, string district, string city, string state, string cep)
        {
            Street = street;
            District = district;
            City = city;
            State = state;
            Cep = cep;                
        }

        public string Street { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Cep { get; set; }
    }
}
