using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using LuceneDemo.Models;
using Microsoft.Extensions.Configuration;
using System.IO;
using Lucene.Net.Index;
using Lucene.Net.Store;
using Lucene.Net.Analysis;
using Lucene.Net.Documents;

namespace LuceneDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IConfiguration configuration;
        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            this.configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        public bool CreateIndex()
        {
            List<StudentModel> studentList = GetStudentList();
            string path = Path.Combine(Path.GetDirectoryName(typeof(Program).Assembly.Location), configuration["LuceneIndexPath"]);

            path = "F://LuceneTestDir";
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
            FSDirectory directory = FSDirectory.Open(path);

            using (IndexWriter writer = new IndexWriter(directory, new PanGuAnalyzer(), true, IndexWriter.MaxFieldLength.LIMITED))
            {
                foreach (var item in studentList)
                {
                    Document document = new Document();//一条数据

                    document.Add(new Field("Id", item.Id.ToString(), Field.Store.NO, Field.Index.NOT_ANALYZED));
                    document.Add(new Field("Name", item.Name.ToString(), Field.Store.YES, Field.Index.ANALYZED));
                    document.Add(new NumericField("Age", Field.Store.YES, true).SetIntValue(item.Age));
                    document.Add(new NumericField("CreateTime", Field.Store.YES, true).SetIntValue(int.Parse(item.CreateTime.ToString("yyyyMMdd"))));
                    writer.AddDocument(document);
                }

                writer.Optimize();//优化，合并
            }
            return true;
        }

        public List<StudentModel> GetStudentList()
        {
            List<StudentModel> studentModels = new List<StudentModel>();
            for (int i = 0; i < 1000; i++)
            {
                studentModels.Add(new StudentModel
                {
                    Id = i + 1,
                    Name = Guid.NewGuid().ToString(),
                    Age = i + 1,
                    CreateTime = DateTime.Now.AddDays(0 - i)
                });
            }
            return studentModels;
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
