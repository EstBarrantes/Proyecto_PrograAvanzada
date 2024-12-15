using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Pokemon_AP_Project_G3.Data;

namespace Pokemon_AP_Project_G3.Controllers
{
    public class POKEMONTESTController : Controller
    {
        private PokemonGameEntities db = new PokemonGameEntities();

        // GET: POKEMONTEST
        public async Task<ActionResult> Index()
        {
            var pokemon = db.Pokemon.Include(p => p.Pokemon2);
            return View(await pokemon.ToListAsync());
        }

        // GET: POKEMONTEST/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pokemon pokemon = await db.Pokemon.FindAsync(id);
            if (pokemon == null)
            {
                return HttpNotFound();
            }
            return View(pokemon);
        }

        // GET: POKEMONTEST/Create
        public ActionResult Create()
        {
            ViewBag.evolves_from = new SelectList(db.Pokemon, "pokemon_id", "name");
            return View();
        }

        // POST: POKEMONTEST/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "pokemon_id,name,type,weakness,weight,number,evolves_from,img_url_ally,img_url_enemy")] Pokemon pokemon)
        {
            if (ModelState.IsValid)
            {
                db.Pokemon.Add(pokemon);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.evolves_from = new SelectList(db.Pokemon, "pokemon_id", "name", pokemon.evolves_from);
            return View(pokemon);
        }

        // GET: POKEMONTEST/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pokemon pokemon = await db.Pokemon.FindAsync(id);
            if (pokemon == null)
            {
                return HttpNotFound();
            }
            ViewBag.evolves_from = new SelectList(db.Pokemon, "pokemon_id", "name", pokemon.evolves_from);
            return View(pokemon);
        }

        // POST: POKEMONTEST/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "pokemon_id,name,type,weakness,weight,number,evolves_from,img_url_ally,img_url_enemy")] Pokemon pokemon)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pokemon).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.evolves_from = new SelectList(db.Pokemon, "pokemon_id", "name", pokemon.evolves_from);
            return View(pokemon);
        }

        // GET: POKEMONTEST/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pokemon pokemon = await db.Pokemon.FindAsync(id);
            if (pokemon == null)
            {
                return HttpNotFound();
            }
            return View(pokemon);
        }

        // POST: POKEMONTEST/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Pokemon pokemon = await db.Pokemon.FindAsync(id);
            db.Pokemon.Remove(pokemon);
            await db.SaveChangesAsync();
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
