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
            LogManager.Adapter = new ConsoleOutLoggerFactoryAdapter();  // 配置适配器
            var factory = new StdSchedulerFactory();                    // 创建调度器工厂
            var sched = factory.GetScheduler();                         // 创建调度器
            sched.ScheduleJob(new JobDetailImpl("jobName", typeof(MyJob))
                , new SimpleTriggerImpl("triggerName", -1, TimeSpan.FromSeconds(1)));  // 执行Job
            sched.Start();          // 启动调度器
            Console.ReadLine();
        }
    }

    class MyJob : IJob
    {
        private readonly ILog _log = LogManager.GetLogger(typeof(MyJob));   // Log
        public void Execute(IJobExecutionContext context)
        {
            _log.Debug("Hello World");  // DEBUG < INFO < WARN < ERROR < FATAL
        }
    }
}
