using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Trippie.Startup))]
namespace Trippie
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}
