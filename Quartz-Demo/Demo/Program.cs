using System;
using System.Threading;
using Common.Logging;
using Common.Logging.Simple;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Triggers;

namespace Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            LogManager.Adapter = new ConsoleOutLoggerFactoryAdapter {Level = LogLevel.Info};  // 配置适配器
            var factory = new StdSchedulerFactory();                    // 创建调度器工厂
            var scheduler = factory.GetScheduler();                         // 创建调度器
            var sched1 = StdSchedulerFactory.GetDefaultScheduler();     // 快速获取调度器
            scheduler.ScheduleJob(new JobDetailImpl("jobName", typeof(MyJob))
                , new SimpleTriggerImpl("triggerName", -1, TimeSpan.FromSeconds(1)));  // 快速执行简单Job
            scheduler.Start();          // 启动调度器,应用程序将不会终止，直到你调用scheduler.shutdown（），因为会有活动线程（非守护线程）。
            ScheduleJob(scheduler);          // 添加标准Job和Trigger
            Thread.Sleep(TimeSpan.FromSeconds(10)); // 主线程睡10s
            scheduler.Shutdown();       // 停止调度器
        }

        private static void ScheduleJob(IScheduler scheduler)
        {
            // define the job and tie it to our MyJob class
            IJobDetail job = JobBuilder.Create<MyJob>()
                .WithIdentity("job1", "group1")
                .Build();

            // Trigger the job to run now, and then repeat every 1 seconds
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("trigger1", "group1")
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(2)
                    .RepeatForever())
                .Build();

            // Tell quartz to schedule the job using our trigger
            scheduler.ScheduleJob(job, trigger);
        }
    }

    class MyJob : IJob
    {
        private readonly ILog _log = LogManager.GetLogger(typeof(MyJob));   // Log

        public void Execute(IJobExecutionContext context)
        {
            _log.Info("Hello World");  // DEBUG < INFO < WARN < ERROR < FATAL
        }
    }
}
