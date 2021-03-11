namespace DBInjectionWithMultiProviders.Domain.Entities
{
    public class UserAddress:BaseEntity
    {
        //overriding Id just for fun
        public int Id { get; set; }
        public string Street1 { get; set; }
        public string Street2 { get; set; }
        public string Landmark { get; set; }
        public string ZipCode { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public UserInfo UserInfo { get; set; }
    }
}