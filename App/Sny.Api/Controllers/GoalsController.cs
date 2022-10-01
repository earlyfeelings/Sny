using Microsoft.AspNetCore.Mvc;
using Sny.Api.Dtos.Models.Goals;
using Sny.Core.GoalsAggregate.Exceptions;
using Sny.Core.Interfaces.Core;
using Sny.Core.Interfaces.Infrastructure;
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
        public async Task<IActionResult> GetById([FromRoute] Guid id)
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

        /// <summary>
        /// Adds goal 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("{id}")]
        [ProducesResponseType(typeof(AddResponseGoalDto), 200)]
        public async Task<IActionResult> Add(AddRequestGoalDto model)
        {
            var goal = (await _gp.AddGoal(model.Name, model.Active, model.Description));
            var goalMapped = new AddResponseGoalDto(goal.Id, goal.Name, goal.Active, goal.Description);
            return Ok(goalMapped);
        }

        /// <summary>
        /// Edits goal
        /// Returns:
        /// - 404 if the goal was not found.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("")]
        [ProducesResponseType(typeof(EditResponseGoalDto), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        public async Task<IActionResult> Edit(EditRequestGoalDto model)
        { 
            try
            {
                var goal = (await _gp.EditGoal(model.Id, model.Name, model.Active, model.Description));
                var goalMapped = new EditResponseGoalDto(goal.Id, goal.Name, goal.Active, goal.Description);
                return Ok(goalMapped);
            }
            catch (GoalNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Remove goal
        /// Returns:
        /// - 404 if the goal was not found.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            try 
            {
                _gp.DeleteGoal(id);
                return Ok();
            }
            catch (GoalNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Remove goal
        /// Returns:
        /// - 404 if the goal was not found.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="activate"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("{id}/{activate}")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> ChangeActiveGoal([FromRoute] Guid id, [FromRoute] bool activate)
        {
            try
            {
                _gp.ChangeActiveGoal(id, activate);
                return Ok();
            }
            catch (GoalNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
