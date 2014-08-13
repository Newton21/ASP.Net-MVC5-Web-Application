using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(UMSMVC5.Startup))]
namespace UMSMVC5
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
