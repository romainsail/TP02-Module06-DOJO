using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BO;
using TP01_Module06.Data;
using TP01_Module06.Models;

namespace TP01_Module06.Controllers
{
    public class SamouraisController : Controller
    {
        private TP01_Module06Context db = new TP01_Module06Context();

        // GET: Samourais
        public ActionResult Index()
        {
            return View(db.Samourais.ToList());
        }

        // GET: Samourais/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Samourai samourai = db.Samourais.Find(id);
            if (samourai == null)
            {
                return HttpNotFound();
            }
            return View(samourai);
        }

        // GET: Samourais/Create
        public ActionResult Create()
        {
            SamouraiVM vm = new SamouraiVM();

            //On affiche que les armes disponibles, c a d non attachées à un Samourai
            List<Arme> armesDispo = new List<Arme>();
            foreach (var arme in db.Armes.ToList())
            {
                if (!db.Samourais.Any(s => s.Arme.Id == arme.Id))
                {
                    armesDispo.Add(arme);
                }
            }

            vm.Armes = armesDispo.Select(a => new SelectListItem { Text = a.Nom, Value = a.Id.ToString() }).ToList(); 
            vm.ArtMartials = db.ArtMartials.Select(a => new SelectListItem { Text = a.Nom, Value = a.Id.ToString() }).ToList(); 
            return View(vm);
        }

        // POST: Samourais/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SamouraiVM samouraiVM)
        {
            if (ModelState.IsValid)
            {
                samouraiVM.Samourai.Arme = db.Armes.FirstOrDefault(a => a.Id == samouraiVM.IdSelectedArme.Value);

                samouraiVM.Samourai.ArtMartials = db.ArtMartials.Where(
                        x => samouraiVM.IdsArtMartial.Contains(x.Id)).ToList();

                db.Samourais.Add(samouraiVM.Samourai);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(samouraiVM);
        }

        // GET: Samourais/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Samourai samourai = db.Samourais.Find(id);
            if (samourai == null)
            {
                return HttpNotFound();
            }
            SamouraiVM vm = new SamouraiVM();
            vm.ArtMartials = db.ArtMartials.Select(a => new SelectListItem { Text = a.Nom, Value = a.Id.ToString() }).ToList();
            vm.Samourai = samourai;

            //On affiche que les armes disponibles, c a d non attachées à un Samourai
            List<Arme> armesDispo = new List<Arme>();
            foreach (var arme in db.Armes.ToList())
            {
                if (!db.Samourais.Any(s => s.Arme.Id == arme.Id))
                {
                    armesDispo.Add(arme);
                }
            }

            //Preselection de l'arme choisie lors de la création
            if (samourai.Arme != null)
            {
                armesDispo.Add(db.Armes.FirstOrDefault(a => a.Id == samourai.Arme.Id));
                vm.IdSelectedArme = samourai.Arme.Id;
            }

            vm.Armes = armesDispo.Select(a => new SelectListItem { Text = a.Nom, Value = a.Id.ToString() }).ToList();

            if (vm.Samourai.ArtMartials.Any())
            {
                vm.IdsArtMartial = vm.Samourai.ArtMartials.Select(x => x.Id).ToList();
            }

            return View(vm);
        }

        // POST: Samourais/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SamouraiVM vm)
        {
            if (ModelState.IsValid)
            {
                Samourai samourai = db.Samourais.Find(vm.Samourai.Id);
                samourai.Force = vm.Samourai.Force;
                samourai.Nom = vm.Samourai.Nom;
                if (vm.IdSelectedArme != null)
                {
                    samourai.Arme = db.Armes.FirstOrDefault(a => a.Id == vm.IdSelectedArme.Value);
                }

                if (vm.IdsArtMartial != null)
                {
                    foreach (var artMartial in samourai.ArtMartials)
                    {
                        foreach (var idArtMartial in vm.IdsArtMartial)
                        {
                            if (!(artMartial.Id == idArtMartial))
                            {
                                samourai.ArtMartials = db.ArtMartials.Where(a => vm.IdsArtMartial.Contains(a.Id)).ToList();
                            }
                        }
                    }
                    
                }

                db.Entry(samourai).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(vm);
        }

        // GET: Samourais/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Samourai samourai = db.Samourais.Find(id);
            if (samourai == null)
            {
                return HttpNotFound();
            }
            return View(samourai);
        }

        // POST: Samourais/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Samourai samourai = db.Samourais.Find(id);
            db.Samourais.Remove(samourai);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
