using Lucene.Net.Analysis;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using LuceneNetDemo.Models;
using LuceneNetDemo.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LuceneNetDemo.Controllers
{
    public class HomeController : Controller
    {
        public string path = "F://LuceneIndexDir";
        public ActionResult Index()
        {
            return View();
        }
        /*
            Analysis：分词器，负责把字符串拆分成原子，包含了标准分词，直接空格拆分项目中用的是盘古中文分词
            Document:数据结构，定义存储数据的格式
            Index:索引的读写类
            QueryParser:查询解析器，负责解析查询语句
            Search：负责各种查询类，命令解析后得到就是查询类
            Store：索引存储类，负责文件夹等等；
            Util:常见工具类库；
         */

        /// <summary>
        /// 创建.Net Framwork Web项目，不支持Core Web
        /// 需引入以下dll：
        /// Lucene.Net
        /// PanGu.HighLight
        /// PanGu.Lucene.Analyzer
        /// </summary>
        /// <returns></returns>
        public int CreateIndex()
        {
            List<Bpo_JobEntity> jobList = JobRepository.GetJobList(1000);
            /*
             问题：
             运行时会报找不到\Dict\Dict.dct的问题，解决办法：因为此文件在上一级目录里面，把Dict文件夹粘贴到bin文件夹里面即可；
             */
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
            FSDirectory directory = FSDirectory.Open(path);

            //IndexReader:对索引进行读取的类。该语句的作用：判断索引库文件夹是否存在以及索引特征文件是否存在。
            bool isUpdate = IndexReader.IndexExists(directory);
            if (isUpdate)
            {
                //同时只能有一段代码对索引库进行写操作。当使用IndexWriter打开directory时会自动对索引库文件上锁。
                //如果索引目录被锁定（比如索引过程中程序异常退出），则首先解锁（提示一下：如果我现在正在写着已经加锁了，但是还没有写完，这时候又来一个请求，那么不就解锁了吗？这个问题后面会解决）
                if (IndexWriter.IsLocked(directory))
                {
                    IndexWriter.Unlock(directory);
                }
            }

            //分词规则：一元分词（一个字符分一组）、二元分词(两个字符一组)、盘古分词（一般用这个）
            using (IndexWriter writer = new IndexWriter(directory, new PanGuAnalyzer(), !isUpdate, IndexWriter.MaxFieldLength.LIMITED))
            {
                //控制写入一个新的segent前内存中保存的doc的数量，默认是10，在这里设置100，也就是每生成100个doc后才写一次，提高效率
                writer.SetMaxBufferedDocs(100);
                /*控制多个segment合并的评率，默认是10，在这里设置为100，指多个线程进行生产索引时，为了解决文件锁的问题，所以每个线程会生成一个文件夹，
                 文件夹里面索引生成多少个之后进行合并*/
                writer.MergeFactor = 100;
                writer.UseCompoundFile = true;//创建复合文件，减少索引文件数量
                foreach (var item in jobList)
                {
                    Document document = new Document();//一条数据

                    //一个字段，列名，值，是否保存值，是否分词
                    //Field.Store.NO 代表不存值，这样就不会生产索引，取值时为null 
                    document.Add(new NumericField("Id", Field.Store.YES, true).SetIntValue(item.Id));
                    document.Add(new Field("Title", item.Title ?? "".ToString(), Field.Store.YES, Field.Index.ANALYZED));
                    document.Add(new NumericField("UserId", Field.Store.YES, true).SetIntValue(item.UserId));
                    document.Add(new Field("UserName", item.UserName.ToString(), Field.Store.YES, Field.Index.ANALYZED));
                    document.Add(new NumericField("CompanyId", Field.Store.YES, true).SetIntValue(item.CompanyId));
                    document.Add(new Field("CompanyName", item.CompanyName.ToString(), Field.Store.YES, Field.Index.ANALYZED));
                    document.Add(new Field("FullAddress", item.FullAddress ?? "".ToString(), Field.Store.YES, Field.Index.ANALYZED));
                    // document.Add(new NumericField("CreateTime", Field.Store.YES, true).SetIntValue(int.Parse(item.CreateTime.ToString("yyyyMMdd"))));
                    document.Add(new Field("CreateDate", item.CreateDate.ToString("yyyy-MM-dd HH:mm:ss"), Field.Store.YES, Field.Index.NOT_ANALYZED));
                    writer.AddDocument(document);
                }

                writer.Optimize();//优化，合并
            }
            return 1;
        }

        //public int DeleteIndex()
        //{
        //    FSDirectory directory = FSDirectory.Open(path);

        //    using (IndexReader reader = IndexReader.Open(directory))
        //    {
        //        Term term = new Term("path",path);
        //        int deleted = reader.Delete(term);
        //    } 
             
        
        //}

        /// <summary>
        /// 根据关键字进行查询，就像sql的like一样
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public List<Bpo_JobEntity> SearchJobList(string title)
        {
            List<Bpo_JobEntity> jobList = new List<Bpo_JobEntity>();
            string path = "F://LuceneIndexDir";
            FSDirectory dir = FSDirectory.Open(path);

            IndexSearcher searcher = new IndexSearcher(dir);//查询器
            TermQuery query = new TermQuery(new Term("Title", title)); //根据名称查询

            //取搜索结果方法1：
            //TopDocs docs = searcher.Search(query, null,2);//找到的结果取2条数据
            //foreach (ScoreDoc sd in docs.ScoreDocs) //从docs.ScoreDocs取数据

            //取搜索结果方法2：
            //声明一个集合Collector，大小为1000条
            TopScoreDocCollector collector = TopScoreDocCollector.Create(1000, true);
            // 根据query查询条件进行查询，查询结果放入collector容器
            searcher.Search(query, null, collector);
            int totalCount = collector.TotalHits;//获取结果的总条数，
            //TopDocs，取值，可用此进行分页
            ScoreDoc[] docs = collector.TopDocs(0, totalCount).ScoreDocs;

            foreach (ScoreDoc sd in docs)
            {
                Document doc = searcher.Doc(sd.Doc);
                jobList.Add(new Bpo_JobEntity
                {
                    Id = Convert.ToInt32(doc.Get("Id") ?? "-100"),
                    Title = doc.Get("Title"),
                    UserId = Convert.ToInt32(doc.Get("UserId") ?? "-100"),
                    UserName = doc.Get("UserName"),
                    CompanyId = Convert.ToInt32(doc.Get("CompanyId") ?? "-100"),
                    CompanyName = doc.Get("CompanyName"),
                    FullAddress = doc.Get("FullAddress"),
                    CreateDate = Convert.ToDateTime(doc.Get("CreateDate"))
                });
            }
            return jobList;
        }

        /// <summary>
        ///根据关键字进行查询,可以对关键字进行分词
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public List<Bpo_JobEntity> SearchJobList2(string title)
        {
            List<Bpo_JobEntity> jobList = new List<Bpo_JobEntity>();
            string path = "F://LuceneIndexDir";
            FSDirectory dir = FSDirectory.Open(path);

            IndexSearcher searcher = new IndexSearcher(dir);//查询器

            /*
             Title参数根据判断进行分词，可以支持分词查询，多个词之间是or的关系，搜索的时候中间加个空格，
             如：wcs 单子 这样就会把包含wcs 或 包含单子关键字的工单搜索出来
             */
            QueryParser parser = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, "Title", new PanGuAnalyzer());

            Query query = parser.Parse(title); //根据名称查询

            //取搜索结果方法1：
            //TopDocs docs = searcher.Search(query, null,2);//找到的结果取2条数据
            //foreach (ScoreDoc sd in docs.ScoreDocs) //从docs.ScoreDocs取数据

            //取搜索结果方法2：
            //声明一个集合Collector，大小为1000条
            TopScoreDocCollector collector = TopScoreDocCollector.Create(1000, true);

            //SortField sortField = new SortField("Id", SortField.INT, false);//降序
            ////SortField sortField = new SortField("Id", SortField.INT, false);//降序
            //Sort sort = new Sort(sortField);
            // 根据query查询条件进行查询，查询结果放入collector容器
            searcher.Search(query, null, collector);
            int totalCount = collector.TotalHits;//获取结果的总条数，
            //TopDocs，取值，可用此进行分页
            ScoreDoc[] docs = collector.TopDocs(0, totalCount).ScoreDocs;

            foreach (ScoreDoc sd in docs)
            {
                Document doc = searcher.Doc(sd.Doc);
                jobList.Add(new Bpo_JobEntity
                {
                    Id = Convert.ToInt32(doc.Get("Id") ?? "-100"),
                    Title = doc.Get("Title"),
                    UserId = Convert.ToInt32(doc.Get("UserId") ?? "-100"),
                    UserName = doc.Get("UserName"),
                    CompanyId = Convert.ToInt32(doc.Get("CompanyId") ?? "-100"),
                    CompanyName = doc.Get("CompanyName"),
                    FullAddress = doc.Get("FullAddress"),
                    CreateDate = Convert.ToDateTime(doc.Get("CreateDate"))
                });
            }
            return jobList;
        }

        /// <summary>
        ///根据关键字进行查询,根据int的范围进行搜索
        /// </summary>
        /// <param name="Title"></param>
        /// <returns></returns>
        public List<Bpo_JobEntity> SearchJobList3(string Title, int MinId, int MaxId)
        {
            List<Bpo_JobEntity> jobList = new List<Bpo_JobEntity>();
            string path = "F://LuceneIndexDir";
            FSDirectory dir = FSDirectory.Open(path);

            IndexSearcher searcher = new IndexSearcher(dir);//查询器

            /*
             Title参数根据判断进行分词，可以支持分词查询，多个词之间是or的关系，搜索的时候中间加个空格，
             如：wcs 单子 这样就会把包含wcs 或 包含单子关键字的工单搜索出来
             */
            QueryParser parser = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, "Title", new PanGuAnalyzer());

            Query query = parser.Parse(Title); //根据名称查询

            //根据id区间区间搜索
            NumericRangeFilter<int> intFilter = null;
            if (MinId > 0 && MaxId == 0)
            {
                intFilter = NumericRangeFilter.NewIntRange("Id", MinId, int.MaxValue, true, true);
            }
            else if (MaxId > 0 && MinId == 0)
            {
                intFilter = NumericRangeFilter.NewIntRange("Id", 0, MaxId, true, true);
            }
            else if (MaxId > 0 && MinId > 0)
            {
                intFilter = NumericRangeFilter.NewIntRange("Id", MinId, MaxId, true, true);
            }
            
            //定义排序
            SortField sortField = new SortField("Id", SortField.INT, false);//降序
            SortField sortField2 = new SortField("CompanyId", SortField.INT, false);//降序
            Sort sort = new Sort(sortField, sortField2);

            //取搜索结果方法1：
            TopDocs docs = searcher.Search(query, intFilter, 1000, sort);//找到的结果取100条数据
            foreach (ScoreDoc sd in docs.ScoreDocs) //从docs.ScoreDocs取数据
            {
                Document doc = searcher.Doc(sd.Doc);
                jobList.Add(new Bpo_JobEntity
                {
                    Id = Convert.ToInt32(doc.Get("Id") ?? "-100"),
                    Title = doc.Get("Title"),
                    UserId = Convert.ToInt32(doc.Get("UserId") ?? "-100"),
                    UserName = doc.Get("UserName"),
                    CompanyId = Convert.ToInt32(doc.Get("CompanyId") ?? "-100"),
                    CompanyName = doc.Get("CompanyName"),
                    FullAddress = doc.Get("FullAddress"),
                    CreateDate = Convert.ToDateTime(doc.Get("CreateDate"))
                });
            }
            return jobList;
        }

        /// <summary>
        ///根据关键字进行查询,根据int的范围进行搜索
        /// </summary>
        /// <param name="Title"></param>
        /// <returns></returns>
        public List<Bpo_JobEntity> SearchJobList4(string Title, string FullAddress, int MinId, int MaxId)
        {
            List<Bpo_JobEntity> jobList = new List<Bpo_JobEntity>();
            string path = "F://LuceneIndexDir";
            FSDirectory dir = FSDirectory.Open(path);

            //IndexSearcher searcher = new IndexSearcher(dir);//声明一个查询器，或者以下面一种方式声明
            IndexReader reader = IndexReader.Open(dir, true);//查询器
            IndexSearcher searcher = new IndexSearcher(reader);

            //搜索条件，可以同时制定多个搜索条件
            PhraseQuery query = new PhraseQuery(); //其实是多个TeamQuery的集合
            query.Add(new Term("Title", Title));//Title中含有Title的工单
            query.Add(new Term("Title", Title.Substring(0, 1)));//Title中含有Title的工单

            //根据id区间区间搜索
            NumericRangeFilter<int> intFilter = null;
            if (MinId > 0 && MaxId == 0)
            {
                intFilter = NumericRangeFilter.NewIntRange("Id", MinId, int.MaxValue, true, true);
            }
            else if (MaxId > 0 && MinId == 0)
            {
                intFilter = NumericRangeFilter.NewIntRange("Id", 0, MaxId, true, true);
            }
            else if (MaxId > 0 && MinId > 0)
            {
                intFilter = NumericRangeFilter.NewIntRange("Id", MinId, MaxId, true, true);
            }

            //定义排序
            SortField sortField = new SortField("Id", SortField.INT, false);//降序
            SortField sortField2 = new SortField("CompanyId", SortField.INT, false);//降序
            Sort sort = new Sort(sortField, sortField2);

            //取搜索结果方法1：
            TopDocs docs = searcher.Search(query, intFilter, 1000, sort);//找到的结果取100条数据
            foreach (ScoreDoc sd in docs.ScoreDocs) //从docs.ScoreDocs取数据
            {
                Document doc = searcher.Doc(sd.Doc);
                jobList.Add(new Bpo_JobEntity
                {
                    Id = Convert.ToInt32(doc.Get("Id") ?? "-100"),
                    Title = doc.Get("Title"),
                    UserId = Convert.ToInt32(doc.Get("UserId") ?? "-100"),
                    UserName = doc.Get("UserName"),
                    CompanyId = Convert.ToInt32(doc.Get("CompanyId") ?? "-100"),
                    CompanyName = doc.Get("CompanyName"),
                    FullAddress = doc.Get("FullAddress"),
                    CreateDate = Convert.ToDateTime(doc.Get("CreateDate"))
                });
            }
            return jobList;
        }

        public ActionResult SearchResult(string Title, string FullAddress, int MinId, int MaxId)
        {
            //List<Bpo_JobEntity> studentModels = SearchJobList(Title);
            //List<Bpo_JobEntity> studentModels = SearchJobList2(Title);

            List<Bpo_JobEntity> studentModels = SearchJobList3(Title, MinId, MaxId);
            //List<Bpo_JobEntity> studentModels = SearchJobList4(Title, FullAddress, MinId, MaxId);

            return View(studentModels);
        }


    }
}