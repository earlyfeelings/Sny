using Microsoft.AspNetCore.Mvc;
using Sny.Api.Dtos.Models.Goals;
using Sny.Core.GoalsAggregate.Exceptions;
using Sny.Core.Interfaces.Core;
using System.Net;

namespace Sny.Api.Controllers
{
    [ApiController]
    [Route("goals")]
    public class GoalsController : Controller
    {
        private readonly IGoalProvider _gp;

        public GoalsController(IGoalProvider gp)
        {
            this._gp = gp;
        }

        /// <summary>
        /// Returns all goals.
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(IEnumerable<GoalDto>), 200)]
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetList()
        {
            var list = (await _gp.GetGoals()).Select(d => new GoalDto(d.Id, d.Name, d.Active, d.Description));
            return Ok(list);
        }

        /// <summary>
        /// Returns single goal by specified ID.
        /// Returns:
        /// - 404 if the goal was not found.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(GoalDto), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        public async Task< IActionResult> GetById([FromRoute] Guid id)
        {
            try
            {
                var goal = (await _gp.GetGoalById(id));
                var goalMapped = new GoalDto(goal.Id, goal.Name, goal.Active, goal.Description);
                return Ok(goalMapped);
            }
            catch (GoalNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
