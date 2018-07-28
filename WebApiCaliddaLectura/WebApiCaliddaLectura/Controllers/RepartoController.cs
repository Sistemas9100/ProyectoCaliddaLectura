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
    public class RepartoController : ApiController
    {
        [HttpPost]
        [Route("api/SaveRepartoRegistro")]
        public HttpResponseMessage SaveRepartoRegistro(List<SendReparto> reparto)
        {
            HttpResponseMessage response;
            try
            {
                String m = RepartoDA.saveRepartoRegistro(reparto);
                response = this.Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(JsonConvert.SerializeObject(new { mensaje = m }), Encoding.UTF8, "application/json");
            }
            catch (Exception ex)
            {
                response = this.Request.CreateResponse(HttpStatusCode.InternalServerError);
                response.Content = new StringContent(JsonConvert.SerializeObject(new { mensaje = ex.Message }), Encoding.UTF8, "application/json"); ;
            }

            return response;

        }

        [HttpPost] // This is from System.Web.Http, and not from System.Web.Mvc
        [Route("api/SaveRepartoPhoto")]
        public HttpResponseMessage Upload()
        {
            try
            {
                if (!Request.Content.IsMimeMultipartContent())
                {
                    this.Request.CreateResponse(HttpStatusCode.UnsupportedMediaType);
                }
                string path = "C:/HostingSpaces/admincobraperu/www.cobraperu.com/wwwroot/Calidda/Content/foto/foto/";

                HttpFileCollection files = HttpContext.Current.Request.Files;
                string fileName = Path.GetFileName(files[0].FileName);
                files[0].SaveAs(path + fileName);

                return this.Request.CreateResponse(HttpStatusCode.OK, new
                {
                    mensaje = "Mensaje enviado"
                });
            }
            catch (Exception ex)
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, new
                {
                    mensaje = ex.Message
                });
            }


        }

        [HttpGet]
        [Route("api/ListaDeReparto")]
        public HttpResponseMessage ListaDeReparto(int operarioId)
        {

            HttpResponseMessage response;
            try
            {
                response = this.Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(JsonConvert.SerializeObject(new
                {
                    RepartoLectura = RepartoDA.GetReparto(operarioId).Select(x => new
                    {
                        x.id_Reparto,
                        x.id_Operario_Reparto,
                        x.Suministro_Medidor_reparto,
                        x.Suministro_Numero_reparto,
                        x.foto_Reparto,
                        x.Direccion_Reparto,
                        x.Cod_Orden_Reparto,
                        x.Cod_Actividad_Reparto,
                        x.Cliente_Reparto,
                        x.CodigoBarra,
                        x.estado,
                        x.activo
                    })
                }), Encoding.UTF8, "application/json");
            }
            catch (Exception ex)
            {
                response = this.Request.CreateResponse(HttpStatusCode.InternalServerError);
                response.Content = new StringContent(JsonConvert.SerializeObject(ex.Message), Encoding.UTF8, "application/json"); ;
            }

            return response;

        }
    }
}
