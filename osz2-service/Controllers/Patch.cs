using Microsoft.AspNetCore.Mvc;

namespace osz2_service.Controllers;

public class Patch : ControllerBase
{
    [Route("/osz2/patch")]
    [HttpPost]
    [Consumes("multipart/form-data")]
    [Produces("application/json")]
    public ActionResult Post([FromForm(Name = "osz2")] IFormFile osz2, [FromForm(Name = "patch")] IFormFile patch)
    {
        if (osz2 == null || patch == null)
            return this.BadRequest("No file provided");

        PatchDecryptor patcher = new PatchDecryptor();
        using var data = new MemoryStream();
        
        try
        {
            patcher.Patch(osz2.OpenReadStream(), data, patch.OpenReadStream(), 0);
            return File(data.ToArray(), "application/octet-stream", "osz2");
        }
        catch (Exception e)
        {
            return this.BadRequest(e.Message);
        }
    }
}