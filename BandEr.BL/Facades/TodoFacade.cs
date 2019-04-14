using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BandEr.Common.Exceptions;
using BandEr.DAL;
using BandEr.DAL.DTO;
using BandEr.DAL.Entity;
using BandEr.Infrastructure.Auth.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BandEr.BL.Facades
{
    public class TodoFacade
    {
        private readonly BandErDbContext _context;
        private readonly ILogger _log;
        private readonly ITenantProvider _tenantProvider;
        private readonly IMapper _mapper;

        public TodoFacade(BandErDbContext context, ILogger<TodoFacade> log, ITenantProvider tenantProvider, IMapper mapper)
        {
            _context = context;
            _log = log;
            _tenantProvider = tenantProvider;
            _mapper = mapper;
        }

        public async Task<List<ValueListDto>> GetAsync()
        {
            var id = int.Parse(_tenantProvider.GetUserId());
            var q = _mapper.ProjectTo<ValueListDto>(_context.Values
                .Where(x => x.Owner.Id == id));
            return await q.ToListAsync();
        }
        public async Task<ValueDetailDto> GetAsync(int id)
        {
            var userId = int.Parse(_tenantProvider.GetUserId());
            var entity = await _context.Values                
                .Where(x => x.Owner.Id == userId)
                .FirstOrDefaultAsync(x => x.Id == id)
                ?? throw new NotFoundException<ValueEntity>(id);
            return _mapper.Map<ValueDetailDto>(entity);
        }
        public async Task AddAsync(string value)
        {
            var userId = int.Parse(_tenantProvider.GetUserId());
            _context.Values.Add(new ValueEntity { OwnerId = userId, Value = value});
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(int id, string value)
        {
            var userId = int.Parse(_tenantProvider.GetUserId());
            var entity = await _context.Values
                .Include(x => x.Owner)
                .Where(x => x.Owner.Id == userId)
                .FirstOrDefaultAsync(x => x.Id == id)
                ?? throw new NotFoundException<ValueEntity>(id);
            entity.Value = value;
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var userId = int.Parse(_tenantProvider.GetUserId());
            var entity = await _context.Values
                .Include(x => x.Owner)
                .Where(x => x.Owner.Id == userId)
                .FirstOrDefaultAsync(x => x.Id == id)
                ?? throw new NotFoundException<ValueEntity>(id);
            _context.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
