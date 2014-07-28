using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TronCell.Queue.Web.Models;

namespace TronCell.Queue.Web
{
    //public class PeopleController : Controller
    //{
    //    private TronCellRetailWebContext db = new TronCellRetailWebContext();

    //    // GET: People
    //    public async Task<ActionResult> Index()
    //    {
    //        return View(await db.People.ToListAsync());
    //    }

    //    // GET: People/Details/5
    //    public async Task<ActionResult> Details(int? id)
    //    {
    //        if (id == null)
    //        {
    //            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
    //        }
    //        People people = await db.People.FindAsync(id);
    //        if (people == null)
    //        {
    //            return HttpNotFound();
    //        }
    //        return View(people);
    //    }

    //    // GET: People/Create
    //    public ActionResult Create()
    //    {
    //        return View();
    //    }

    //    // POST: People/Create
    //    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    //    // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public async Task<ActionResult> Create([Bind(Include = "PeopleId,UserName")] People people)
    //    {
    //        if (ModelState.IsValid)
    //        {
    //            db.People.Add(people);
    //            await db.SaveChangesAsync();
    //            return RedirectToAction("Index");
    //        }

    //        return View(people);
    //    }

    //    // GET: People/Edit/5
    //    public async Task<ActionResult> Edit(int? id)
    //    {
    //        if (id == null)
    //        {
    //            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
    //        }
    //        People people = await db.People.FindAsync(id);
    //        if (people == null)
    //        {
    //            return HttpNotFound();
    //        }
    //        return View(people);
    //    }

    //    // POST: People/Edit/5
    //    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    //    // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public async Task<ActionResult> Edit([Bind(Include = "PeopleId,UserName")] People people)
    //    {
    //        if (ModelState.IsValid)
    //        {
    //            db.Entry(people).State = EntityState.Modified;
    //            await db.SaveChangesAsync();
    //            return RedirectToAction("Index");
    //        }
    //        return View(people);
    //    }

    //    // GET: People/Delete/5
    //    public async Task<ActionResult> Delete(int? id)
    //    {
    //        if (id == null)
    //        {
    //            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
    //        }
    //        People people = await db.People.FindAsync(id);
    //        if (people == null)
    //        {
    //            return HttpNotFound();
    //        }
    //        return View(people);
    //    }

    //    // POST: People/Delete/5
    //    [HttpPost, ActionName("Delete")]
    //    [ValidateAntiForgeryToken]
    //    public async Task<ActionResult> DeleteConfirmed(int id)
    //    {
    //        People people = await db.People.FindAsync(id);
    //        db.People.Remove(people);
    //        await db.SaveChangesAsync();
    //        return RedirectToAction("Index");
    //    }

    //    protected override void Dispose(bool disposing)
    //    {
    //        if (disposing)
    //        {
    //            db.Dispose();
    //        }
    //        base.Dispose(disposing);
    //    }
    //}
}
