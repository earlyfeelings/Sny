﻿using Sny.Core.Goals;
using Sny.Core.Interfaces.Core;
using Sny.Core.Interfaces.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sny.Core.GoalsAggregate.Services
{
    public class GoalsProvider : IGoalsProvider
    {
        private readonly IGoalsReadOnlyRepo _gror;

        public GoalsProvider(IGoalsReadOnlyRepo gror)
        {
            this._gror = gror;
        }

        public Task<Goal> GetGoalById(Guid id)
        {
            return _gror.GetGoalById(id);
        }

        public Task<IReadOnlyCollection<Goal>> GetGoals()
        {
            return _gror.GetGoals();
        }
    }
}
