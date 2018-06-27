using AddressWebAPI.Models;
using AddressWebAPI.Providers;
using System.Web.Http;
using System.Web.Http.Cors;

namespace AddressWebAPI.Controllers
{
  [EnableCors(origins: "*", headers: "*", methods: "*")]
  public class ContactController : ApiController
  {
    [HttpGet]
    [Route("api/contact")]
    public APICallingResult searchContact(int tagId, string keyword)
    {
      string outputMessage = null;
      var contacts = AddressProvider.searchContact(tagId, keyword, out outputMessage);

      return new APICallingResult
      {
        Status = contacts != null,
        Data = contacts,
        ProcessMessage = outputMessage
      };
    }

    [HttpGet]
    [Route("api/contact/{id}")]
    public APICallingResult getContact(int id)
    {
      string outputMessage = null;
      var contact = AddressProvider.getContact(id, out outputMessage);

      return new APICallingResult
      {
        Status = true,
        Data = contact,
        ProcessMessage = outputMessage
      };
    }
  }
}