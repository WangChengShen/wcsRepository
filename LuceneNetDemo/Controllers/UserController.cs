using log4net;
using LuceneNetDemo.Models;
using LuceneNetDemo.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace LuceneNetDemo.Controllers
{
    /// <summary>
    /// 测试生成索引速度
    /// </summary>
    public class UserController : Controller
    {
        ILog logHelper = log4net.LogManager.GetLogger("UserController");
        public string path = "F://UserLuceneIndexDir";
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        private int rowCount = 10000000;
        /// <summary>
        ///单线程创建索引
        /// </summary>
        /// <returns></returns>
        public int CreateIndex()
        {
            DateTime startTime = DateTime.Now;
            List<Bpo_JobEntity> userList = DataRepository.GetJobList(1, rowCount);
            LuceneBuild luceneBuild = new LuceneBuild();
            int result = luceneBuild.BuildIndex(userList, path) ? 1 : 0;

            double time = (DateTime.Now - startTime).TotalSeconds;
            logHelper.Info($"创建用户({rowCount}条)索引时间：{time}秒");
            return result;
        }

        #region 多线程生成索引 
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        List<string> childDirList = new List<string>();

        public int CreateIndexMutiThread(int taskCount)
        {
            List<Bpo_JobEntity> userList = DataRepository.GetJobList(1, rowCount);

            int totalCount = userList.Count;

            int yunCount = (int)Math.Floor(Convert.ToDouble(totalCount / taskCount));
            int yu = totalCount % taskCount;
            DateTime startTime = DateTime.Now;
            List<Task> taskList = new List<Task>();
            for (int i = 1; i <= taskCount; i++)
            {
                string childPath = $"{path}//{i.ToString("000")}";
                childDirList.Add(childPath);
                logHelper.Info($"createIndexMutiThread{i}");

                List<Bpo_JobEntity> data = null;

                if (i == taskCount && yu > 0)
                {
                    data = userList.Skip((i - 1) * yunCount).Take(yunCount + yu).ToList();
                }
                else
                {
                    data = userList.Skip((i - 1) * yunCount).Take(yunCount).ToList();
                }
                Task task = Task.Run(() =>
                {
                    createIndexMutiThread(data, i, cancellationTokenSource, childPath, true);
                });
                taskList.Add(task);
                //Thread.Sleep(200);
            }

            taskList.Add(Task.Factory.ContinueWhenAll(taskList.ToArray(), mergeIndex));
            Task.WaitAll(taskList.ToArray());  //为了展示出多线程的异常
            logHelper.Debug(cancellationTokenSource.IsCancellationRequested ? "失败" : "成功");

            double time = (DateTime.Now - startTime).TotalSeconds;
            logHelper.Info($"多线程完成{userList.Count}条数据，耗时{time}秒");
            return 1;
        }

        private bool createIndexMutiThread(List<Bpo_JobEntity> userList, int taskCount,
            CancellationTokenSource cancellationTokenSource, string rootIndexPath, bool isCreate = false)
        {
            try
            {
                if (!cancellationTokenSource.IsCancellationRequested)
                {
                    int pageNum = taskCount;
                    DateTime startTime = DateTime.Now;
                    new LuceneBuild().BuildIndexMutiThread<Bpo_JobEntity>(userList, rootIndexPath, true);
                    double time = (DateTime.Now - startTime).TotalSeconds;
                    logHelper.Info($"线程{taskCount}完成{userList.Count}条数据，耗时{time}秒");

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



    }
}