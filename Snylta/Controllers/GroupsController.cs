using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Microsoft.EntityFrameworkCore;
using Snylta.Data;
using Snylta.Models;

namespace Snylta
{
    public class GroupsController : Controller
    {
        private readonly IHostingEnvironment _host;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly ApplicationDbContext _context;

        public GroupsController(ApplicationDbContext context, RoleManager<Role> roleManager, IHostingEnvironment host, UserManager<User> userManager)
        {
            _host = host;
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        // GET: Groups
        public async Task<IActionResult> Index()
        {
            return View(await _context.Group.ToListAsync());
        }

        // GET: Groups/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @group = await _context.Group
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@group == null)
            {
                return NotFound();
            }

            return View(@group);
        }

        // GET: Groups/Create
        public IActionResult Create()
        {
            return View();
        }

        //// POST: Groups/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Name,Description,")] Group @group)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        if (!_roleManager.RoleExistsAsync(Constants.ConstRoles.MotherSnylt).Result)
        //        {
        //            await _roleManager.CreateAsync(new Role(Constants.ConstRoles.MotherSnylt));
        //        }

        //        _context.Add(@group);

        //        await _context.AddAsync(
        //            new GroupUsers()
        //            {
        //                GroupId = group.Id,
        //                UserId = _userManager.GetUserId(User),
        //                RoleId = _roleManager.FindByNameAsync(Constants.ConstRoles.MotherSnylt).Result.Id
        //            }
        //        );

        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(@group);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description")] Group @group, IFormFile file, string __RequestVerificationToken)
        {
            //var file = files.First();

            if (ModelState.IsValid)
            {
                if (!_roleManager.RoleExistsAsync(Constants.ConstRoles.MotherSnylt).Result)
                {
                    await _roleManager.CreateAsync(new Role(Constants.ConstRoles.MotherSnylt));
                }

                DirectoryInfo d = new DirectoryInfo(_host.WebRootPath + "\\CameraPhotos\\");//Assuming Test is your Folder
                FileInfo[] webcamImgs = d.GetFiles(__RequestVerificationToken + "*");

                if (webcamImgs.Count() == 0 && file != null)
                {
                    var groupGuid = Guid.NewGuid().ToString();
                    var fileName = groupGuid + file.FileName.ToString();
                    var filePath = _host.WebRootPath + "\\groupimages\\" + fileName;
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);

                    }

                    group.Pic = fileName;
                    _context.Add(group);

                }
                else
                {

                    webcamImgs[0].MoveTo(Path.Combine(_host.WebRootPath + "\\groupimages\\", webcamImgs[0].Name));
                    var filePath = _host.WebRootPath + "\\groupimages\\" + webcamImgs[0].Name;

                    group.Pic = webcamImgs[0].Name;
                    _context.Add(group);
                }

                await _context.AddAsync(
                    new GroupUsers()
                    {
                        GroupId = group.Id,
                        UserId = _userManager.GetUserId(User),
                        RoleId = _roleManager.FindByNameAsync(Constants.ConstRoles.MotherSnylt).Result.Id
                    }
                );

                //ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", thing.UserId);

                //---Lägga till bild
             
                //Getting Text files
                //1 hitta eventuella webcambilder som användaren tagit
                //2 flytta dem till mappen där vi lägger tingimages. (foreach?)
                //3 fyll filePaths-listan med alla filepaths till de flyttade filerna
                //Skapa ThinPic-objekt för varje bild och spara ner i databasen

                // full path to file in temp location
                //Lägger till bilder som användaren lägger upp





                
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(group);
        }





        public async Task<IActionResult> Join(string id)
        {
            var group = _context.Group.Find(id);

            if (group == null)
                return NotFound();

            var userId = _userManager.GetUserId(User);

            if (group.GroupUsers.Any(user => user.UserId == userId))
                return BadRequest("Du är redan medlem JÖÖ!");

            await _context.AddAsync(
                new GroupUsers()
                {
                    GroupId = group.Id,
                    UserId = userId
                }
            );

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Groups/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Group @group = await _context.Group.FindAsync(id);
            if (@group == null)
            {
                return NotFound();
            }
            return View(@group);
        }

        // POST: Groups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name")] Group @group)
        {
            if (id != @group.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@group);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GroupExists(@group.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(@group);
        }



        // GET: Groups/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @group = await _context.Group
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@group == null)
            {
                return NotFound();
            }

            return View(@group);
        }

        // POST: Groups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var @group = await _context.Group.FindAsync(id);
            _context.Group.Remove(@group);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Groups/Delete/5
        public async Task<IActionResult> RemoveMember(string groupId, string userId)
        {
            var activeUserId = _userManager.GetUserId(User);
            var group = await _context.Group.FindAsync(groupId);

            if (groupId == null || userId == null || !group.GroupUsers.Where(x => x.UserId == activeUserId).Any(y => y.Role.Name == Constants.ConstRoles.MotherSnylt))
            {
                return NotFound();
            }


            var groupUser = group.GroupUsers.First(x => x.UserId == userId);
            group.GroupUsers.Remove(groupUser);
            await _context.SaveChangesAsync();
            return View(nameof(Edit), group);


        }

        [HttpPost, ActionName("RemoveMember")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveMemberConfirmed(string groupId, string userId)
        {
            var activeUserId = _userManager.GetUserId(User);


            var group = await _context.Group.FindAsync(groupId);
            if (group.GroupUsers.Where(x => x.UserId == activeUserId).Any(y => y.Role.Name == Constants.ConstRoles.MotherSnylt))
            { }
            var groupUser = group.GroupUsers.First(x => x.UserId == userId);
            group.GroupUsers.Remove(groupUser);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GroupExists(string id)
        {
            return _context.Group.Any(e => e.Id == id);
        }
    }
}
