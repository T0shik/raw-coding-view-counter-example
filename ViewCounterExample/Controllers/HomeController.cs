using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ViewCounterExample.Data;

namespace ViewCounterExample.Controllers
{
    public class HomeController : BaseController
    {
        private readonly AppDbContext _ctx;
        private readonly IWebHostEnvironment _env;

        public HomeController(
            IWebHostEnvironment env,
            AppDbContext ctx)
        {
            _ctx = ctx;
            _env = env;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var posts = _ctx.Posts.ToList();
            return View(posts);
        }

        [HttpGet]
        public IActionResult Images()
        {
            var images = _ctx.Images.ToList();
            return View(images);
        }

        [HttpGet]
        public async Task<IActionResult> Post(int id)
        {
            var post = _ctx.Posts
                .Include(post => post.Views)
                .FirstOrDefault(x => x.Id == id);

            post.Views.Add(new Models.View { UserId = UserId });
            post.ViewsCount++;
            await _ctx.SaveChangesAsync();

            return View(post);
        }

        [HttpGet]
        public async Task<IActionResult> Image(string name)
        {
            var image = _ctx.Images.FirstOrDefault(x => x.Name == name);
            if (image == null)
            {
                return NoContent();
            }

            if (!_ctx.Views.Any(x =>
                    EF.Property<int>(x, "ImageId") == image.Id
                    && x.UserId == UserId))
            {
                image.Views.Add(new Models.View { UserId = UserId });
                image.ViewsCount++;
                await _ctx.SaveChangesAsync();
            }

            var filePath = Path.Combine(_env.WebRootPath, "img", name);
            return new FileStreamResult(new FileStream(filePath, FileMode.Open, FileAccess.Read), "image/jpg");
        }
    }
}
