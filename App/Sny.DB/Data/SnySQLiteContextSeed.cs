using Sny.DB.Entities;

namespace Sny.DB.Data
{
    public class SnySQLiteContextSeed
    {
        public static async System.Threading.Tasks.Task SeedAsync(SnySQLiteContext snyContext, int? retry = 0)
        {
            int retryForAvailability = retry.Value;

            try
            {
                if (!snyContext.Accounts.Any())
                {
                    snyContext.Accounts.AddRange(GetPreconfiguredAccounts());
                    await snyContext.SaveChangesAsync();
                }

                if (!snyContext.Goals.Any())
                {
                    snyContext.Goals.AddRange(GetPreconfiguredGoals());
                    await snyContext.SaveChangesAsync();
                }

                if (!snyContext.Tasks.Any())
                {
                    snyContext.Tasks.AddRange(GetPreconfiguredTasks());
                    await snyContext.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                if (retryForAvailability < 10)
                {
                    retryForAvailability++;
                    await SeedAsync(snyContext, retryForAvailability);
                }
                throw;
            }
        }

        private static IEnumerable<Account> GetPreconfiguredAccounts()
        {
            return new List<Account>()
            {
                new Account() { Id = new Guid("8fcfc3fa-590d-462d-b063-dbac4e4b42c1"), Email = "dev@dev.cz", Password = "rq6QAAvjqmE2Xqfxl8eNwQ==", Name = "dev" },
                new Account() { Id = new Guid("8fcfc3fa-590d-462d-b063-dbac4e4b42c4"), Email = "test@dev.cz", Password = "rq6QAAvjqmE2Xqfxl8eNwQ==", Name = "test" },
            };
        }

        private static IEnumerable<Goal> GetPreconfiguredGoals()
        {
            return new List<Goal>()
            {
                new Goal() { Id = new Guid("8fcfc3fa-590d-462d-b063-dbac4e4b42b1"), Name = "Cíl test1", Active = false, IsCompleted = false, Description = "Tak ale toto je cool popisek.1", AccountId = new Guid("8fcfc3fa-590d-462d-b063-dbac4e4b42c1") },
                new Goal() { Id = new Guid("8fcfc3fa-590d-462d-b063-dbac4e4b42b2"), Name = "Cíl test2", Active = true, IsCompleted = false, Description = "Tak ale toto je cool popisek.2", AccountId = new Guid("8fcfc3fa-590d-462d-b063-dbac4e4b42c1") },
                new Goal() { Id = new Guid("8fcfc3fa-590d-462d-b063-dbac4e4b42b3"), Name = "Cíl test3", Active = false, IsCompleted = false, Description = "Tak ale toto je cool popisek.3", AccountId = new Guid("8fcfc3fa-590d-462d-b063-dbac4e4b42c1") },
                new Goal() { Id = new Guid("8fcfc3fa-590d-462d-b063-dbac4e4b42b4"), Name = "Cíl test4", Active = false, IsCompleted = false, Description = "Tak ale toto je cool popisek.4", AccountId = new Guid("8fcfc3fa-590d-462d-b063-dbac4e4b42c1") },
                new Goal() { Id = new Guid("8fcfc3fa-590d-462d-b063-dbac4e4b42b5"), Name = "Cíl test5", Active = true, IsCompleted = false, Description = "Tak ale toto je cool popisek.5", AccountId = new Guid("8fcfc3fa-590d-462d-b063-dbac4e4b42c1") },
                new Goal() { Id = new Guid("8fcfc3fa-590d-462d-b063-dbac4e4b42b6"), Name = "Cíl test6", Active = true, IsCompleted = false, Description = "Tak ale toto je cool popisek.6", AccountId = new Guid("8fcfc3fa-590d-462d-b063-dbac4e4b42c4") },
                new Goal() { Id = new Guid("8fcfc3fa-590d-462d-b063-dbac4e4b42b7"), Name = "Cíl test7", Active = false, IsCompleted = false, Description = "Tak ale toto je cool popisek.7", AccountId = new Guid("8fcfc3fa-590d-462d-b063-dbac4e4b42c4") },
            };
        }

        private static IEnumerable<Entities.Task> GetPreconfiguredTasks()
        {
            return new List<Entities.Task>()
            {
                new Entities.Task() { Id = new Guid("8fcfc3fa-590d-462d-b063-dbac4e4b42b7"), Name = "Úkol test1", IsCompleted = false, Description = "Tak ale toto je cool popisek.1", GoalId = new Guid("8fcfc3fa-590d-462d-b063-dbac4e4b42b1") },
                new Entities.Task() { Id = new Guid("8fcfc3fa-590d-462d-b063-dbac4e4b42b8"), Name = "Úkol test2", IsCompleted = false, Description = "Tak ale toto je cool popisek.2", GoalId = new Guid("8fcfc3fa-590d-462d-b063-dbac4e4b42b1") },
                new Entities.Task() { Id = new Guid("8fcfc3fa-590d-462d-b063-dbac4e4b42b9"), Name = "Úkol test3", IsCompleted = false, Description = "Tak ale toto je cool popisek.3", GoalId = new Guid("8fcfc3fa-590d-462d-b063-dbac4e4b42b1") },
                new Entities.Task() { Id = new Guid("8fcfc3fa-590d-462d-b063-dbac4e4b42ba"), Name = "Úkol test4", IsCompleted = false, Description = "Tak ale toto je cool popisek.4", GoalId = new Guid("8fcfc3fa-590d-462d-b063-dbac4e4b42b1") },
                new Entities.Task() { Id = new Guid("8fcfc3fa-590d-462d-b063-dbac4e4b42bb"), Name = "Úkol test5", IsCompleted = false, Description = "Tak ale toto je cool popisek.5", GoalId = new Guid("8fcfc3fa-590d-462d-b063-dbac4e4b42b2") },
                new Entities.Task() { Id = new Guid("8fcfc3fa-590d-462d-b063-dbac4e4b42bc"), Name = "Úkol test6", IsCompleted = false, Description = "Tak ale toto je cool popisek.6", GoalId = new Guid("8fcfc3fa-590d-462d-b063-dbac4e4b42b2") },
                new Entities.Task() { Id = new Guid("8fcfc3fa-590d-462d-b063-dbac4e4b42bd"), Name = "Úkol test7",  IsCompleted = false, Description = "Tak ale toto je cool popisek.7", GoalId = new Guid("8fcfc3fa-590d-462d-b063-dbac4e4b42b2") },
            };
        }
    }
}
