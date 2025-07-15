using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SalesWebMvc.Models;
using SalesWebMvc.Services;
using SalesWebMvc.Services.Exceptions;
using Microsoft.EntityFrameworkCore; // Necessário para DbConcurrencyException

namespace SalesWebMvc.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly SalesWebMvcContext _context;
        private readonly DepartmentService _departmentService;

        public DepartmentsController(DepartmentService departmentService, SalesWebMvcContext context)
        {
            _departmentService = departmentService;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var list = await _departmentService.FindAllAsync();
            return View(list);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Department department)
        {
            if (!ModelState.IsValid)
            {
                return View(department);
            }

            await _departmentService.InsertAsync(department);
            return RedirectToAction(nameof(Index));
        }

        // Action GET para exibir a página de confirmação de exclusão
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                // Redireciona para a Action Error no HomeController com mensagem
                return RedirectToAction("Error", "Home", new { message = "Id not provided", entityType = "Department" });
            }

            var department = await _departmentService.FindByIdAsync(id.Value);
            if (department == null)
            {
                // Redireciona para a Action Error no HomeController com mensagem
                return RedirectToAction("Error", "Home", new { message = "Id not found", entityType = "Department" });
            }

            return View(department);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")] // Importante! Faz com que o método DeleteConfirmed responda ao POST do Delete
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _departmentService.RemoveAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (IntegrityException e)
            {
                return RedirectToAction("Error", "Home", new { message = e.Message, entityType = "Department" });
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home", new { message = "Ocorreu um erro inesperado ao tentar deletar o departamento.", entityType = "Department" });
            }
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Department
                .Include(d => d.Sellers) // <-- isso aqui é o que carrega os vendedores
                .FirstOrDefaultAsync(m => m.Id == id);

            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Error", "Home", new { message = "Id not provided", entityType = "Department" });
            }

            var department = await _departmentService.FindByIdAsync(id.Value);
            if (department == null)
            {
                return RedirectToAction("Error", "Home", new { message = "Id not found", entityType = "Department" });
            }

            return View(department);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Department department)
        {
            if (!ModelState.IsValid)
            {
                return View(department);
            }

            if (id != department.Id)
            {
                return RedirectToAction("Error", "Home", new { message = "Id mismatch", entityType = "Department" });
            }

            try
            {
                await _departmentService.UpdateAsync(department);
                return RedirectToAction(nameof(Index));
            }
            catch (NotFoundException e)
            {
                return RedirectToAction("Error", "Home", new { message = e.Message, entityType = "Department" });
            }
            catch (DbConcurrencyException e)
            {
                return RedirectToAction("Error", "Home", new { message = e.Message, entityType = "Department" });
            }
            // CORRIGIDO: Adicionado catch para Exception genérica para tratamento completo
            catch (Exception e)
            {
                return RedirectToAction("Error", "Home", new { message = "Ocorreu um erro inesperado ao tentar atualizar o departamento.", entityType = "Department" });
            }
        }
    }
}