using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
[assembly: OwinStartup(typeof(PepuxService.Startup))]


namespace PepuxService
{
    public class Startup
    {
        System.Timers.Timer timer = new System.Timers.Timer();
        public void Configuration(IAppBuilder app)
        {

                timer.Elapsed += new System.Timers.ElapsedEventHandler(update_Elapsed);
                timer.Interval = 120000;
                timer.Enabled = true;
                timer.Start();
            
            
        }
        void update_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Debug.WriteLine("TimerTest");
        }
    }
}
