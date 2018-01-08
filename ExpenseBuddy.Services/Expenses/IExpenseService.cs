using ExpenseBuddy.Data.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseBuddy.Services.Expenses
{
    public interface IExpenseService
    {
        Task<IEnumerable<Expense>> All();
        IEnumerable<SelectListItem> GetOwnersList(string selectedUserId);
        Task<IEnumerable<SelectListItem>> GetPayersList(int expenseId, string userId);
        Task<IEnumerable<SelectListItem>> GetElementsList();
        Task CreateAsync(Expense expense, IEnumerable<SelectListItem> selectedPayers);
        Task<Expense> FindById(int id);
        Task Update(int expId, Expense updatedExpense, ICollection<SelectListItem> selectedItems);
        Task<IEnumerable<Expense>> AllPaginated<TDestination>(string search, int? page, int pageSize = 5);
    }
}
