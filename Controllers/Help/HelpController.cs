using CESJapanDataServices.Models;
using CESJapanDataServices.Models.CustomerCare;
using CESJapanDataServices.Providers.Administator;
using CESJapanDataServices.Providers.Help;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace CESJapanDataServices.Controllers.Help
{

    public class HelpController : ApiController
  {
    [HttpGet]
    [Route("Blue/User/{UserID}/BlueDocument")]
    public IList<BlueDocument> BlueDocumentCategorySelect(string UserID, string ClientIntID, string DocumentCategoryID, string DocumentName)
    {
      int userID = 0, documentCategoryID = 0;
      if (!int.TryParse(UserID, out userID))
        userID = 0;
      if (!int.TryParse(DocumentCategoryID, out documentCategoryID))
        documentCategoryID = 0;

      string result = string.Empty;
      if (DocumentName == null)
        DocumentName = string.Empty;

      IList<BlueDocument> ls = HelpProvider.BlueDocumentSelect(userID, documentCategoryID, DocumentName);
      //result = JsonConvert.SerializeObject(ls);
      return ls;
    }
  }
}