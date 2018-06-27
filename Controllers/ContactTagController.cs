using AddressWebAPI.Models;
using AddressWebAPI.Providers;
using System.Web.Http;
using System.Web.Http.Cors;

namespace AddressWebAPI.Controllers
{
  [EnableCors(origins: "*", headers: "*", methods: "*")]
  public class ContactTagController : ApiController
  {
    [HttpGet]
    [Route("api/tags/")]
    public APICallingResult getTag()
    {
      string outputMessage = null;
      var tags = AddressProvider.getTags(out outputMessage);

      return new APICallingResult
      {
        Status = tags != null,
        Data = tags,
        ProcessMessage = outputMessage
      };
    }

    [HttpDelete]
    [Route("api/tags/{id}")]
    public APICallingResult deleteTag(int id)
    {
      int idOut = 0;
      string outputMessage = null;
      AddressProvider.deleteTag(id, out idOut, out outputMessage);

      return new APICallingResult
      {
        Status = (idOut == id),
        Data = null,
        Id = idOut,
        ProcessMessage = outputMessage
      };
    }

    [HttpPost]
    [HttpPut]
    [Route("api/tags/")]
    public APICallingResult mergeTag(ContactTagModel tagModel)
    {
      string outputMessage = null;
      int idOut = 0;
      var tag = AddressProvider.mergeTag(tagModel, out idOut, out outputMessage);

      return new APICallingResult
      {
        Status = tag != null,
        Data = tag,
        ProcessMessage = outputMessage,
        Id = idOut
      };
    }


    [HttpPut]
    [Route("api/tags/")]
    public APICallingResult assignTag(int contactId, int tagId)
    {
      string outputMessage = null;
      int idOut = 0;
      AddressProvider.changeTagAssign(contactId, tagId, out idOut, out outputMessage);

      return new APICallingResult
      {
        Status = true,
        Data = null,
        ProcessMessage = outputMessage,
        Id = idOut
      };
    }

  }
}