using AfiliadosApp.Models;
using AfiliadosApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AfiliadosApp.Controllers
{
    public class AffiliateController : Controller
    {
        private readonly AffiliatesContext _db;

        public AffiliateController(AffiliatesContext db) => _db = db;

        [HttpGet]
        public IActionResult Index()
        {
            var affiliates = _db.Affiliates.Include("Status").Include("InsurancePlan").ToList();
            return View(affiliates);
        }

        [HttpGet]
        public IActionResult Create() {
            var affiliateViewModel = new AffiliateViewModel {
                Affiliate = new Affiliate(),
                InsurancePlans = new SelectList(_db.InsurancePlans.Where(i => i.StatusId != 2).ToList(), "Id", "Description")
            };

            return View(affiliateViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(AffiliateViewModel affiliateViewModel)
        {
            if (!ModelState.IsValid) return View(affiliateViewModel);

            _db.Affiliates.Add(affiliateViewModel.Affiliate);
            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var affiliateViewModel = new AffiliateViewModel
            {
                Affiliate = _db.Affiliates.FirstOrDefault(i => i.Id == id),
                InsurancePlans = new SelectList(_db.InsurancePlans.Where(i => i.StatusId != 2).ToList(), "Id", "Description")
            };

            if (affiliateViewModel.Affiliate is not null) return View(affiliateViewModel);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(AffiliateViewModel affiliateViewModel)
        {
            var affiliate = _db.Affiliates.FirstOrDefault(i => i.Id == affiliateViewModel.Affiliate.Id);

            if (ModelState.IsValid && affiliate is not null)
            {
                affiliate.Name = affiliateViewModel.Affiliate.Name;
                affiliate.LastName = affiliateViewModel.Affiliate.LastName;
                affiliate.Genre = affiliateViewModel.Affiliate.Genre;
                affiliate.IdCard = affiliateViewModel.Affiliate.IdCard;
                affiliate.SocialSecurity = affiliateViewModel.Affiliate.SocialSecurity;
                affiliate.InsurancePlanId = affiliateViewModel.Affiliate.InsurancePlanId;
                _db.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            return View(affiliateViewModel);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var affiliate = _db.Affiliates.Find(id);

            if (affiliate is not null)
            {
                _db.Affiliates.Remove(affiliate);
                _db.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public IActionResult Activate(int id)
        {
            var affiliate = _db.Affiliates.FirstOrDefault(i => i.Id == id);

            if (affiliate is not null)
            {
                affiliate.StatusId = 1;
                _db.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Deactivate(int id)
        {
            var affiliate = _db.Affiliates.FirstOrDefault(i => i.Id == id);

            if (affiliate is not null)
            {
                affiliate.StatusId = 2;
                _db.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult AddAmount(int id)
        {
            var affiliate = new Affiliate { Id = id };

            if (id != 0) return View(affiliate);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddAmount(Affiliate updateAffiliate)
        {
            var affiliate = _db.Affiliates.FirstOrDefault(i => i.Id == updateAffiliate.Id);

            if (affiliate is not null && affiliate.StatusId != 2)
            {
                affiliate.SpendedAmount += updateAffiliate.SpendedAmount;
                _db.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            return View(affiliate);
        }

        [HttpGet]
        public IActionResult DeductAmount(int id)
        {
            var affiliate = new Affiliate { Id = id };

            if (id != 0) return View(affiliate);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeductAmount(Affiliate updateAffiliate)
        {
            var affiliate = _db.Affiliates.FirstOrDefault(i => i.Id == updateAffiliate.Id);

            if (affiliate is not null && !(updateAffiliate.SpendedAmount > affiliate.SpendedAmount) && affiliate.StatusId != 2)
            {
                affiliate.SpendedAmount -= updateAffiliate.SpendedAmount;
                _db.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            return View(affiliate);
        }
    }
}
