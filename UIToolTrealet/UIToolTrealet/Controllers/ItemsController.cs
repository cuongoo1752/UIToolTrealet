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
        public async Task<IActionResult> Index(int? id)
        {
            int? pageId = id;
            if (pageId == null)
            {
                return NotFound();
            }

            var items = await _context.Item.Where(m => m.PageId.Equals(pageId)).ToListAsync<Item>();
            if (items == null)
            {
                return NotFound();
            }

            ViewData["PageId"] = pageId;

            return View(items);
        }



        public async Task<object> generateObjectPage(int? id)
        {
            Page page = await _context.Page.FirstOrDefaultAsync(m => m.PageId == id);


            Dictionary<string, string> objPage = new Dictionary<string, string>();

            objPage.Add("exec", "streamline");
            objPage.Add("pageid", page.PageId.ToString());
            objPage.Add("title", page.Title);
            objPage.Add("desc", page.Desc);
            objPage.Add("opening-hours", page.OpeningHours);
            objPage.Add("exhibition", page.Exhibition);
            objPage.Add("upcoming-events", page.UpcomingEvents);
            objPage.Add("registration-desc", page.RegistrationDesc);
            objPage.Add("buyonline-desc", page.BuyOnlineDesc);
            objPage.Add("banner-src", page.BannerURL);

            // Add từng element
            List<Item> items = await _context.Item.Where(x => x.PageId == page.PageId).ToListAsync();

            List<object> ojbItems = new List<object>();

            foreach (var item in items)
            {
                ojbItems.Add(await generateObject(item));
            }

            var obj = new
            {
                trealet = objPage,
                listItems = ojbItems,
            };

            return obj;

        }



        public async Task<ActionResult> Download(int? id, bool? isPage)
        {
            Item item = await _context.Item.FirstOrDefaultAsync(m => m.ItemId == id);


            int? PageId;
            string nameFile;
            var obj = new object();
            if (isPage == true)
            {
                obj = await generateObjectPage(id);
                nameFile = "streamline-page.trealet";
                PageId = id;

            }
            else
            {
                obj = await generateObject(item);
                nameFile = generateNameFile(item);
                PageId = item.PageId;
            }


            if (obj == null)
            {
                return NotFound();
            }

            string JSONresult = JsonConvert.SerializeObject(obj, Formatting.Indented,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                        }
);

            string namePage = PageId.ToString();
            if (PageId == 1)
            {
                namePage = "first";
            }
            else
            {
                namePage = "second";
            }

            string nameFolder = "\\Json\\" + namePage + "page\\";

            // Nếu chưa tạo folder thì tạo folder
            if (!Directory.Exists(_environment.WebRootPath + nameFolder))
            {
                Directory.CreateDirectory(_environment.WebRootPath + nameFolder);
            }



            string path = _environment.WebRootPath + nameFolder + nameFile;

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
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, nameFile);
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
        public IActionResult Create(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                ViewData["PageId"] = new SelectList(_context.Item, "PageId", "PageId");
                Item item = new Item()
                {
                    PageId = (int)id,
                };
                return View(item);
            }
        }

        // POST: Items/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ItemId,Title,Desc,TitleImage,PageId")] Item item)
        {
            if (ModelState.IsValid)
            {
                _context.Add(item);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", new { id = item.PageId });
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
            ViewData["PageId"] = new SelectList(_context.Item, "PageId", "PageId", item.PageId);
            return View(item);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ItemId,Title,Desc,TitleImage,PageId")] Item item)
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
                return RedirectToAction("Index", new { id = item.PageId });
            }
            ViewData["PageId"] = new SelectList(_context.Item, "PageId", "PageId", item.PageId);
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
                .Include(v => v.Page)
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
        public async Task<IActionResult> DeleteConfirmed(int id, int pageId)
        {
            var item = await _context.Item.FindAsync(id);
            _context.Item.Remove(item);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { id = pageId });
        }

        private bool ItemExists(int id)
        {
            return _context.Item.Any(e => e.ItemId == id);
        }

        public async Task<object> generateObject(Item item)
        {
            List<Info> infoes = await _context.Info.Where(m => m.ItemId.Equals(item.ItemId)).ToListAsync<Info>();
            List<Image> images = await _context.Image.Where(m => m.ItemId.Equals(item.ItemId)).ToListAsync<Image>();
            List<Video> videos = await _context.Video.Where(m => m.ItemId.Equals(item.ItemId)).ToListAsync<Video>();
            //List<Interaction> interactions = await _context.Interaction.Where(m => m.ItemId.Equals(id)).ToListAsync<Interaction>();


            List<string> imagesMap = new List<string>();
            List<string> videosMap = new List<string>();
            //List<object> interactionsMap = new List<object>();
            Dictionary<string, string> infoesMap = new Dictionary<string, string>();

            //int length = Math.Max(Math.Max(Math.Max(images.Count(), infoes.Count()), videos.Count()), interactions.Count());
            int length = Math.Max(Math.Max(images.Count(), infoes.Count()), videos.Count());

            // Map lại mảng images
            for (int i = 0; i < length; i++)
            {
                //images
                if (i < images.Count())
                {
                    imagesMap.Add(images[i].Url);
                }

                //videos
                if (i < videos.Count())
                {
                    videosMap.Add(videos[i].Url);
                }

                //interactions
                //if (i < interactions.Count())
                //{
                //    interactionsMap.Add(new
                //    {
                //        Id = interactions[i].InteractionId,
                //        interactions[i].Question,
                //        AnswerA = interactions[i].AnswerA,
                //        AnswerB = interactions[i].AnswerB,
                //        AnswerC = interactions[i].AnswerC,
                //        AnswerD = interactions[i].AnswerD,
                //        TrueAnswer = interactions[i].TrueAnswer,
                //    });
                //}

                if (i < infoes.Count())
                {
                    infoesMap.Add(infoes[i].Key, infoes[i].Value);
                }



            }

            object objJson = new
            {
                trealet = new
                {
                    itemID = item.ItemId,
                    title = item.Title,
                    titleImage = item.TitleImage,
                    info = new Dictionary<string, string>(infoesMap),
                    decs = item.Desc,
                    items = new List<string>(imagesMap),
                    videos = new List<string>(videosMap),
                    pageid = item.PageId
                }


            };
            return objJson;
        }

        public string generateNameFile(Item item)
        {
            string name = item.Title;

            // Bỏ dấu
            name = RemoveUnicode(name);

            name = name.ToLower();

            // Bỏ dấu cách 2 bên và chuyển dấu cách thành -
            string[] words = name.Split(' ');

            string nameFile = "streamline";
            for (int i = 0; i < words.Length; i++)
            {
                nameFile = nameFile + '_' + words[i];
            }

            nameFile = nameFile + ".trealet";

            return nameFile;
        }

        public static string RemoveUnicode(string text)
        {
            string[] arr1 = new string[] { "á", "à", "ả", "ã", "ạ", "â", "ấ", "ầ", "ẩ", "ẫ", "ậ", "ă", "ắ", "ằ", "ẳ", "ẵ", "ặ",
    "đ",
    "é","è","ẻ","ẽ","ẹ","ê","ế","ề","ể","ễ","ệ",
    "í","ì","ỉ","ĩ","ị",
    "ó","ò","ỏ","õ","ọ","ô","ố","ồ","ổ","ỗ","ộ","ơ","ớ","ờ","ở","ỡ","ợ",
    "ú","ù","ủ","ũ","ụ","ư","ứ","ừ","ử","ữ","ự",
    "ý","ỳ","ỷ","ỹ","ỵ",};
            string[] arr2 = new string[] { "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a",
    "d",
    "e","e","e","e","e","e","e","e","e","e","e",
    "i","i","i","i","i",
    "o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o",
    "u","u","u","u","u","u","u","u","u","u","u",
    "y","y","y","y","y",};
            for (int i = 0; i < arr1.Length; i++)
            {
                text = text.Replace(arr1[i], arr2[i]);
                text = text.Replace(arr1[i].ToUpper(), arr2[i].ToUpper());
            }
            return text;
        }
    }
}
