using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using StockApi.Dtos;
using StockApi.Modals;
using VidepGame.Data;

namespace StockApi.Services
{
    public class SessionService(AppDbContext context): ISessionService
    {

        public async Task<List<Session>> GetSessionsAsync()
        {
            return await context.sessions.ToListAsync();
        }

        public async Task<List<Session>> GetMySessionsAsync(string username)
        {
            var sessions = await context.sessions.Where(s => s.Username == username).ToListAsync();
            return sessions;
        }
        public async Task<Session?> GetSessionAsync(int id)
        {
            var foundSession = await context.sessions.FindAsync(id);

            return foundSession;
        }
        public async Task<Session> AddSessionAsync(SessionCreateDto session)
        {
            // for jerry rigging let usrname just be emploeid
            var openSession = await context.sessions.Where(s => s.Username == session.Username && s.Locked == false).FirstOrDefaultAsync();

            if(openSession is not null){
                return openSession;
            }
       
            var newSession = new Session
            {
                SessionDate = session.SessionDate,
                Username = session.Username,
                CreatedAt = session.CreatedAt,
                ExportedAt = session.ExportedAt,
                Locked = session.Locked,
            };

            context.sessions.Add(newSession);
            await context.SaveChangesAsync();

            return newSession;
        }
        public async Task<bool> DeleteSessionAsync(int id)
        {
            var foundSession = await context.sessions.FindAsync(id);

            if(foundSession is null)
            {
                return false;
            }

            context.sessions.Remove(foundSession);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateSessionAsync(int id, SessionUpdateDto session)
        {
            var foundSession = await context.sessions.FindAsync(id);
            if(foundSession is null)
            {
                return false;
            }

            foundSession.SessionDate = session.SessionDate;
            foundSession.Username = session.Username;
            foundSession.ExportedAt = session.ExportedAt;
            foundSession.Locked = session.Locked;

            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> LockSessionAsync(int id)
        {
            var foundSession = await context.sessions.FindAsync(id);

            if(foundSession is null)
            {
                return false;
            }

            foundSession.Locked = true;
            await context.SaveChangesAsync();
            return true;
        }
    }
}
