using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Carolo_TV.Startup))]
namespace Carolo_TV
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
