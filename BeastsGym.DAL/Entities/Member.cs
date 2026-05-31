namespace BeastsGym.DAL.Entities
{
    public class Member : GymUser
    {
        public string Photo { get; set; }
        public HealthRecord HealthRecord { get; set; } = null!;

        public ICollection<Membership> Memberships = new HashSet<Membership>();

        public ICollection<Booking> Bookings { get; set; } = new HashSet<Booking>();
    }
}