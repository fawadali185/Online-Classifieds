using AutoMapper;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ListHell.Startup))]
namespace ListHell
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
           
        }
    }
}
