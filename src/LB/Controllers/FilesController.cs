using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace LB.Controllers
{
    public class FilesController : ApiController
    {
        [HttpPost]
        public async Task<IHttpActionResult> Post()
        {
            try
            {
                if (!Request.Content.IsMimeMultipartContent())
                {
                    return BadRequest("Not Mime Multipart Content");
                }

                var provider = new MultipartMemoryStreamProvider();

                await Request.Content.ReadAsMultipartAsync(provider);

                var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Upload");

                foreach (var file in provider.Contents)
                {
                    var fileName = Path.GetFileName(file.Headers.ContentDisposition.FileName.Trim('\"'));
                    var fullPath = Path.Combine(path, fileName);
                    var buffer = await file.ReadAsByteArrayAsync();
                    File.WriteAllBytes(fullPath, buffer);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
