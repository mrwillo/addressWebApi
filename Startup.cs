using log4net.Config;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(AddressWebAPI.Startup))]

namespace AddressWebAPI
{
  public partial class Startup
  {
    public void Configuration(IAppBuilder app)
    {
      XmlConfigurator.Configure();
      ConfigureAuth(app);
    }
  }
}