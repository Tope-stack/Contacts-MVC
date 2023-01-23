using ContactsMVC.Data;
using ContactsMVC.Models;
using ContactsMVC.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Net;
using System.Numerics;

namespace ContactsMVC.Controllers
{
    public class ContactsController : Controller
    {
        private readonly ContactsDemoDbContext contactsDemoDbContext;

        public ContactsController(ContactsDemoDbContext contactsDemoDbContext)
            {
            this.contactsDemoDbContext = contactsDemoDbContext;
            }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var contacts = await contactsDemoDbContext.Contacts.ToListAsync();
            return View(contacts);
        }

        [HttpGet]

        public IActionResult Add()
        {
            return View();  
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddContactViewModel addContactRequest)
        {
            var contact = new Contact()
            {
                Id = Guid.NewGuid(),
                FullName = addContactRequest.FullName,
                Email = addContactRequest.Email,
                Phone = addContactRequest.Phone,
                Address = addContactRequest.Address
            };
            await contactsDemoDbContext.Contacts.AddAsync(contact);
            await contactsDemoDbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]

        public async Task<IActionResult> View(Guid id)
        {
            var contact = await contactsDemoDbContext.Contacts.FirstOrDefaultAsync(x => x.Id == id);    

            if (contact != null)
            {
                var viewModel = new UpdateContact()
                {
                    Id = Guid.NewGuid(),
                    FullName = contact.FullName,
                    Email = contact.Email,
                    Phone = contact.Phone,
                    Address = contact.Address
                };

                return await Task.Run(() => View("view", viewModel));
            }

            return RedirectToAction("Index");   
        }

        [HttpPost]
        
        public async Task<IActionResult> View(UpdateContact model)
        {
            var contact = await contactsDemoDbContext.Contacts.FindAsync(model.Id);

            if (contact != null)
            {
                contact.FullName = model.FullName;
                contact.Email = model.Email;
                contact.Phone = model.Phone;
                contact.Address = model.Address;

                await contactsDemoDbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(UpdateContact model)
        {
            var contact = await contactsDemoDbContext.Contacts.FindAsync(model.Id);

            if (contact != null)
            {
                contactsDemoDbContext.Remove(contact);
                await contactsDemoDbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }
    }
}
