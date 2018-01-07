using ExpenseBuddy.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseBuddy.Services.Duties
{
    public interface IDutiesService
    {
        Task<IEnumerable<ExpensePayer>> AllForUser(string userId);
        Task<IEnumerable<ExpensePayer>> All();
        Task<ExpensePayer> FindById(string userId, int expenseId, string payerId);
        Task Pay(string userId, int expenseId, string payerId);
    }
}
