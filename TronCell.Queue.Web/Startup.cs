using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TronCell.Queue.Web.Startup))]
namespace TronCell.Queue.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
