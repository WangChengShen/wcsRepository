using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Store;
using LuceneNetDemo.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace LuceneNetDemo.Repository
{
    /// <summary>
    /// 索引创建
    /// </summary>
    public class LuceneBuild
    {
        #region  批量BuildIndex 索引合并 

        public bool BuildIndex( List<Bpo_JobEntity> jobList,string rootIndexPath )
        { 
            /* 问题：
             运行时会报找不到\Dict\Dict.dct的问题，解决办法：因为此文件在上一级目录里面，把Dict文件夹粘贴到bin文件夹里面即可；
             */
            if (!System.IO.Directory.Exists(rootIndexPath))
            {
                System.IO.Directory.CreateDirectory(rootIndexPath);
            }
            FSDirectory directory = FSDirectory.Open(rootIndexPath);

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
            return true;
        }


        /// <summary>
        /// 多线程批量创建索引(要求是统一的sourceflag，即目录是一致的)
        /// </summary>
        /// <param name="jobList">sourceflag统一的</param>
        /// <param name="pathSuffix">索引目录后缀，加在电商的路径后面，为空则为根目录.如sa\1</param>
        /// <param name="isCreate">默认为false 增量索引  true的时候删除原有索引</param>
        public void BuildIndexMutiThread(List<Bpo_JobEntity> jobList, string rootIndexPath, bool isCreate = false)
        {
            if (jobList == null || jobList.Count == 0)
                return;
             
            System.IO.DirectoryInfo dirInfo = System.IO.Directory.CreateDirectory(rootIndexPath);
            FSDirectory directory = FSDirectory.Open(dirInfo);
            using (IndexWriter writer = new IndexWriter(directory, new PanGuAnalyzer(), isCreate, IndexWriter.MaxFieldLength.LIMITED))
            {
                writer.SetMaxBufferedDocs(100);//控制写入一个新的segent前内存中保存的doc的数量 默认10  
                writer.MergeFactor = 100;//控制多个segment合并的频率，默认10
                writer.UseCompoundFile = true;//创建复合文件 减少索引文件数量

                foreach (var item in jobList)
                {
                    Document document = new Document();//一条数据

                    //一个字段，列名，值，是否保存值，是否分词
                    //Field.Store.NO 代表不存值，这样就不会生产索引，取值时为null 
                    document.Add(new NumericField("Id", Field.Store.YES, true).SetIntValue(item.Id));
                    document.Add(new Field("Title", item.Title ?? "".ToString(), Field.Store.YES, Field.Index.ANALYZED));
                    document.Add(new NumericField("UserId", Field.Store.YES, true).SetIntValue(item.UserId));
                    document.Add(new Field("UserName", item.UserName??"".ToString(), Field.Store.YES, Field.Index.ANALYZED));
                    document.Add(new NumericField("CompanyId", Field.Store.YES, true).SetIntValue(item.CompanyId));
                    document.Add(new Field("CompanyName", item.CompanyName??"".ToString(), Field.Store.YES, Field.Index.ANALYZED));
                    document.Add(new Field("FullAddress", item.FullAddress ?? "".ToString(), Field.Store.YES, Field.Index.ANALYZED));
                    // document.Add(new NumericField("CreateTime", Field.Store.YES, true).SetIntValue(int.Parse(item.CreateTime.ToString("yyyyMMdd"))));
                    document.Add(new Field("CreateDate", item.CreateDate.ToString("yyyy-MM-dd HH:mm:ss"), Field.Store.YES, Field.Index.NOT_ANALYZED));
                    writer.AddDocument(document);
                }
                //  writer.Optimize();//优化，合并 (多线程创建索引的时候不做优化合并，Merge的时候处理)
            }
        }

        /// <summary>
        /// 将索引合并到上级目录
        /// </summary>
        /// <param name="sourceDir">子文件夹名</param>
        public void MergeIndex(string rootPath, string[] childDirs)
        {
            if (childDirs == null || childDirs.Length == 0) return;

            Analyzer analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);

            System.IO.DirectoryInfo footDirInfo = System.IO.Directory.CreateDirectory(rootPath);
            FSDirectory directory = FSDirectory.Open(footDirInfo);

            using (IndexWriter writer = new IndexWriter(directory, analyzer, true, IndexWriter.MaxFieldLength.LIMITED))
            {
                FSDirectory[] dirNo = childDirs.Select(dir =>
                {
                    System.IO.DirectoryInfo dirInfo = System.IO.Directory.CreateDirectory(dir);
                    return FSDirectory.Open(dirInfo);
                }).ToArray();

                writer.MergeFactor = 100;//控制多个segment合并的频率，默认10
                writer.UseCompoundFile = true;//创建符合文件 减少索引文件数量
                writer.AddIndexesNoOptimize(dirNo);
                writer.Optimize();
            }


        }
        #endregion

        #region 单个/批量索引增删改
        /// <summary>
        /// 新增一条数据的索引
        /// </summary>
        /// <param name="jobEntity"></param>
        public void InsertIndex(Bpo_JobEntity jobEntity, string rootPath)
        {
            if (jobEntity == null) return;

            System.IO.DirectoryInfo footDirInfo = System.IO.Directory.CreateDirectory(rootPath);

            bool isCreate = footDirInfo.GetFiles().Count() == 0;//下面没有文件则为新建索引 

            FSDirectory directory = FSDirectory.Open(footDirInfo);
            using (IndexWriter writer = new IndexWriter(directory, CreateAnalyzerWrapper(), isCreate, IndexWriter.MaxFieldLength.LIMITED))
            {
                writer.MergeFactor = 100;//控制多个segment合并的频率，默认10
                writer.UseCompoundFile = true;//创建符合文件 减少索引文件数量
                Document document = new Document();//一条数据

                //一个字段，列名，值，是否保存值，是否分词
                //Field.Store.NO 代表不存值，这样就不会生产索引，取值时为null 
                document.Add(new NumericField("Id", Field.Store.YES, true).SetIntValue(jobEntity.Id));
                document.Add(new Field("Title", jobEntity.Title ?? "".ToString(), Field.Store.YES, Field.Index.ANALYZED));
                document.Add(new NumericField("UserId", Field.Store.YES, true).SetIntValue(jobEntity.UserId));
                document.Add(new Field("UserName", jobEntity.UserName.ToString(), Field.Store.YES, Field.Index.ANALYZED));
                document.Add(new NumericField("CompanyId", Field.Store.YES, true).SetIntValue(jobEntity.CompanyId));
                document.Add(new Field("CompanyName", jobEntity.CompanyName.ToString(), Field.Store.YES, Field.Index.ANALYZED));
                document.Add(new Field("FullAddress", jobEntity.FullAddress ?? "".ToString(), Field.Store.YES, Field.Index.ANALYZED));
                // document.Add(new NumericField("CreateTime", Field.Store.YES, true).SetIntValue(int.Parse(jobEntity.CreateTime.ToString("yyyyMMdd"))));
                document.Add(new Field("CreateDate", jobEntity.CreateDate.ToString("yyyy-MM-dd HH:mm:ss"), Field.Store.YES, Field.Index.NOT_ANALYZED));
                writer.AddDocument(document);
            }
        }

        /// <summary>
        /// 创建分析器
        /// </summary>
        /// <returns></returns>
        private PerFieldAnalyzerWrapper CreateAnalyzerWrapper()
        {
            Analyzer analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);

            PerFieldAnalyzerWrapper analyzerWrapper = new PerFieldAnalyzerWrapper(analyzer);
            analyzerWrapper.AddAnalyzer("title", new PanGuAnalyzer());
            analyzerWrapper.AddAnalyzer("categoryid", new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30));
            return analyzerWrapper;
        }

        /// <summary>
        /// 删除指定的索引
        /// </summary>
        /// <param name="ci"></param>
        public void DeleteIndex(Bpo_JobEntity jobEntity, string rootIndexPath)
        {
            if (jobEntity == null) return;
            Analyzer analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);

            System.IO.DirectoryInfo dirInfo = System.IO.Directory.CreateDirectory(rootIndexPath);
            FSDirectory directory = FSDirectory.Open(dirInfo);
            using (IndexReader reader = IndexReader.Open(directory, false))
            {
                reader.DeleteDocuments(new Term("Id", jobEntity.Id.ToString()));
            }
        }

        /// <summary>
        /// 批量删除数据的索引
        /// </summary>
        /// <param name="jobList"></param>
        public void DeleteIndexMuti(List<Bpo_JobEntity> jobList, string rootIndexPath)
        {
            if (jobList == null || jobList.Count == 0) return;
            Analyzer analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);

            System.IO.DirectoryInfo dirInfo = System.IO.Directory.CreateDirectory(rootIndexPath);
            FSDirectory directory = FSDirectory.Open(dirInfo);
            using (IndexReader reader = IndexReader.Open(directory, false))
            {
                foreach (Bpo_JobEntity job in jobList)
                {
                    reader.DeleteDocuments(new Term("Id", job.Id.ToString()));
                }
            }
        }
         
        /// <summary>
        /// 更新一条数据的索引
        /// </summary>
        /// <param name="ci"></param>
        public void UpdateIndex(Bpo_JobEntity job, string rootIndexPath)
        {
            if (job == null) return;
            System.IO.DirectoryInfo dirInfo = System.IO.Directory.CreateDirectory(rootIndexPath);

            bool isCreate = dirInfo.GetFiles().Count() == 0;//下面没有文件则为新建索引 

            FSDirectory directory = FSDirectory.Open(dirInfo);
            using (IndexWriter writer = new IndexWriter(directory, CreateAnalyzerWrapper(), isCreate, IndexWriter.MaxFieldLength.LIMITED))
            {
                writer.MergeFactor = 100;//控制多个segment合并的频率，默认10
                writer.UseCompoundFile = true;//创建符合文件 减少索引文件数量

                Document document = new Document();//一条数据

                //一个字段，列名，值，是否保存值，是否分词
                //Field.Store.NO 代表不存值，这样就不会生产索引，取值时为null 
                document.Add(new NumericField("Id", Field.Store.YES, true).SetIntValue(job.Id));
                document.Add(new Field("Title", job.Title ?? "".ToString(), Field.Store.YES, Field.Index.ANALYZED));
                document.Add(new NumericField("UserId", Field.Store.YES, true).SetIntValue(job.UserId));
                document.Add(new Field("UserName", job.UserName.ToString(), Field.Store.YES, Field.Index.ANALYZED));
                document.Add(new NumericField("CompanyId", Field.Store.YES, true).SetIntValue(job.CompanyId));
                document.Add(new Field("CompanyName", job.CompanyName.ToString(), Field.Store.YES, Field.Index.ANALYZED));
                document.Add(new Field("FullAddress", job.FullAddress ?? "".ToString(), Field.Store.YES, Field.Index.ANALYZED));
                // document.Add(new NumericField("CreateTime", Field.Store.YES, true).SetIntValue(int.Parse(job.CreateTime.ToString("yyyyMMdd"))));
                document.Add(new Field("CreateDate", job.CreateDate.ToString("yyyy-MM-dd HH:mm:ss"), Field.Store.YES, Field.Index.NOT_ANALYZED));

                writer.UpdateDocument(new Term("Id", job.Id.ToString()), document);
            }

        }

        /// <summary>
        /// 批量更新数据的索引
        /// </summary>
        /// <param name="jobList">sourceflag统一的</param>
        public void UpdateIndexMuti(List<Bpo_JobEntity> jobList, string rootIndexPath)
        {
            if (jobList == null || jobList.Count == 0) return;

            foreach (var job in jobList)
            {
                UpdateIndex(job, rootIndexPath);
            }
        }
        #endregion
    }
}