using MemeApi.Controllers;
using MemeApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace UnitTestMemeBank
{
    [TestClass]
    public class PutUnitTests
    {
        public static readonly DbContextOptions<MemeApiContext> options
            = new DbContextOptionsBuilder<MemeApiContext>()
            .UseInMemoryDatabase(databaseName: "testDatabase")
            .Options;
        public static IConfiguration configuration = null;
        public static readonly IList<string> memeTitles = new List<string> { "dankMeme", "dankerMeme" };

        [TestInitialize]
        public void SetupDb()
        {
            using (var context = new MemeApiContext(options))
            {
                MemeItem memeItem1 = new MemeItem()
                {
                    Title = memeTitles[0]
                };

                MemeItem memeItem2 = new MemeItem()
                {
                    Title = memeTitles[1]
                };

                context.MemeItem.Add(memeItem1);
                context.MemeItem.Add(memeItem2);
                context.SaveChanges();
            }
        }

        [TestCleanup]
        public void ClearDb()
        {
            using (var context = new MemeApiContext(options))
            {
                context.MemeItem.RemoveRange(context.MemeItem);
                context.SaveChanges();
            };
        }

        [TestMethod]
        public async Task TestPutMemeItemNoContentStatusCode()
        {
            using (var context = new MemeApiContext(options))
            {
                // Given
                string title = "putMeme";
                MemeItem memeItem1 = context.MemeItem.Where(x => x.Title == memeTitles[0]).Single();
                memeItem1.Title = title;

                // When
                MemeController memeController = new MemeController(context, configuration);
                IActionResult result = await memeController.PutMemeItem(memeItem1.Id, memeItem1) as IActionResult;

                // Then
                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof(NoContentResult));
            }
        }

        [TestMethod]
        public async Task TestPutMemeItemUpdate()
        {
            using (var context = new MemeApiContext(options))
            {
                // Given
                string title = "putMeme";
                MemeItem memeItem1 = context.MemeItem.Where(x => x.Title == memeTitles[0]).Single();
                memeItem1.Title = title;

                // When
                MemeController memeController = new MemeController(context, configuration);
                IActionResult result = await memeController.PutMemeItem(memeItem1.Id, memeItem1) as IActionResult;

                // Then
                memeItem1 = context.MemeItem.Where(x => x.Title == title).Single();
            }
        }
    }
}