using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Snylta.Data;
using Snylta.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Snylta.Services;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Snylta.Models.ImageTagGenerator;

namespace Snylta
{
    public class ThingsController : Controller
    {
        private readonly IHostingEnvironment _host;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly TranslationService _translationService;
        private readonly ImageTagGeneratorService _imageTagGeneratorService;

        public ThingsController(ApplicationDbContext context, UserManager<User> userManager, IHostingEnvironment host, TranslationService translationService, ImageTagGeneratorService imageTagGeneratorService)
        {
            _host = host;
            _context = context;
            _userManager = userManager;
            _translationService = translationService;
            _imageTagGeneratorService = imageTagGeneratorService;
        }

        // GET: Things
        public async Task<IActionResult> Index()
        {
            User user = await _userManager.GetUserAsync(User);
            var applicationDbContext = _context.Thing.Include(x => x.Snyltningar).Where(t => t.Owner.Id != user.Id);

            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> MyThings()
        {
            User user = await _userManager.GetUserAsync(User);

            return View(user.Things);
        }


        // GET: Things/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var thing = await _context.Thing
                .Include(t => t.Owner)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (thing == null)
            {
                return NotFound();
            }

            return View(thing);
        }



        // GET: Things/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Things/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        public IActionResult Test(IFormFile snapPic)
        {
            return Ok(snapPic);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description")] Thing thing, List<IFormFile> files, string __RequestVerificationToken)
        {
            //var file = files.First();

            if (ModelState.IsValid)
            {

                thing.UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

                _context.Add(thing);
                ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", thing.UserId);

                //---Lägga till bild

                DirectoryInfo d = new DirectoryInfo(@"C:\Project\AcceleratedLearning\Slutprojekt\Snylta\wwwroot\CameraPhotos\");
                FileInfo[] webcamImgs = d.GetFiles(__RequestVerificationToken + "*"); 
                //1 hitta eventuella webcambilder som användaren tagit
                //2 flytta dem till mappen där vi lägger tingimages. (foreach?)
                //3 fyll filePaths-listan med alla filepaths till de flyttade filerna
                //Skapa ThinPic-objekt för varje bild och spara ner i databasen
                
                var thingGuid = Guid.NewGuid().ToString();

                var picList = new List<ThingPic>();
                var filePaths = new List<string>();
                //Lägger till bilder som användaren har tagit med kamera
             
                
                foreach (FileInfo img in webcamImgs)
                {
                    var pic = new ThingPic();
                    //img.Replace

                    img.MoveTo(Path.Combine(@"C:\Project\AcceleratedLearning\Slutprojekt\Snylta\wwwroot\thingimages\", img.Name));
                    //var fileName = thingGuid + img.Name.ToString();
                    var filePath = _host.WebRootPath + "\\thingimages\\" + img.Name;

                    filePaths.Add(filePath);

                    pic.Pic = img.Name;
                    picList.Add(pic);
                    _context.ThingPic.Add(pic);
                }

                //Lägger till bilder som användaren lägger upp
                foreach (var file in files)
                {
                    var pic = new ThingPic();

                    var fileName = thingGuid + file.FileName.ToString();
                    var filePath = _host.WebRootPath + "\\thingimages\\" + fileName;

                    filePaths.Add(filePath);

                    if (file.Length > 0 && file.Length < 10000000)
                    {
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);

                        }

                        pic.Pic = fileName;
                        picList.Add(pic);
                        _context.ThingPic.Add(pic);
                    }
                }

                //var EnglishTagList = new List<string>();

                var analysises = await _imageTagGeneratorService.GetTagsForImages(filePaths);
                
                //Flatterns the list of analysises with lists of imagetags to list of just imagetags
                var imageTags = analysises.SelectMany(imagetags => imagetags.Tags).ToList();

                //Removes duplicates with the same tag name and in the process sums duplicates confidence
                imageTags = imageTags
                    .GroupBy(
                        imageTag => imageTag.Name,
                        imageTag => imageTag.Confidence,
                        (key, group) => new ImageTag(key, group.Sum())
                    ).ToList();

                //EnglishTagList = GetTagsFromAnalysises(analysises);
                //var listOfconfidence = GetConfidencesFromAnalysises(analysises);
                var SwedishTagList = await _translationService.TranslateText(imageTags.Select(tag => tag.Name).ToList());

                for (int i = 0; i < imageTags.Count(); i++)
                {
                    Tag tag;

                    if (!_context.Tag.Any(t => t.EnglishTag == imageTags[i].Name))
                    {
                        tag = new Tag()
                        {
                            EnglishTag = imageTags[i].Name,
                            SwedishTag = SwedishTagList[i]
                        };

                        _context.Add(tag);
                    }
                    else
                    {
                        tag = _context.Tag.First(t => t.EnglishTag == imageTags[i].Name);
                    }

                    var thingTag = new ThingTags()
                    {
                        TagId = tag.Id,
                        ThingId = thing.Id,
                        Confidence = imageTags[i].Confidence
                    };
                    _context.Add(thingTag);
                }
                thing.ThingPics = picList;
                //_context.Thing.Add(thing);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(thing);
        }

        private List<double> GetConfidencesFromAnalysises(List<ImageAnalysis> analysises)
        {
            var listOfConfidence = new List<double>();

            foreach (var analysis in analysises)
                listOfConfidence.AddRange(analysis.Tags.Select(t => t.Confidence));

            return listOfConfidence;
        }

        private List<string> GetTagsFromAnalysises(List<ImageAnalysis> analysises)
        {
            var tags = new List<String>();

            foreach (var analysis in analysises)
                tags.AddRange(analysis.Tags.Select(t => t.Name));

            return tags;
        }

        //private List<string> GoodEnoughTags(List<ImageAnalysis> analysises)
        //{
        //    var GoodEnoughTags = new List<string>();
        //    foreach (var item in analysises)
        //    {
        //        GoodEnoughTags.AddRange(item.Tags.Where(x => x.Confidence > 0.1).Select(x => x.Name));
        //    }

        //    return GoodEnoughTags;
        //}

        // GET: Things/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var thing = await _context.Thing.FindAsync(id);
            if (thing == null)
            {
                return NotFound();
            }

            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", thing.UserId);
            return View(thing);
        }

        // POST: Things/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,UserId,Description,ThingPic")] Thing thing, List<IFormFile> files)
        {
            if (id != thing.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var thingGuid = Guid.NewGuid().ToString();

                var picList = new List<ThingPic>();
                foreach (var file in files)
                {
                    var pic = new ThingPic();

                    var fileName = thingGuid + file.FileName.ToString();
                    var filePath = _host.WebRootPath + "\\thingimages\\" + fileName;

                    if (file.Length > 0)
                    {
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        pic.Pic = fileName;
                        picList.Add(pic);
                    }
                }

                try
                {
                    thing.ThingPics = picList;
                    _context.Update(thing);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ThingExists(thing.Id))
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
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", thing.UserId);
            return View(thing);
        }

        // GET: Things/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var thing = await _context.Thing
                .Include(t => t.Owner)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (thing == null)
            {
                return NotFound();
            }
            return View(thing);
        }

        // POST: Things/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var thing = await _context.Thing.FindAsync(id);
            thing.ThingPics.RemoveAll(t => t.ThingId == id);

            _context.Thing.Remove(thing);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Snyltningar(string id)
        {
            User user = await _userManager.GetUserAsync(User);
            return View(user.Snyltningar.Where(x => x.Active).Select(x => x.Thing).ToList());
        }

        public async Task<IActionResult> Snylta(string id)
        {
            User user = await _userManager.GetUserAsync(User);
            Thing thing = _context.Thing.FirstOrDefault(x => x.Id == id);

            //Kontrollerar att prylen är tillgänglig och att den får lånas av användaren
            if (thing == null)
                return BadRequest($"Hittade ingen pryl med id {id}");

            if (thing.Snyltningar.Any(x => x.Active))
                if (thing.Snyltningar.FirstOrDefault(x => x.Active).Snyltare == user)
                    return BadRequest($"Du snyltar redan prylen {thing.Name}");
                else
                    return BadRequest($"Prylen {thing.Name} är redan snyltad");

            if (thing.Owner == user)
                return BadRequest($"Du kan inte låna din egen pryl!");


            _context.Add(new Snyltning(user.Id, thing.Id));
            _context.SaveChanges();

            return Ok($"Du {user.UserName} snyltar nu {thing.Name}!");
        }

        public IActionResult AvSnylta(string id)
        {
            var snyltning = _context.Snyltning.FirstOrDefault(x => x.ThingId == id && x.Active);
            snyltning.Active = false;
            _context.SaveChanges();

            return RedirectToAction("MyThings");
        }

        private bool ThingExists(string id)
        {
            return _context.Thing.Any(e => e.Id == id);
        }


        [HttpPost]
        public IActionResult Capture(string name)
        {
            var files = HttpContext.Request.Form.Files;
            if (files != null)
            {
                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        // Getting Filename  
                        var fileName = file.FileName;
                        // Unique filename "Guid"  
                        var myUniqueFileName = Convert.ToString(Guid.NewGuid());
                        // Getting Extension  
                        //var fileExtension = Path.GetExtension(fileName);
                        var fileExtension = fileName;

                        // Concating filename + fileExtension (unique filename)  
                        var newFileName = string.Concat(myUniqueFileName, fileExtension);
                        //  Generating Path to store photo   
                        var filepath = Path.Combine(_host.WebRootPath, "CameraPhotos") + $@"\{fileName}";

                        if (!string.IsNullOrEmpty(filepath))
                        {
                            // Storing Image in Folder  
                            StoreInFolder(file, filepath);
                        }

                        //var imageBytes = System.IO.File.ReadAllBytes(filepath);
                        //if (imageBytes != null)
                        //{
                        //    // Storing Image in Folder  
                        //    StoreInDatabase(imageBytes);
                        //}

                    }
                }
                return Json(true);
            }
            else
            {
                return Json(false);
            }
        }

        private void StoreInFolder(IFormFile file, string fileName)
        {
            using (FileStream fs = System.IO.File.Create(fileName))
            {
                file.CopyTo(fs);
                fs.Flush();
            }
        }
    }
}
