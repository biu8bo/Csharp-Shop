using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commons.Quartz
{
    /// <summary>
    /// 任务调度基类
    /// </summary>
    [DisallowConcurrentExecution()]
    public abstract class JobBase : IJob
    {
        #region IJob 成员

        public Task Execute(IJobExecutionContext context)
        {
            try
            {
                ExcuteJob(context);
                return Task.CompletedTask; ;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
        /// <summary>
        /// 执行计划，子类可以重写
        /// </summary>
        public virtual string Cron => "0/1 * * * * ?";
        /// <summary>
        /// Job具体类去实现自己的逻辑
        /// </summary>
        protected abstract void ExcuteJob(IJobExecutionContext context);
    }
}
