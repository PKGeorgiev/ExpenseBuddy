﻿using ExpenseBuddy.Data;
using ExpenseBuddy.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseBuddy.Services
{
    public interface IBankRepositoryService : IGenericRepository<ExpenseBuddyDbContext, Bank, int>
    {
    }
}
