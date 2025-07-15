using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SalesWebMvc.Models;
using SalesWebMvc.Models.ViewModels;
using SalesWebMvc.Services;

namespace SalesWebMvc.Controllers
{
    public class SalesRecordsController : Controller
    {
        private readonly SalesRecordService _salesRecordService;
        private readonly SellerService _sellerService;

        public SalesRecordsController(SalesRecordService salesRecordService, SellerService sellerService)
        {
            _salesRecordService = salesRecordService;
            _sellerService = sellerService;
        }

        public IActionResult IndexSimpleSearch()
        {
            return View(new List<SalesRecord>());
        }

        public IActionResult IndexGroupingSearch()
        {
            return View(new List<IGrouping<Department, SalesRecord>>());
        }

        public async Task<IActionResult> SimpleSearch(DateTime? minDate, DateTime? maxDate)
        {
            if (minDate.HasValue && maxDate.HasValue && maxDate < minDate)
            {
                ViewData["ErrorMessage"] = "A data final não pode ser anterior à data inicial.";
                ViewData["MinDate"] = "";
                ViewData["MaxDate"] = "";
                return View(new List<SalesRecord>());
            }

            var result = await _salesRecordService.FindByDateAsync(minDate, maxDate);

            if (result.Count == 0)
            {
                ViewData["ErrorMessage"] = "Nenhuma venda foi registrada nesse período.";
            }

            ViewData["MinDate"] = minDate?.ToString("yyyy-MM-dd");
            ViewData["MaxDate"] = maxDate?.ToString("yyyy-MM-dd");

            return View(result);
        }

        public async Task<IActionResult> GroupingSearch(DateTime? minDate, DateTime? maxDate)
        {
            if (!minDate.HasValue || !maxDate.HasValue)
            {
                TempData["ErrorMessage"] = "Preencha ambas as datas.";
                return RedirectToAction("IndexGroupingSearch");
            }

            if (minDate > maxDate)
            {
                TempData["ErrorMessage"] = "A data final não pode ser anterior à data inicial.";
                TempData["MinDate"] = minDate.Value.ToString("yyyy-MM-dd");
                TempData["MaxDate"] = maxDate.Value.ToString("yyyy-MM-dd");
                return RedirectToAction("IndexGroupingSearch");
            }

            var grouped = await _salesRecordService.FindByDateGroupingAsync(minDate, maxDate);

            if (grouped == null || !grouped.Any())
            {
                TempData["ErrorMessage"] = "Nenhuma venda foi registrada nesse período.";
                TempData["MinDate"] = minDate.Value.ToString("yyyy-MM-dd");
                TempData["MaxDate"] = maxDate.Value.ToString("yyyy-MM-dd");
                return RedirectToAction("IndexGroupingSearch");
            }

            ViewData["MinDate"] = minDate?.ToString("yyyy-MM-dd");
            ViewData["MaxDate"] = maxDate?.ToString("yyyy-MM-dd");

            return View(grouped);
        }

        [HttpGet]
        public async Task<JsonResult> CheckSimpleRecords(DateTime? minDate, DateTime? maxDate)
        {
            var result = await _salesRecordService.FindByDateAsync(minDate, maxDate);
            return Json(new { success = result != null && result.Any() });
        }

        [HttpGet]
        public async Task<JsonResult> CheckGroupedRecords(DateTime? minDate, DateTime? maxDate)
        {
            var grouped = await _salesRecordService.FindByDateGroupingAsync(minDate, maxDate);
            return Json(new { success = grouped != null && grouped.Any() });
        }

        public async Task<IActionResult> DepartmentalSales(DateTime? minDate, DateTime? maxDate)
        {
            if (!minDate.HasValue)
            {
                minDate = new DateTime(DateTime.Now.Year, 1, 1);
            }
            if (!maxDate.HasValue)
            {
                maxDate = DateTime.Now;
            }

            ViewData["minDate"] = minDate.Value.ToString("yyyy-MM-dd");
            ViewData["maxDate"] = maxDate.Value.ToString("yyyy-MM-dd");

            var result = await _salesRecordService.FindAggregatedSalesByDepartmentAsync(minDate, maxDate);
            return View(result);
        }

        public async Task<IActionResult> Create()
        {
            var sellers = await _sellerService.FindAllAsync();
            var viewModel = new SalesRecordFormViewModel
            {
                Sellers = sellers
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SalesRecord salesRecord)
        {
            if (!ModelState.IsValid)
            {
                var sellers = await _sellerService.FindAllAsync();
                var viewModel = new SalesRecordFormViewModel
                {
                    SalesRecord = salesRecord,
                    Sellers = sellers
                };
                return View(viewModel);
            }

            salesRecord.Id = 0;
            salesRecord.Date = DateTime.SpecifyKind(salesRecord.Date, DateTimeKind.Utc);

            await _salesRecordService.InsertAsync(salesRecord);

            TempData["SuccessMessage"] = "Venda registrada com sucesso!";

            // Recarrega os dados para o formulário
            var newViewModel = new SalesRecordFormViewModel
            {
                SalesRecord = new SalesRecord(), // zera o form
                Sellers = await _sellerService.FindAllAsync()
            };

            return View(newViewModel);
        }
    }
}
