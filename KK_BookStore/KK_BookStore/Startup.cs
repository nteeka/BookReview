using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(KK_BookStore.Startup))]
namespace KK_BookStore
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            
            ConfigureAuth(app);
            
        }

    }
}
