using BeastsGym.BLL.Interfaces;
using BeastsGym.BLL.ViewModels.MemberViewModel;
using BeastsGym.DAL.Entities;
using BeastsGym.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeastsGym.BLL.Classes
{
    public class MemberServices : IMemberServices
    {
        private readonly IgenericRepository<Member> memberRepository;
        private readonly IgenericRepository<Plan> planRepository;
        private readonly IgenericRepository<Booking> bookingRepository;
        private readonly IgenericRepository<Membership> membershipRepository;

        public IgenericRepository<HealthRecord> HealthRecordRepository { get; }

        public MemberServices(IgenericRepository<Member> memberRepository, IgenericRepository<Membership> membershipRepository, IgenericRepository<Plan> planRepository, IgenericRepository<HealthRecord> HealthRecordRepository, IgenericRepository<Booking> BookingRepository)
        {
            this.memberRepository = memberRepository;
            this.planRepository = planRepository;
            this.HealthRecordRepository = HealthRecordRepository;
            bookingRepository = BookingRepository;
        }
        //GET
        public async Task<IEnumerable<MemberViewModel>> GetAllMembersAsync(CancellationToken ct = default)
        {
            var members = await memberRepository.GetAll(false, ct);

            if (!members.Any()) return [];

            var MembersViewModel = members.Select(m => new MemberViewModel()
            {
                Id = m.Id,
                Name = m.Name,
                Email = m.Email,
                Phone = m.PhoneNumber,
                Photo = m.Photo,
                Gender = m.Gender.ToString()
            });
            return MembersViewModel;
        }

        public async Task<MemberViewModel?> GetMemberDetailsAsync(int memberId, CancellationToken ct = default)
        {
            //member + membership + plan
            var member = await memberRepository.GetById(memberId, ct);
            if(member == null) return null;

            var MemberVM = new MemberViewModel()
            {
                Name = member.Name,
                Email = member.Email,
                Phone = member.PhoneNumber,
                Photo = member.Photo,
                DateOfBirth = member.DateOfBirth.ToShortDateString(),
                Gender = member.Gender.ToString(),
                Address = $"{member.Address.BuildingNumber} - {member.Address.Street} - {member.Address.City}"
            };
            var ActiveMemberShip = await membershipRepository.FirstOrDefaultAsync(mb=>mb.MemberId == memberId && mb.EndDate > DateTime.Now, false, ct);

            if(ActiveMemberShip is not null)
            {
                var activePlan = await planRepository.GetById(ActiveMemberShip.PlanId, ct);
                MemberVM.PlanName = activePlan?.PlanName;
                MemberVM.MembershipStartDate = ActiveMemberShip.CreatedAt.ToShortDateString();
                MemberVM.MembershipEndDate = ActiveMemberShip.EndDate.ToShortDateString();
            }

            return MemberVM;
        }

        public async Task<HealthRecordViewModel?> GetMemberHealthRecordAsync(int memberId, CancellationToken ct = default)
        {
            var Record = await HealthRecordRepository.FirstOrDefaultAsync(r => r.MemberId == memberId, false, ct);
            
            if (Record is null) return null;

            return new HealthRecordViewModel()
            {
                Weight = Record.Weight,
                Height = Record.Height,
                BloodType = Record.BloodType,
                Note = Record.Note
            };
        }

        public async Task<MemberToUpdateViewModel> GetMemberToUpdateAsync(int memberId, CancellationToken ct = default)
        {
            var Member = await memberRepository.GetById(memberId, ct);

            if(Member is null) return null;

            return new MemberToUpdateViewModel()
            {
                Name = Member.Name,
                Email = Member.Email,
                Phone = Member.PhoneNumber,
                Photo = Member.Photo,
                BuildingNumber = Member.Address.BuildingNumber,
                Street = Member.Address.Street,
                City = Member.Address.City
            };
        }

        //POST
        public async Task<bool> CreateMemberAsync(CreateMemberViewModel model, CancellationToken ct = default)
        {
            var emailExists = await memberRepository.AnyAsync(m => m.Email == model.Email,false, ct);
            var phoneExists = await memberRepository.AnyAsync(m => m.PhoneNumber == model.Phone, false, ct);

            if (emailExists || phoneExists) return false;

            var Member = new Member()
            {
                Name = model.Name,
                Email = model.Email,
                PhoneNumber = model.Phone,
                DateOfBirth = model.DateOfBirth,
                Gender = model.Gender,
                Address = new Address()
                {
                    BuildingNumber = model.BuildingNumber,
                    Street = model.Street,
                    City = model.City
                },
                HealthRecord = new HealthRecord()
                {
                    Weight = model.HealthRecordViewModel.Weight,
                    Height = model.HealthRecordViewModel.Height,
                    BloodType = model.HealthRecordViewModel.BloodType,
                    Note = model.HealthRecordViewModel.Note
                }
            };
            memberRepository.Add(Member);
            var Result = await memberRepository.CompleteAsync();
            return Result > 0;
        }
       
        public async Task<bool> UpdateMemberDetailsAsync(int id, MemberToUpdateViewModel model, CancellationToken ct = default)
        {
            var member = await memberRepository.GetById(id, ct);

            if(member ==null) return false;
            if (await memberRepository.AnyAsync(m => m.Email == model.Email && m.Id != id)) return false;
            if (await memberRepository.AnyAsync(m => m.PhoneNumber == model.Phone && m.Id != id)) return false;

            member.Email = model.Email;
            member.PhoneNumber = model.Phone;
            member.Name = model.Name!;
            member.Address.BuildingNumber = model.BuildingNumber;
            member.Address.Street = model.Street;
            member.Address.City = model.City;
            member.UpdatedAt = DateTime.Now;
            
            memberRepository.Update(member);

            var Result = await memberRepository.CompleteAsync();
            return Result > 0;
        }


        public async Task<bool> DeleteMemberAsync(int memberId, CancellationToken ct = default)
        {
            var HasFutureSessions = await bookingRepository.AnyAsync(b => b.MemberId == memberId && b.Session.EndDate > DateTime.Now, false, ct);

            if (HasFutureSessions) return false;

            memberRepository.Delete(memberId);

            var Result = await memberRepository.CompleteAsync();
            return Result > 0;
        }
    }
}
