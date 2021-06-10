using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using scb10x_assignment_party_haan_backend.Domain.AggregatesModel.UserAggregate;
using scb10x_assignment_party_haan_backend.Domain.DTOs.PartyHaanAggregate;
using scb10x_assignment_party_haan_backend.Domain.DTOs.UserAggregate;
using scb10x_assignment_party_haan_backend.Infrastructure.DataContexts;

namespace scb10x_assignment_party_haan_backend.Infrastructure.Repositories
{
    public class PartyHaanRepository : IPartyHaanRepository
    {
        private readonly IServiceScope _scope;
        private readonly IConfiguration _configuration;
        private readonly PartyHaanContext _partyHaanContext;

        public PartyHaanRepository(IConfiguration configuration, IServiceProvider services)
        {
            _scope = services.CreateScope();
            _configuration = configuration;
            _partyHaanContext = _scope.ServiceProvider.GetRequiredService<PartyHaanContext>();
        }

        public async Task<List<PartyHaansResponse>> GetParties(int userId)
        {
            var result =  await _partyHaanContext.PartiesHaan
                                .OrderByDescending(m => m.CreatedDate)
                                .Select(m => new PartyHaansResponse
                                {
                                    Id = m.Id,
                                    Name = m.Name,
                                    Detail = m.Detail,
                                    Limit = m.Limit,
                                    CreatedDate = m.CreatedDate,
                                    CreatedBy = m.CreatedBy,
                                    UpdatedDate = m.UpdatedDate,
                                    UpdatedBy = m.UpdatedBy,
                                    PartyHaanMembers = m.PartyHaanMembers,
                                    Joined = m.PartyHaanMembers.FirstOrDefault(m => m.UserRefId.Equals(userId)) != null,
                                    CurrentMember = m.PartyHaanMembers.Count()
                                })
                                .ToListAsync();

            return result;
        }


        public async Task<PartyHaansResponse> GetPartHaanById(int userId, int id)
        {
            var result = await _partyHaanContext.PartiesHaan
                                .Include(m => m.PartyHaanMembers)
                                .FirstOrDefaultAsync(m => m.Id.Equals(id));

            if (result is null)
                throw new Exception("ไม่พบ partyHaan");

            return new PartyHaansResponse
            {
                Id = result.Id,
                Name = result.Name,
                Detail = result.Detail,
                Limit = result.Limit,
                CreatedDate = result.CreatedDate,
                CreatedBy = result.CreatedBy,
                UpdatedDate = result.UpdatedDate,
                UpdatedBy = result.UpdatedBy,
                PartyHaanMembers = result.PartyHaanMembers,
                Joined = result.PartyHaanMembers.FirstOrDefault(m => m.UserRefId.Equals(userId)) != null,
                CurrentMember = result.PartyHaanMembers.Count
            };
        }

