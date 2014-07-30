using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PneuservisISMVC.Startup))]
namespace PneuservisISMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
