using log4net;
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
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace LuceneNetDemo.Controllers
{
    /// <summary>
    /// 创建.Net Framwork Web项目，不支持Core Web
    /// 需引入以下dll：
    /// Lucene.Net
    /// PanGu.HighLight
    /// PanGu.Lucene.Analyzer
    /// 
    /// 常用的工具类：
    /// Analysis：分词器，负责把字符串拆分成原子，包含了标准分词，直接空格拆分项目中用的是盘古中文分词
    /// Document:数据结构，定义存储数据的格式
    /// Index:索引的读写类
    /// QueryParser:查询解析器，负责解析查询语句
    /// Search：负责各种查询类，命令解析后得到就是查询类
    /// Store：索引存储类，负责文件夹等等；
    /// Util:常见工具类库；
    /// </summary>
    public class HomeController : Controller
    {
        ILog logHelper = log4net.LogManager.GetLogger("HomeController");
        public string path = "F://LuceneIndexDir";
        public ActionResult Index()
        {
            /*
             1.nuget 引入Log4net.dll
             2.项目启动初始化log4net
             3.像下面这样进行使用
             */
            logHelper.Info("HomeController-Index");
            return View();
        }

        /// <summary>
        ///单线程创建索引
        /// </summary>
        /// <returns></returns>
        public int CreateIndex()
        {
            List<Bpo_JobEntity> jobList = DataRepository.GetJobList(1, 1000);
            LuceneBuild luceneBuild = new LuceneBuild();
            return luceneBuild.BuildIndex(jobList, path) ? 1 : 0;
        }

        #region 多线程生成索引 
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        List<string> childDirList = new List<string>();
        public int CreateIndexMutiThread(int taskCount)
        {
            List<Task> taskList = new List<Task>();
            for (int i = 1; i <= taskCount; i++)
            {
                string childPath = $"{path}//{i.ToString("000")}";
                childDirList.Add(childPath);
                logHelper.Info($"createIndexMutiThread{i}");
                Task task = Task.Run(() =>
                {
                    createIndexMutiThread(i, cancellationTokenSource, childPath, true);
                });
                taskList.Add(task);
                Thread.Sleep(200);
            }

            taskList.Add(Task.Factory.ContinueWhenAll(taskList.ToArray(), mergeIndex));
            Task.WaitAll(taskList.ToArray());  //为了展示出多线程的异常
            logHelper.Debug(cancellationTokenSource.IsCancellationRequested ? "失败" : "成功");
            return 1;
        }

        private bool createIndexMutiThread(int taskCount,
            CancellationTokenSource cancellationTokenSource, string rootIndexPath, bool isCreate = false)
        {
            try
            {
                if (!cancellationTokenSource.IsCancellationRequested)
                {
                    int pageNum = taskCount;
                    List<Bpo_JobEntity> jobList = DataRepository.GetJobList(pageNum, 800);

                    new LuceneBuild().BuildIndexMutiThread<Bpo_JobEntity>(jobList, rootIndexPath, true);
                    logHelper.Info($"线程{taskCount}完成1000数据");
                    return true;
                }
            }
            catch (Exception ex)
            {
                logHelper.Error($"线程{taskCount}出现异常", ex);
                cancellationTokenSource.Cancel();
            }
            return false;
        }

        private bool mergeIndex(Task[] tasks)
        {
            try
            {
                if (cancellationTokenSource.IsCancellationRequested)
                {
                    return false;
                }
                LuceneBuild luceneBuild = new LuceneBuild();

                luceneBuild.MergeIndex(path, childDirList.ToArray());

                return true;
            }
            catch (Exception ex)
            {
                logHelper.Error($"合并索引出现异常", ex);
                return false;
            }
        }
        #endregion

        #region 单条操作
        /// <summary>
        /// 新增一条数据的索引
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        public int InsertIndex(int jobId)
        {
            Bpo_JobEntity jobEntity = DataRepository.GetJobById(jobId);
            if (jobEntity == null) return 0;

            LuceneBuild luceneBuild = new LuceneBuild();
            luceneBuild.InsertIndex(jobEntity, path);
            return 1;
        }
        /// <summary>
        /// 删除一条数据的索引
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        public int DeleteIndex(int jobId)
        {
            Bpo_JobEntity jobEntity = DataRepository.GetJobById(jobId);
            if (jobEntity == null) return 0;

            LuceneBuild luceneBuild = new LuceneBuild();
            luceneBuild.DeleteIndex<Bpo_JobEntity>(jobEntity, "Id", path);
            return 1;
        }

        /// <summary>
        /// 更新一条数据的索引
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        public int UpdateIndex(int jobId)
        {
            Bpo_JobEntity jobEntity = DataRepository.GetJobById(jobId);
            if (jobEntity == null) return 0;
            jobEntity.UserName = "王承申";

            LuceneBuild luceneBuild = new LuceneBuild();
            luceneBuild.UpdateIndex<Bpo_JobEntity>(jobEntity,"Id", path);
            return 1;
        }
        #endregion

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

            //搜索条件，BooleanQuery可以同时制定多个搜索条件  
            BooleanQuery booleanQuery = new BooleanQuery();
            //Query query = new TermQuery(new Term("Title", $"*{Title}*"));//不支持通配符 
            //Query query = new WildcardQuery(new Term("Title", $"*{Title}*")); // 通配符 

            if (!string.IsNullOrEmpty(Title))//空格隔开，按照多个词进行搜索
            {
                Title = new LuceneAnalyze().AnalyzerKeyword("Title", Title);
                QueryParser parser = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, "Title", new PanGuAnalyzer());
                Query query = parser.Parse(Title);
                booleanQuery.Add(query, Occur.MUST);
            }
            if (MinId > 0)//空格隔开，按照多个词进行搜索
            {
                //string idStr = new LuceneAnalyze().AnalyzerKeyword("Id", MinId.ToString());
                QueryParser parser = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, "Id", new PanGuAnalyzer());
                Query query = parser.Parse(MinId.ToString());
                booleanQuery.Add(query, Occur.MUST);
            }

            if (!string.IsNullOrEmpty(FullAddress))
            {
                FullAddress = new LuceneAnalyze().AnalyzerKeyword("FullAddress", FullAddress);
                QueryParser parser = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, "FullAddress", new PanGuAnalyzer());
                Query query = parser.Parse(FullAddress);
                booleanQuery.Add(query, Occur.MUST);

                //使用WildcardQuery，相当于sql的like
                //Query query2 = new WildcardQuery(new Term("FullAddress", $"*{FullAddress}*")); // 通配符 
                //booleanQuery.Add(query2, Occur.MUST);
            }

            //根据id区间区间搜索(此时id存储索引时要是NumericField类型)
            NumericRangeFilter<int> intFilter = null;
            //if (MinId > 0 && MaxId == 0)
            //{
            //    intFilter = NumericRangeFilter.NewIntRange("Id", MinId, int.MaxValue, true, true);
            //}
            //else if (MaxId > 0 && MinId == 0)
            //{
            //    intFilter = NumericRangeFilter.NewIntRange("Id", 0, MaxId, true, true);
            //}
            //else if (MaxId > 0 && MinId > 0)
            //{
            //    intFilter = NumericRangeFilter.NewIntRange("Id", MinId, MaxId, true, true);
            //}

            //定义排序
            SortField sortField = new SortField("Id", SortField.STRING, false);//降序
            SortField sortField2 = new SortField("CompanyId", SortField.INT, false);//降序
            Sort sort = new Sort(sortField, sortField2);

            //取搜索结果方法1：
            TopDocs docs = searcher.Search(booleanQuery, intFilter, 10000, sort);//找到的结果取100条数据
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

            // List<Bpo_JobEntity> studentModels = SearchJobList3(Title, MinId, MaxId);
            List<Bpo_JobEntity> studentModels = SearchJobList4(Title, FullAddress, MinId, MaxId);

            return View(studentModels);
        }




    }
}