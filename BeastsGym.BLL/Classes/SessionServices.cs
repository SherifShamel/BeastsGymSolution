using AutoMapper;
using BeastsGym.BLL.Common;
using BeastsGym.BLL.Interfaces;
using BeastsGym.BLL.ViewModels.SessionViewModels;
using BeastsGym.DAL.Entities;
using BeastsGym.DAL.Repositories.classes;
using BeastsGym.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeastsGym.BLL.Classes
{
    public class SessionServices : ISessionServices

    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public SessionServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<Result> CreateSessionAsync(CreateSessionViewModel model, CancellationToken ct = default)
        {
            if (model.EndDate <= model.StartDate) return Result.Validation("End date must be after start date.");
            if (model.StartDate <= DateTime.Now) return Result.Validation("Start date must be in the future.");

            var TrainerRepo = unitOfWork.GetRepository<Trainer>();
            var Trainer = TrainerRepo.GetById(model.TrainerId, ct);

            if (Trainer is null) return Result.NotFound("Trainer not found.");

            var CategoryRepo = unitOfWork.GetRepository<Category>();
            var Category = CategoryRepo.GetById(model.CategoryId, ct);

            if (Category is null) return Result.NotFound("Category not found.");

            var session = mapper.Map<CreateSessionViewModel, Session>(model);

            var SessionRepo = unitOfWork.GetRepository<Session>();

            SessionRepo.Add(session);

            var RowEffected = await unitOfWork.CompleteAsync();

            return RowEffected > 0 ? Result.Ok() : Result.Fail("Failed to create session.");
        }

        public async Task<IEnumerable<SessionViewModel>> GetAllSessionsAsync(CancellationToken ct)
        {
            //1. get sessions.
            //2. find trainer for each session.
            //3. find category for each session.

            var Sessions = await unitOfWork.SessionRepository.GetAllSessionsWithTrainerAndCategoryAsync(ct);

            if (!Sessions.Any()) return null;

            Sessions = Sessions.OrderByDescending(s => s.StartDate);

            var MappedSessions = mapper.Map<IEnumerable<Session>, IEnumerable<SessionViewModel>>(Sessions);

            foreach (var session in MappedSessions)
            {
                session.AvailableSlots = session.Capacity - await unitOfWork.SessionRepository.GetCountOfBookedSlotsAsync(session.Id, ct);
            }
            return MappedSessions;
        }

        public async Task<IEnumerable<CategorySelectViewModel>> GetCategoriesForDropDownAsync(CancellationToken ct = default)
        {
            var Categories = await unitOfWork.GetRepository<Category>().GetAll(false, ct);
            return mapper.Map<IEnumerable<Category>, IEnumerable<CategorySelectViewModel>>(Categories);
        }

        public async Task<SessionViewModel?> GetSessionByIdAsync(int sessionId, CancellationToken ct = default)
        {
            var session = await unitOfWork.SessionRepository.GetSingleSessionWithTrainerAndCategoryAsync(sessionId, ct);

            if (session is null) return null;

            var mappedSession = mapper.Map<Session, SessionViewModel>(session);

            mappedSession.AvailableSlots = mappedSession.Capacity - await unitOfWork.SessionRepository.GetCountOfBookedSlotsAsync(mappedSession.Id, ct);

            return mappedSession;
        }

        public async Task<UpdateSessionViewModel> GetSessionToUpdateAsync(int sessionId, CancellationToken ct)
        {
            var session = await unitOfWork.GetRepository<Session>().GetById(sessionId, ct);
            if (session is null) return null;

            if (!await IsSessionValidForUpdate(session, ct)) return null;

            return mapper.Map<Session, UpdateSessionViewModel>(session);
        }

        public async Task<IEnumerable<TrainerSelectViewModel>> GetTrainersForDropDownAsync(CancellationToken ct = default)
        {
            var Trainers = await unitOfWork.GetRepository<Trainer>().GetAll(false, ct);
            return mapper.Map<IEnumerable<Trainer>, IEnumerable<TrainerSelectViewModel>>(Trainers);
        }

        public async Task<Result> RemoveSessionAsync(int sessionId, CancellationToken ct)
        {
            var repo = unitOfWork.GetRepository<Session>(); //session Repo

            var session = await repo.GetById(sessionId, ct);

            if(session is null) return Result.NotFound("Session Not Found");
            if(session.EndDate >= DateTime.Now) return Result.Fail("Cannot remove session that has already ended.");

            var bookedCount = await unitOfWork.SessionRepository.GetCountOfBookedSlotsAsync(sessionId, ct);

            if (bookedCount > 0)
                return Result.Fail("Can't delete a Session That has bookings");

            repo.Delete(sessionId);
            var affectedRows = await unitOfWork.CompleteAsync();

            return affectedRows > 0 ? Result.Ok() : Result.Fail("Failed to remove session.");

        }

        public async Task<Result> UpdateSessionAsync(int id, UpdateSessionViewModel model, CancellationToken ct)
        {
            var sessionRepo = unitOfWork.GetRepository<Session>();

            var session = await sessionRepo.GetById(id, ct);

            if (session is null) return Result.NotFound("Session Not Found");

            if (session.StartDate <= DateTime.Now)
                return Result.Fail("Can Not Edit Session That Has Already Started");

            var bookedCount = await unitOfWork.SessionRepository.GetCountOfBookedSlotsAsync(id, ct);
            if (bookedCount > 0)
            {
                return Result.Fail("Cannot Edit Session That Has Booked Slots");
            }

            if (model.EndDate <= model.StartDate) return Result.Validation("End date must be after start date.");
            if (model.StartDate <= DateTime.Now) return Result.Validation("Start date must be in the future.");

            var TrainerRepo = unitOfWork.GetRepository<Trainer>();
            var Trainer = TrainerRepo.GetById(model.TrainerId, ct);

            if (Trainer is null) return Result.NotFound("Trainer not found.");

            session.UpdatedAt = DateTime.Now;

            mapper.Map(model, session);
            sessionRepo.Update(session);

            var EffectedRows = await unitOfWork.CompleteAsync();

            return EffectedRows > 0 ? Result.Ok() : Result.Fail("Failed to update session.");

        }

        private async Task<bool> IsSessionValidForUpdate(Session session, CancellationToken ct)
        {
            if (session.StartDate <= DateTime.Now) return false;

            var booked = await unitOfWork.SessionRepository.GetCountOfBookedSlotsAsync(session.Id, ct);
            return booked == 0;
        }
    }
}
