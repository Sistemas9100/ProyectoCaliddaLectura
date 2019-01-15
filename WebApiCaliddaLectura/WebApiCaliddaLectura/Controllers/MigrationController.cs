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

        [HttpGet]
        [Route("api/Migration/GetOperarioById")]
        public IHttpActionResult GetOperarioById(int operarioId)
        {
            Mensaje operarios = MigrationDA.getOperarioById(operarioId);
            return Ok(operarios);
        }

        [HttpGet]
        [Route("api/Migration/GetOperarios")]
        public IHttpActionResult GetOperarios()
        {
            List<Operario> operarios = MigrationDA.getOperarios();
            return Ok(operarios);
        }

        [HttpGet]
        [Route("api/Migration/UpdateOperario")]
        public IHttpActionResult UpdateOperario(int operarioId, int lecturaManual)
        {
            Mensaje mensaje = MigrationDA.updateOperario(operarioId, lecturaManual);
            return Ok(mensaje);
        }


        [HttpPost]
        [Route("api/Migration/SaveOperarioGps")]
        public IHttpActionResult SaveOperarioGps(EstadoOperario estadoOperario)
        {
            Mensaje mensaje = ServiciosDA.saveOperarioGps(estadoOperario);
            return Ok(mensaje);
        }

        [HttpPost]
        [Route("api/Migration/SaveEstadoMovil")]
        public IHttpActionResult SaveEstadoMovil(EstadoMovil estadoMovil)
        {
            Mensaje mensaje = ServiciosDA.saveEstadoMovil(estadoMovil);
            return Ok(mensaje);
        }


        // NUEVOOOOOOOOOOOOOOOOOOOOO

        [HttpPost]
        [Route("api/Migration/Save")]
        public IHttpActionResult SaveRegistroMasivo()
        {
            try
            {
                //string path = HttpContext.Current.Server.MapPath("~/Imagen/");
                string path = "C:/HostingSpaces/admincobraperu/www.cobraperu.com/wwwroot/Calidda/Content/foto/foto/";
                var fotos = HttpContext.Current.Request.Files;
                var json = HttpContext.Current.Request.Form["model"];
                Registro p = JsonConvert.DeserializeObject<Registro>(json);

                Mensaje mensaje = MigrationDA.saveRegistroRx(p);

                if (mensaje != null)
                {
                    for (int i = 0; i < fotos.Count; i++)
                    {
                        string fileName = Path.GetFileName(fotos[i].FileName);
                        fotos[i].SaveAs(path + fileName);
                    }
                }
                else
                {
                    mensaje = new Mensaje();
                    mensaje.mensaje = "Registro repetido";
                }

                return Ok(mensaje);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        [Route("api/Migration/SaveRegistroCorte")]
        public IHttpActionResult SaveRegistroCorte()
        {
            try
            {
                //string path = HttpContext.Current.Server.MapPath("~/Imagen/");
                string path = "C:/HostingSpaces/admincobraperu/www.cobraperu.com/wwwroot/Calidda/Content/foto/foto/";
                var fotos = HttpContext.Current.Request.Files;
                var json = HttpContext.Current.Request.Form["model"];
                var suministro = HttpContext.Current.Request.Form["suministro"];

                Mensaje verificar = MigrationDA.verificarCorte(suministro);

                if (verificar.codigo == 0)
                {
                    Registro p = JsonConvert.DeserializeObject<Registro>(json);

                    Mensaje mensaje = MigrationDA.saveRegistroRx(p);

                    if (mensaje != null)
                    {
                        for (int i = 0; i < fotos.Count; i++)
                        {
                            string fileName = Path.GetFileName(fotos[i].FileName);
                            fotos[i].SaveAs(path + fileName);
                        }
                    }
                    else
                    {
                        mensaje = new Mensaje();
                        mensaje.mensaje = "Registro repetido";
                    }

                    return Ok(mensaje);
                }
                else
                {
                    return Ok(verificar);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
