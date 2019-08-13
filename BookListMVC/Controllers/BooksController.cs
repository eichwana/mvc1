using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookListMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookListMVC.Controllers
{
    public class BooksController : Controller
    {

        private readonly ApplicationDbContext _db;

        public BooksController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View(_db.Books.ToList());
        }

        //get action method > book/create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Book book)
        {
            if (ModelState.IsValid)
            {
                _db.Add(book);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");

            }
            return View(book);
        }


        //get edit book
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _db.Books.SingleOrDefaultAsync(m => m.Id == id);

            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        //Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Book book)
        {
            if (ModelState.IsValid)
            {
                //_db.Update(book); this will update everything with what's entered. Even if no changes have occured

                // below method will only update changes
                var BookFromDb = await _db.Books.FirstOrDefaultAsync(b => b.Id == book.Id);
                BookFromDb.Name = book.Name;
                BookFromDb.Author = book.Author;
                BookFromDb.Price = book.Price;

                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        //Delete book
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _db.Books.SingleOrDefaultAsync(m => m.Id == id);

            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        //Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteEntry(int? id)
        {
            var book = await _db.Books.SingleOrDefaultAsync(m => m.Id == id);
            _db.Books.Remove(book);

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));


        }


        // Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _db.Books.SingleOrDefaultAsync(m => m.Id == id);

            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        //Details to edit
        [HttpPost, ActionName("Details")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Detailsz(int? id)
        {
            var book = await _db.Books.SingleOrDefaultAsync(m => m.Id == id);
           // _db.Books.Remove(book);

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));


        }
    }
}