using Entidades;
using Negocio;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
namespace WebApiFenosa.Controllers
{
    public class MigrationController : ApiController
    {

        [HttpGet]
        [Route("api/Migration/GetLogin")]
        public IHttpActionResult GetLogin(string user, string password)
        {
            Login login = ServiciosDA.GetOne(user, password);

            if (login != null)
            {
                return Ok(login);
            }
            else return NotFound();

        }

        [HttpGet]
        [Route("api/Migration/MigracionAll")]
        public IHttpActionResult MigracionAll(int operarioId)
        {
            Migracion migracion = MigrationDA.getMigracion(operarioId);

            if (migracion != null)
            {
                return Ok(migracion);
            }
            else return NotFound();

        }

        [HttpPost]
        [Route("api/Migration/SaveListRegistro")]
        public IHttpActionResult SaveListRegistro(List<Registro> registroList)
        {
            Mensaje mensaje = MigrationDA.saveListRegistro(registroList);
            if (mensaje != null)
            {
                return Ok(mensaje);
            }
            else return NotFound();

        }

        [HttpPost]
        [Route("api/Migration/SaveRegistro")]
        public IHttpActionResult SaveListRegistro(Registro registroList)
        {
            Mensaje mensaje = MigrationDA.saveRegistro(registroList);
            if (mensaje != null)
            {
                return Ok(mensaje);
            }
            else return NotFound();
        }

        [HttpPost] // This is from System.Web.Http, and not from System.Web.Mvc
        [Route("api/Migration/SavePhoto")]
        public HttpResponseMessage Upload()
        {

            try
            {
                //string path = "C:/HostingSpaces/admincobraperu/www.cobraperu.com/wwwroot/Calidda/Content/foto/foto/";
                string path = HttpContext.Current.Server.MapPath("~/Imagen/");
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
            if (mensaje != null)
            {
                return Ok(mensaje);
            }
            else return NotFound();
        }

    }
}
