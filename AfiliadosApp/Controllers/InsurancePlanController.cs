using AfiliadosApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AfiliadosApp.Controllers
{
    public class InsurancePlanController : Controller
    {
        private readonly AffiliatesContext _db;

        public InsurancePlanController(AffiliatesContext db) => _db = db;

        [HttpGet]
        public IActionResult Index()
        {
            var plans = _db.InsurancePlans.Include("Status").ToList();
            return View(plans);
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(InsurancePlan insurancePlan)
        {
            if (!ModelState.IsValid) return View(insurancePlan);

            _db.InsurancePlans.Add(insurancePlan);
            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var insurancePlan = _db.InsurancePlans.FirstOrDefault(i => i.Id == id);

            if (insurancePlan is not null) return View(insurancePlan);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(InsurancePlan updateInsurancePlan)
        {
            var insurancePlan = _db.InsurancePlans.FirstOrDefault(i => i.Id == updateInsurancePlan.Id);

            if (ModelState.IsValid && insurancePlan is not null) {
                insurancePlan.Description = updateInsurancePlan.Description;
                insurancePlan.CoverageLimit = updateInsurancePlan.CoverageLimit;
                _db.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            return View(insurancePlan);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var insurancePlan = _db.InsurancePlans.Find(id);

            if(insurancePlan is not null)
            {
                _db.InsurancePlans.Remove(insurancePlan);
                _db.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Activate(int id)
        {
            var insurancePlan = _db.InsurancePlans.Include("Affiliates").FirstOrDefault(i => i.Id == id);

            if(insurancePlan is not null)
            {
                insurancePlan.StatusId = 1;
                insurancePlan.Affiliates = insurancePlan.Affiliates.Select(a => {
                    a.StatusId = 1; return a; 
                }).ToList();
                _db.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Deactivate(int id)
        {
            var insurancePlan = _db.InsurancePlans.Include("Affiliates").FirstOrDefault(i => i.Id == id);

            if (insurancePlan is not null)
            {
                insurancePlan.StatusId = 2;
                insurancePlan.Affiliates = insurancePlan.Affiliates.Select(a => {
                    a.StatusId = 2; return a;
                }).ToList();
                _db.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
