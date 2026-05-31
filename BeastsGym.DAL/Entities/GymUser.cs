using BeastsGym.DAL.Entities.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeastsGym.DAL.Entities
{
    public abstract class GymUser : BaseEntity
    {
        [Required, MaxLength(50)]
        public string Name { get; set; } = null!;

        [Required, MaxLength(50), EmailAddress]
        public string Email { get; set; } = null!;

        [Required, MaxLength(20), RegularExpression(@"^01[0-2,5]{1}[0-9]{8}$", ErrorMessage = "Egyptian Phone Numbers Only")]
        public string PhoneNumber { get; set; } = null!;

        public DateOnly DateOfBirth { get; set; }
        public Gender Gender { get; set; }

        public Address Address { get; set; }

    }

    [Owned]
    public class Address
    {
        public int BuildingNumber { get; set; }
        [Required, MaxLength(30)]
        public string City { get; set; } = null!;

        [Required, MaxLength(30)]
        public string Street { get; set; } = null!;
    }
}
