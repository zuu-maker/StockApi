using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using StockApi.Dtos;
using StockApi.Modals;
using StockApi.Services;

namespace StockApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionsController(ISessionService service) : ControllerBase
    {

        [HttpGet]
        public async Task<ActionResult<List<Session>>> GetSessions()
        {
            var sessions = await service.GetSessionsAsync();
            return Ok(sessions);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Session>> GetSession(int id)
        {
            var session = await service.GetSessionAsync(id);

            if(session is null)
            {
                return NotFound("Session not found");
            }
            return Ok(session);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteSession(int id)
        {
            var result = await service.DeleteSessionAsync(id);
            return Ok(result);
        }


        [HttpPost]
        public async Task<ActionResult<Session>> AddSession(SessionCreateDto newSession)
        {
            var result = await service.AddSessionAsync(newSession);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<bool>> UpdateSession(int id, SessionUpdateDto session)
        {
            var result = await service.UpdateSessionAsync(id, session);
            return Ok(result);
        }
     }
}
