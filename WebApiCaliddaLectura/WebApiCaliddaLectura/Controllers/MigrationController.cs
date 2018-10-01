using Entidades;
using Entities;
using Negocio;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
namespace WebApiFenosa.Controllers
{
    public class MigrationController : ApiController
    {

        [HttpGet]
        [Route("api/Migration/GetLogin")]
        public IHttpActionResult GetLogin(string user, string password, string version, string imei, string token)
        {
            Login login = ServiciosDA.GetOne(user, password, version, imei, token);

            if (login != null)
            {
                return Ok(login);
            }
            else return NotFound();

        }

        [HttpGet]
        [Route("api/Migration/MigracionAll")]
        public IHttpActionResult MigracionAll(int operarioId, string version)
        {
            Migracion migracion = MigrationDA.getMigracion(operarioId, version);
            return Ok(migracion);
        }


        [HttpGet]
        [Route("api/Migration/SincronizarObservadas")]
        public IHttpActionResult MigracionAll(int operarioId)
        {
            List<Suministro> suministro = MigrationDA.getObservadas(operarioId);
            return Ok(suministro);
        }


        [HttpPost]
        [Route("api/Migration/SaveListRegistro")]
        public IHttpActionResult SaveListRegistro(List<Registro> registroList)
        {
            Mensaje mensaje = MigrationDA.saveListRegistro(registroList);
            return Ok(mensaje);
        }


        [HttpPost]
        [Route("api/Migration/SaveRegistro")]
        public IHttpActionResult SaveListRegistro(Registro registroList)
        {
            Mensaje mensaje = MigrationDA.saveRegistro(registroList);
            return Ok(mensaje);
        }


        [HttpPost] // This is from System.Web.Http, and not from System.Web.Mvc
        [Route("api/Migration/SavePhoto")]
        public HttpResponseMessage Upload()
        {
            try
            {
                string path = "C:/HostingSpaces/admincobraperu/www.cobraperu.com/wwwroot/Calidda/Content/foto/foto/";
                //string path = HttpContext.Current.Server.MapPath("~/Imagen/");
                System.Web.HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;
                string fileName = Path.GetFileName(files[0].FileName);
                files[0].SaveAs(path + fileName);
                return this.Request.CreateResponse(HttpStatusCode.OK, new { mensaje = "Mensaje enviado" });
            }
            catch (Exception)
            {
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, new { mensaje = "Error" });
            }
        }

        [HttpGet]
        [Route("api/Migration/VerificarCorte")]
        public IHttpActionResult VerificarCorte(string suministro)
        {
            Mensaje mensaje = MigrationDA.verificarCorte(suministro);
            return Ok(mensaje);
        }


        [HttpGet]
        [Route("api/Migration/SincronizarCorteReconexion")]
        public IHttpActionResult sincronizarCorteReconexion(int operarioId)
        {
            Sincronizar sync = ServiciosDA.sincronizar(operarioId);
            if (sync.suministrosCortes == null & sync.suministroReconexion == null)
            {
                return NotFound();
            }
            else return Ok(sync);
        }


        [Route("api/Migration/SavePhoto2")]
        [AllowAnonymous]
        public async Task<HttpResponseMessage> PostUserImage()
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            try
            {
                var httpRequest = HttpContext.Current.Request;

                foreach (string file in httpRequest.Files)
                {
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);

                    var postedFile = httpRequest.Files[file];
                    if (postedFile != null && postedFile.ContentLength > 0)
                    {

                        int MaxContentLength = 1024 * 1024 * 1; //Size = 1 MB  

                        IList<string> AllowedFileExtensions = new List<string> { ".jpg", ".gif", ".png" };
                        var ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                        var extension = ext.ToLower();
                        if (!AllowedFileExtensions.Contains(extension))
                        {
                            var message = string.Format("Please Upload image of type .jpg,.gif,.png.");
                            dict.Add("error", message);
                            return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                        }
                        else if (postedFile.ContentLength > MaxContentLength)
                        {
                            var message = string.Format("Please Upload a file upto 1 mb.");
                            dict.Add("error", message);
                            return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                        }
                        else
                        {
                            var filePath = HttpContext.Current.Server.MapPath("~/Imagen/" + postedFile.FileName + extension);
                            postedFile.SaveAs(filePath);
                        }
                    }

                    var message1 = string.Format("ok");
                    return Request.CreateErrorResponse(HttpStatusCode.Created, message1); ;
                }
                var res = string.Format("Please Upload a image.");
                dict.Add("error", res);
                return Request.CreateResponse(HttpStatusCode.NotFound, dict);
            }
            catch (Exception ex)
            {
                var res = string.Format("some Message");
                dict.Add("error", res);
                return Request.CreateResponse(HttpStatusCode.NotFound, dict);
            }
        }

    }
}