        public async Task<PartyHaansResponse> CreatePartyHaan(int userId, PartyHaanRequest request)
        {
            try
            {
                var partyHaan = new PartyHaan
                {
                    Name = request.Name,
                    Limit = request.Limit,
                    Detail = request.Detail,
                    CreatedBy = userId,
                    CreatedDate = DateTime.Now,
                    PartyHaanMembers = new List<PartyHaanMember>
                    {
                        new PartyHaanMember
                        {
                            UserRefId = userId,
                            CreatedBy = userId,
                            CreatedDate = DateTime.Now
                        }
                    }

                };
                _partyHaanContext.PartiesHaan.Add(partyHaan);

                await _partyHaanContext.SaveChangesAsync();

                return new PartyHaansResponse
                {
                    Id = partyHaan.Id,
                    Name = partyHaan.Name,
                    Detail = partyHaan.Detail,
                    Limit = partyHaan.Limit,
                    CreatedDate = partyHaan.CreatedDate,
                    CreatedBy = partyHaan.CreatedBy,
                    UpdatedDate = partyHaan.UpdatedDate,
                    UpdatedBy = partyHaan.UpdatedBy,
                    PartyHaanMembers = partyHaan.PartyHaanMembers,
                    Joined = partyHaan.PartyHaanMembers.FirstOrDefault(m => m.UserRefId.Equals(userId)) != null,
                    CurrentMember = partyHaan.PartyHaanMembers.Count
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        public async Task<PartyHaanResponse> UpdatePartyHaan(int userId, int partyHaanId, PartyHaanRequest request)
        {
            try
            {
                var partyHaan = await _partyHaanContext.PartiesHaan
                                .FirstOrDefaultAsync(m => m.Id.Equals(partyHaanId) && m.CreatedBy.Equals(userId));
                if (partyHaan == null)
                    throw new Exception("ไม่สามารถอัปเดต partyHaan");

                if (!string.IsNullOrEmpty(request.Name))
                    partyHaan.Name = request.Name;

                if (!string.IsNullOrEmpty(request.Detail))
                    partyHaan.Detail = request.Detail;

                if (request.Limit != null)
                {
                    if (request.Limit < partyHaan.PartyHaanMembers.Count)
                        throw new Exception($"ไม่สามารถอัปเดตข้อมูลได้เนื่องจากขณะนี้มีสมาชิก {partyHaan.PartyHaanMembers.Count} คน");

                    partyHaan.Limit = request.Limit;
                }

                await _partyHaanContext.SaveChangesAsync();

                return new PartyHaanResponse
                {
                    Id = partyHaan.Id
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<PartyHaanResponse> DeletePartyHaan(int userId, int partyHaanId)
        {
            try
            {
                var partyHaan = await _partyHaanContext
                                        .PartiesHaan
                                        .Include(m => m.PartyHaanMembers)
                                        .FirstOrDefaultAsync(m => m.Id.Equals(partyHaanId) && m.CreatedBy.Equals(userId));

                if (partyHaan == null)
                    throw new Exception("ไม่พบ partyHaan");

                _partyHaanContext.PartiesHaan.Remove(partyHaan);

                await _partyHaanContext.SaveChangesAsync();

                return new PartyHaanResponse
                {
                    Id = partyHaan.Id,
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<PartyHaanResponse> AddPartyHaanMember(int userId, int partyHaanId)
        {
            try
            {
                var countMember = _partyHaanContext
                                        .PartyHaanMembers
                                        .Count(m => m.PartHaanRefId.Equals(partyHaanId));


                var partyHaan = await _partyHaanContext.PartiesHaan
                                .FirstOrDefaultAsync(m => m.Id.Equals(partyHaanId));
                if(partyHaan == null)
                    throw new Exception("ไม่พบ partyHaan");

                if (countMember >= partyHaan.Limit)
                    throw new Exception("เกินจำนวนสมาชิก");

                var relation = new PartyHaanMember
                {
                     PartHaanRefId = partyHaanId,
                     UserRefId = userId,
                     CreatedBy = userId,
                     CreatedDate = DateTime.Now   
                };

                _partyHaanContext.PartyHaanMembers.Add(relation);

                await _partyHaanContext.SaveChangesAsync();

                return new PartyHaanResponse
                {
                    Id = partyHaan.Id,
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<PartyHaanResponse> DeletePartyHaanMember(int userId, int partyHaanId)
        {
            try
            {
                var relation = await _partyHaanContext
                                        .PartyHaanMembers
                                        .FirstOrDefaultAsync(m => m.PartHaanRefId.Equals(partyHaanId) && m.UserRefId.Equals(userId));

                if (relation == null)
                    throw new Exception("ไม่พบ partyHaan");

                var partyHaan = await _partyHaanContext.PartiesHaan
                                .FirstOrDefaultAsync(m => m.Id.Equals(partyHaanId));

                if (partyHaan == null)
                    throw new Exception("ไม่พบ partyHaan");


                _partyHaanContext.PartyHaanMembers.Remove(relation);

                await _partyHaanContext.SaveChangesAsync();

                return new PartyHaanResponse
                {
                    Id = partyHaan.Id,
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
