using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using UIToolTrealet.Data;
using UIToolTrealet.Models;

namespace UIToolTrealet.Controllers
{
    public class ItemsController : Controller
    {
        private readonly ApplicationDbContext _context;
        public static IWebHostEnvironment _environment;

        public ItemsController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: Items
        public async Task<IActionResult> Index()
        {
            return View(await _context.Item.ToListAsync());
        }

        public async Task<ActionResult> Download(int? id)
        {

            var obj = await generateObject(id);

            if (obj == null)
            {
                return NotFound();
            }

            string JSONresult = JsonConvert.SerializeObject(obj, Formatting.Indented);

            // Nếu chưa tạo folder thì tạo folder
            if (!Directory.Exists(_environment.WebRootPath + "\\Json\\"))
            {
                Directory.CreateDirectory(_environment.WebRootPath + "\\Json\\");
            }

            string path = _environment.WebRootPath + "\\Json\\" + id + ".json";

            // Xóa file nếu file đã tồn tại
            if (System.IO.File.Exists(path))
            {
                try
                {
                    System.IO.File.Delete(path);
                }
                catch (Exception ex)
                {
                    //Do something
                }
            }

            // Write that JSON to txt file,  
            System.IO.File.WriteAllText(path, JSONresult);

            byte[] fileBytes = System.IO.File.ReadAllBytes(path);
            ViewBag.pathJson = path;
            string fileName = "myfile.json";
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        // GET: Items/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Item
                .FirstOrDefaultAsync(m => m.ItemId == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // GET: Items/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Items/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ItemId,Title,Desc,ImageCodeAvatar")] Item item)
        {
            if (ModelState.IsValid)
            {
                _context.Add(item);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(item);
        }

        // GET: Items/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Item.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ItemId,Title,Desc,ImageCodeAvatar")] Item item)
        {
            if (id != item.ItemId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(item);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemExists(item.ItemId))
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
            return View(item);
        }

        // GET: Items/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Item
                .FirstOrDefaultAsync(m => m.ItemId == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.Item.FindAsync(id);
            _context.Item.Remove(item);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ItemExists(int id)
        {
            return _context.Item.Any(e => e.ItemId == id);
        }

        public async Task<object> generateObject(int? id)
        {
            Item item = await _context.Item.FirstOrDefaultAsync(m => m.ItemId == id);
            List<Image> images = await _context.Image.Where(m => m.ItemId.Equals(id)).ToListAsync<Image>();
            List<Video> videos = await _context.Video.Where(m => m.ItemId.Equals(id)).ToListAsync<Video>();
            List<Interaction> interactions = await _context.Interaction.Where(m => m.ItemId.Equals(id)).ToListAsync<Interaction>();


            List<object> imagesMap = new List<object>();
            List<object> videosMap = new List<object>();
            List<object> interactionsMap = new List<object>();

            int length = Math.Max(Math.Max(images.Count(), videos.Count()), interactions.Count());
            // Map lại mảng images
            for (int i = 0; i < length; i++)
            {
                //images
                if (i < images.Count())
                {
                    imagesMap.Add(new
                    {
                        Id = images[i].ImageId,
                        Code = images[i].ImageCodeTrealet
                    });
                }

                //videos
                if (i < videos.Count())
                {
                    videosMap.Add(new
                    {
                        Id = videos[i].VideoId,
                        Url = videos[i].Url,
                        Title = videos[i].Title,
                        Type = videos[i].Type,
                    });
                }

                //interactions
                if (i < interactions.Count())
                {
                    interactionsMap.Add(new
                    {
                        Id = interactions[i].InteractionId,
                        interactions[i].Question,
                        AnswerA = interactions[i].AnswerA,
                        AnswerB = interactions[i].AnswerB,
                        AnswerC = interactions[i].AnswerC,
                        AnswerD = interactions[i].AnswerD,
                        TrueAnswer = interactions[i].TrueAnswer,
                    });
                }

            }

            object objJson = new
            {
                trealet = new
                {
                    Id = item.ItemId,
                    Title = item.Title,
                    Decs = item.Desc,
                    ImageAvatarCode = item.ImageCodeAvatar,
                    Images = new List<object>(imagesMap),
                    Videos = new List<object>(videosMap),
                    Interaction = new List<object>(interactionsMap)
                }


            };
            return objJson;
        }
    }
}
