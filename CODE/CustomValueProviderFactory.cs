using System.Web;
using System.Web.Mvc;
using System.Globalization;
namespace Listhell.CODE
{
  public class CustomValueProviderFactory : ValueProviderFactory
  {
    public override IValueProvider GetValueProvider(ControllerContext controllerContext)
    {
      return new CustomValueProvider();
    }
  }
}