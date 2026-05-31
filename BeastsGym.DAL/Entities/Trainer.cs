using BeastsGym.DAL.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeastsGym.DAL.Entities
{
    public class Trainer : GymUser
    {
        public int Capacity { get; set; }
        public DateOnly HireDate { get; set; }
        public Speciality Speciality { get; set; }

        public ICollection<Session> Sessions = new HashSet<Session>();
    }
}
