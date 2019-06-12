using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MovieAPIProjectV2.Startup))]
namespace MovieAPIProjectV2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
