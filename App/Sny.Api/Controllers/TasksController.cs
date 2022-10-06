using Microsoft.AspNetCore.Mvc;
using Sny.Core.GoalsAggregate.Exceptions;
using Sny.Core.Interfaces.Core;
using static Sny.Api.Dtos.Models.Tasks.Tasks;

namespace Sny.Api.Controllers
{
    [ApiController]
    [Route("tasks")]
    public class TasksController : Controller
    {
        private readonly ITaskProvider _tp;

        public TasksController(ITaskProvider tp)
        {
            this._tp = tp;
        }

        /// <summary>
        /// Returns all tasks by specified ID of goal.
        /// Returns:
        /// - 404 if the task was not found.
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(IEnumerable<TaskDto>), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        [HttpGet]
        [Route("byGoal/{id}")]
        public async Task<IActionResult> GetListByGoalId([FromRoute] Guid id)
        {
            try
            {
                var list = (await _tp.GetTasksByGoalId(id)).Select(d => new TaskDto(d.Id, d.Name, d.Description, d.DueDate, d.IsCompleted, d.GoalId));
                return Ok(list);
            }
            catch (GoalNotFoundException)
            {
                return NotFound();
            }            
        }

        /// <summary>
        /// Returns single task by specified ID.
        /// Returns:
        /// - 404 if the task was not found.
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(IEnumerable<TaskDto>), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            try
            {
                var task = (await _tp.GetTaskById(id));
                var taskMapped = new TaskDto(task.Id, task.Name, task.Description, task.DueDate, task.IsCompleted, task.GoalId);
                return Ok(taskMapped);
            }
            catch (GoalNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Adds task 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        [ProducesResponseType(typeof(AddResponseTaskDto), 200)]
        public async Task<IActionResult> Add(AddRequestTaskDto model)
        {
            var task = (await _tp.AddTask(model.Name, model.Description, model.DueDate, model.IsCompleted, model.GoalId));
            var taskMapped = new AddResponseTaskDto(task.Id, task.Name, task.Description, task.DueDate, task.IsCompleted, task.GoalId);
            return Ok(taskMapped);
        }

        /// <summary>
        /// Edits task
        /// Returns:
        /// - 404 if the task was not found.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(typeof(EditResponseTaskDto), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        public async Task<IActionResult> Edit(EditRequestTaskDto model)
        {
            try
            {
                var task = (await _tp.EditTask(model.Id, model.Name, model.Description, model.DueDate, model.IsCompleted));
                var taskMapped = new EditResponseTaskDto(task.Id, task.Name, task.Description, task.DueDate, task.IsCompleted, task.GoalId);
                return Ok(taskMapped);
            }
            catch (GoalNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Remove task
        /// Returns:
        /// - 404 if the task was not found.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            try
            {
                await _tp.DeleteTask(id);
                return Ok();
            }
            catch (GoalNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Change state IsCompleted of task to specified value
        /// Returns:
        /// - 404 if the task was not found.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="complete"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("{id}/complete/{complete}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        public async Task<IActionResult> ChangeCompleteTask([FromRoute] Guid id, [FromRoute] bool complete)
        {
            try
            {
                await _tp.ChangeCompleteTask(id, complete);
                return Ok();
            }
            catch (GoalNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
