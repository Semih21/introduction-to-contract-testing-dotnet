namespace CustomerConsumer.Models
{
    public class Address
    {
        public Guid Id { get; set; }
        public string AddressType { get; set; }
        public string Street { get; set; }
        public int Number { get; set; }
        public string City { get; set; }
        public int ZipCode { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
    }
}
