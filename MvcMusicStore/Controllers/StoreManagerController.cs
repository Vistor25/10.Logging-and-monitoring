using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using MvcMusicStore.Models;
using NLog;

namespace MvcMusicStore.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class StoreManagerController : Controller
    {
        private readonly MusicStoreEntities _storeContext = new MusicStoreEntities();

        private Logger logger = LogManager.GetCurrentClassLogger();

        // GET: /StoreManager/
        public async Task<ActionResult> Index()
        {
            logger.Info($"Request for a list list of albums sorted by price");
            return View(await _storeContext.Albums
                .Include(a => a.Genre)
                .Include(a => a.Artist)
                .OrderBy(a => a.Price).ToListAsync());
        }

        // GET: /StoreManager/Details/5
        public async Task<ActionResult> Details(int id = 0)
        {
            var album = await _storeContext.Albums.FindAsync(id);
            
            if (album == null)
            {
                logger.Error($"Request to get album details with not existing id: {id}");
                return HttpNotFound();
            }

            logger.Info($"Request to get album details with id: {id}");
            return View(album);
        }

        // GET: /StoreManager/Create
        public async Task<ActionResult> Create()
        {
            return await BuildView(null);
        }

        // POST: /StoreManager/Create
        [HttpPost]
        public async Task<ActionResult> Create(Album album)
        {
            if (ModelState.IsValid)
            {
                _storeContext.Albums.Add(album);
                
                await _storeContext.SaveChangesAsync();

                logger.Info($"Created new album with title: {album.Title}");
                
                return RedirectToAction("Index");
            }

            return await BuildView(album);
        }

        // GET: /StoreManager/Edit/5
        public async Task<ActionResult> Edit(int id = 0)
        {
            var album = await _storeContext.Albums.FindAsync(id);
            if (album == null)
            {
                logger.Error($"Request to edit album with id: {id} not completed, because album with this id not found");
                return HttpNotFound();
            }

            logger.Info($"Request to edit album with id: {id}");
            return await BuildView(album);
        }

        // POST: /StoreManager/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(Album album)
        {
            if (ModelState.IsValid)
            {
                _storeContext.Entry(album).State = EntityState.Modified;

                await _storeContext.SaveChangesAsync();

                logger.Info($"Album with title - {album.Title} was edit");

                return RedirectToAction("Index");
            }

            return await BuildView(album);
        }

        // GET: /StoreManager/Delete/5
        public async Task<ActionResult> Delete(int id = 0)
        {
            var album = await _storeContext.Albums.FindAsync(id);
            if (album == null)
            {
                logger.Error($"Request to get album with not existing id - {id} for delete it");
                return HttpNotFound();
            }

            logger.Info($"Request to get album with id - {id} for delete it");
            return View(album);
        }

        // POST: /StoreManager/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            logger.Info($"Request to delete album with id - {id}");
            var album = await _storeContext.Albums.FindAsync(id);
            if (album == null)
            {
                logger.Error($"Album with id - {id} not found for delete");
                return HttpNotFound();
            }

            _storeContext.Albums.Remove(album);

            await _storeContext.SaveChangesAsync();

            logger.Info($"Album with id - {id} was deleted");

            return RedirectToAction("Index");
        }

        private async Task<ActionResult> BuildView(Album album)
        {
            ViewBag.GenreId = new SelectList(
                await _storeContext.Genres.ToListAsync(),
                "GenreId",
                "Name",
                album == null ? null : (object)album.GenreId);

            ViewBag.ArtistId = new SelectList(
                await _storeContext.Artists.ToListAsync(),
                "ArtistId",
                "Name",
                album == null ? null : (object)album.ArtistId);

            return View(album);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _storeContext.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}