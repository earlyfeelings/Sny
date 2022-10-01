using Sny.Core.AccountsAggregate;
using Sny.Core.Goals;
using Sny.Core.GoalsAggregate.Exceptions;
using Sny.Core.Interfaces.Infrastructure;
using Sny.Core.TasksAggregate.Exceptions;
using System;
using System.Xml.Linq;

namespace Sny.Infrastructure.Services.Repos
{
    public class TaskInmemoryRepo : ITaskProviderRepo, ITaskReadOnlyRepo
    {
        
        private List<Core.TasksAggregate.Task> _tasks = new List<Core.TasksAggregate.Task>()
        {
            new Core.TasksAggregate.Task(new Guid("8fcfc3fa-590d-462d-b063-dbac4e4b42b7"), "Úkol test1", "Tak ale toto je cool popisek.", null, false, new Guid("8fcfc3fa-590d-462d-b063-dbac4e4b42b1")),
            new Core.TasksAggregate.Task(new Guid("8fcfc3fa-590d-462d-b063-dbac4e4b42b8"), "Zacvičit si test2", "Tak ale toto je cool popisek!2", DateTime.Today, false, new Guid("8fcfc3fa-590d-462d-b063-dbac4e4b42b1")),
            new Core.TasksAggregate.Task(new Guid("8fcfc3fa-590d-462d-b063-dbac4e4b42b9"), "Zhubnout test3", "Tak ale toto je cool popisek!3",null, false, new Guid("8fcfc3fa-590d-462d-b063-dbac4e4b42b1")),
            new Core.TasksAggregate.Task(new Guid("8fcfc3fa-590d-462d-b063-dbac4e4b42ba"), "Jít na záchod test4", "Tak ale toto je cool popisek!4", null, true, new Guid("8fcfc3fa-590d-462d-b063-dbac4e4b42b1")),
            new Core.TasksAggregate.Task(new Guid("8fcfc3fa-590d-462d-b063-dbac4e4b42bb"), "Jít spát test5", "Tak ale toto je cool popisek!5", null, false, new Guid("8fcfc3fa-590d-462d-b063-dbac4e4b42b4")),
            new Core.TasksAggregate.Task(new Guid("8fcfc3fa-590d-462d-b063-dbac4e4b42bc"), "Meditovat test6", "Tak ale toto je cool popisek!6", null, false, new Guid("8fcfc3fa-590d-462d-b063-dbac4e4b42b4")),
        };
        
        public Task<Core.TasksAggregate.Task> AddTask(string name, string description, DateTime? dueDate, bool isCompleted, Guid goalId)
        {
            var task = new Core.TasksAggregate.Task(Guid.NewGuid(), name, description, dueDate, isCompleted, goalId);
            _tasks.Add(task);
            return Task.FromResult(task);
        }

        public Task<Core.TasksAggregate.Task> EditTask(Core.TasksAggregate.Task task)
        {
            DeleteTask(task.Id);
            _tasks.Add(task);
            return Task.FromResult(task);
        }
        
        public async Task<Core.TasksAggregate.Task> GetTaskById(Guid id)
        {
            var task = _tasks.SingleOrDefault(d => d.Id == id);
            return task ?? throw new TaskNotFoundException();
        }

        public async Task<IReadOnlyCollection<Core.TasksAggregate.Task>> GetTasksByGoalId(Guid id)
        {
            return _tasks.Where(d => d.GoalId == id).ToList();
        }        

        public void DeleteTask(Guid id)
        {
            var task = GetTaskById(id).Result;
            _tasks.Remove(task);
        }

        public void ChangeCompleteTask(Guid id, bool complete)
        {
            var task = GetTaskById(id).Result;
            DeleteTask(id);
            task.IsCompleted = complete;
            _tasks.Add(task);
        }
    }
}
