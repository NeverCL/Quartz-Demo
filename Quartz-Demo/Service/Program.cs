using System;
using System.Timers;
using Topshelf;

namespace Service
{
    public class TownCrier
    {
        readonly Timer _timer;
        public TownCrier()
        {
            _timer = new Timer(1000) { AutoReset = true };
            _timer.Elapsed += (sender, eventArgs) => Console.WriteLine("It is {0} and all is well", DateTime.Now);
        }
        public void Start() { _timer.Start(); }
        public void Stop() { _timer.Stop(); }
    }


    public class Program
    {
        public static void Main()
        {
            HostFactory.Run(x =>
            {
                x.Service<TownCrier>(s =>                     
                {
                    s.ConstructUsing(name => new TownCrier());  // 告诉Topshelf如何构建服务的实例
                    s.WhenStarted(tc => tc.Start());            // 如何启动服务
                    s.WhenStopped(tc => tc.Stop());             // 如何停止服务
                });
                x.RunAsLocalSystem();                           // 配置运行方式

                x.SetDescription("Sample Topshelf Host");       // winservice的描述
                x.SetDisplayName("Stuff");                      // winservice的显示名称
                x.SetServiceName("Stuff");                      // winservice的服务名称
            });                                                     
        }
    }
}
