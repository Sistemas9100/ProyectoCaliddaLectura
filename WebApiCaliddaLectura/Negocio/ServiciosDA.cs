using Entidades;
using Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class ServiciosDA
    {
        private static string db = ConfigurationManager.ConnectionStrings["conexionDsige"].ConnectionString;

        public static Login GetOne(string user, string password)
        {
            try
            {
                Login item =null;
                using (SqlConnection cn = new SqlConnection(db))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand("MOVIL_Login", cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@User", user);
                        cmd.Parameters.AddWithValue("@Pass", password);
                        SqlDataReader dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            item = new Login();
                            item.iD_Operario = Convert.ToInt32(dr["ID_Operario"]);
                            item.operario_Login = dr["Operario_Login"].ToString();
                            item.operario_Contrasenia = dr["Operario_Contrasenia"].ToString();
                            item.operario_Nombre = dr["Operario_Nombre"].ToString();
                            item.operario_EnvioEn_Linea = Convert.ToInt32(dr["Operario_EnvioEn_Linea"]);
                            item.tipoUsuario = dr["TipoUsuario"].ToString();
                            item.estado = dr["estado"].ToString();
                        }
                        dr.Close();
                    }
                }
                return item;

            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public static List<Servicio> GetServicio()
        {
            try
            {
                List<Servicio> servicio = new List<Servicio>();
                using (SqlConnection cn = new SqlConnection(db))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand("MOVIL_Servicios", cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        DataTable dt_detalle = new DataTable();
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dt_detalle);
                            foreach (DataRow dr in dt_detalle.Rows)
                            {
                                Servicio rp = new Servicio();
                                rp.id_servicio = Convert.ToInt32(dr["id_servicio"]);
                                rp.estado = Convert.ToInt32(dr["estado"]);
                                rp.nombre_servicio = dr["nombre_servicio"].ToString();
                                servicio.Add(rp);
                            }
                        }
                    }
                }
                return servicio;

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static Mensaje sendEstado_Movil(int id_operario, int gpsactivo,
            int estadobateria, string fechahoraandroid, int modoavion, int plandatos)
        {
            using (SqlConnection cn = new SqlConnection(db))
            {
                Mensaje m = new Mensaje();
                SqlCommand cmd = cn.CreateCommand();
                cmd.Connection.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "MOVIL_Estado_Celular";
                cmd.Parameters.AddWithValue("@id_operario", id_operario);
                cmd.Parameters.AddWithValue("@gpsactivo", gpsactivo);
                cmd.Parameters.AddWithValue("@estadobateria", estadobateria);
                cmd.Parameters.AddWithValue("@fechahoraandroid", fechahoraandroid);
                cmd.Parameters.AddWithValue("@modoavion", modoavion);
                cmd.Parameters.AddWithValue("@plandatos", plandatos);
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();


                return m;
            }

        }

        public static Mensaje sendOperario_GPS(int id_operario, string GPS_Latitud,
            string GPS_Longitud, string fecha_GPD, string fecha_Android)
        {
            using (SqlConnection cn = new SqlConnection(db))
            {
                Mensaje m = new Mensaje();
                SqlCommand cmd = cn.CreateCommand();
                cmd.Connection.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "MOVIL_Registro_OperarioGPS";
                cmd.Parameters.AddWithValue("@id_operario", id_operario);
                cmd.Parameters.AddWithValue("@GPS_Latitud", GPS_Latitud);
                cmd.Parameters.AddWithValue("@GPS_Longitud", GPS_Longitud);
                cmd.Parameters.AddWithValue("@fecha_GPD", fecha_GPD);
                cmd.Parameters.AddWithValue("@fecha_Android", fecha_Android);
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();


                return m;
            }

        }


        // nuevo irvin

        public static Mensaje saveEstadoMovil(EstadoMovil e)
        {
            try
            {
                Mensaje m = new Mensaje();
                using (SqlConnection cn = new SqlConnection(db))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.CommandText = "USP_SAVE_ESTADOCELULAR";
                    cmd.Parameters.Add("@operarioId", SqlDbType.Int).Value = e.operarioId;
                    cmd.Parameters.Add("@gpsActivo", SqlDbType.Bit).Value = e.gpsActivo;
                    cmd.Parameters.Add("@estadoBateria", SqlDbType.Int).Value = e.estadoBateria;
                    cmd.Parameters.Add("@fecha", SqlDbType.VarChar).Value = e.fecha;
                    cmd.Parameters.Add("@modoAvion", SqlDbType.Int).Value = e.modoAvion;
                    cmd.Parameters.Add("@planDatos", SqlDbType.Bit).Value = e.planDatos;
                    cmd.ExecuteNonQuery();
                    m.mensaje = "Enviado";
                    cn.Close();
                }
                return m;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Mensaje saveOperarioGps(EstadoOperario e)
        {
            try
            {
                Mensaje m = new Mensaje();
                using (SqlConnection cn = new SqlConnection(db))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "USP_SAVE_GPS";
                    cmd.Parameters.Add("@operarioId", SqlDbType.Int).Value = e.operarioId;
                    cmd.Parameters.Add("@latitud", SqlDbType.VarChar).Value = e.latitud;
                    cmd.Parameters.Add("@longitud", SqlDbType.VarChar).Value = e.longitud;
                    cmd.Parameters.Add("@fechaGPD", SqlDbType.VarChar).Value = e.fechaGPD;
                    cmd.Parameters.Add("@fecha", SqlDbType.VarChar).Value = e.fecha;
                    cmd.ExecuteNonQuery();
                    m.mensaje = "Enviado";
                    cn.Close();
                }
                return m;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Mensaje saveListEstadoMovil(List<EstadoMovil> moviles)
        {

            try
            {
                Mensaje m = new Mensaje();
                using (SqlConnection cn = new SqlConnection(db))
                {
                    cn.Open();

                    foreach (var e in moviles)
                    {
                        SqlCommand cmd = cn.CreateCommand();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.CommandText = "USP_SAVE_ESTADOCELULAR";
                        cmd.Parameters.Add("@operarioId", SqlDbType.Int).Value = e.operarioId;
                        cmd.Parameters.Add("@gpsActivo", SqlDbType.Bit).Value = e.gpsActivo;
                        cmd.Parameters.Add("@estadoBateria", SqlDbType.Int).Value = e.estadoBateria;
                        cmd.Parameters.Add("@fecha", SqlDbType.VarChar).Value = e.fecha;
                        cmd.Parameters.Add("@modoAvion", SqlDbType.Int).Value = e.modoAvion;
                        cmd.Parameters.Add("@planDatos", SqlDbType.Bit).Value = e.planDatos;
                        cmd.ExecuteNonQuery();
                    }
                    m.mensaje = "Enviado";
                    cn.Close();
                }
                return m;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static Mensaje saveListOperarioGps(List<EstadoOperario> operarios)
        {
            try
            {
                Mensaje m = new Mensaje();
                using (SqlConnection cn = new SqlConnection(db))
                {
                    cn.Open();
                    foreach (var e in operarios)
                    {
                        SqlCommand cmd = cn.CreateCommand();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "USP_SAVE_GPS";
                        cmd.Parameters.Add("@operarioId", SqlDbType.Int).Value = e.operarioId;
                        cmd.Parameters.Add("@latitud", SqlDbType.VarChar).Value = e.latitud;
                        cmd.Parameters.Add("@longitud", SqlDbType.VarChar).Value = e.longitud;
                        cmd.Parameters.Add("@fechaGPD", SqlDbType.VarChar).Value = e.fechaGPD;
                        cmd.Parameters.Add("@fecha", SqlDbType.VarChar).Value = e.fecha;
                        cmd.ExecuteNonQuery();
                    }
                    m.mensaje = "Enviado";
                    cn.Close();
                }
                return m;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public static Sincronizar sincronizar(int operarioId)
        {
            try
            {
                Sincronizar sincronizar = new Sincronizar();
                using (SqlConnection cn = new SqlConnection(db))
                {
                    cn.Open();

                    sincronizar.sincronizarId = 1;

                    SqlCommand cmd = cn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.CommandText = "USP_LIST_SUMINISTRO_CORTES";
                    cmd.Parameters.Add("@ID_Operario", SqlDbType.Int).Value = operarioId;
                    cmd.Parameters.Add("@Tipo", SqlDbType.VarChar).Value = "3";
                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.HasRows)
                    {
                        List<Suministro> suministroCorte = new List<Suministro>();
                        int i = 1;
                        while (dr.Read())
                        {
                            suministroCorte.Add(new Suministro()
                            {
                                iD_Suministro = Convert.ToInt32(dr["ID_Suministro"]),
                                suministro_Numero = dr["Suministro_Numero"].ToString(),
                                suministro_Medidor = dr["Suministro_Medidor"].ToString(),
                                suministro_Cliente = dr["Suministro_Cliente"].ToString(),
                                suministro_Direccion = dr["Suministro_Direccion"].ToString(),
                                suministro_UnidadLectura = dr["Suministro_UnidadLectura"].ToString(),
                                suministro_TipoProceso = dr["Suministro_TipoProceso"].ToString(),
                                suministro_LecturaMinima = dr["Suministro_LecturaMinima"].ToString(),
                                suministro_LecturaMaxima = dr["Suministro_LecturaMaxima"].ToString(),
                                suministro_Fecha_Reg_Movil = dr["Suministro_Fecha_Reg_Movil"].ToString(),
                                suministro_UltimoMes = dr["Suministro_UltimoMes"].ToString(),
                                suministro_NoCortar = Convert.ToInt32(dr["Suministro_NoCortar"]),
                                estado = Convert.ToInt32(dr["Estado"]),
                                suministroOperario_Orden = Convert.ToInt32(dr["SuministroOperario_Orden"]),
                                orden = i++,
                                activo = 1
                            });
                        }
                        sincronizar.suministrosCortes = suministroCorte;
                    }
                    else sincronizar.suministrosCortes = null;


                    SqlCommand cmdR = cn.CreateCommand();
                    cmdR.CommandType = CommandType.StoredProcedure;
                    cmdR.CommandTimeout = 0;
                    cmdR.CommandText = "USP_LIST_SUMINISTRO_CORTES";
                    cmdR.Parameters.Add("@ID_Operario", SqlDbType.Int).Value = operarioId;
                    cmdR.Parameters.Add("@Tipo", SqlDbType.VarChar).Value = "4";
                    SqlDataReader drR = cmdR.ExecuteReader();

                    if (dr.HasRows)
                    {
                        List<Suministro> suministroReconexiones = new List<Suministro>();
                        int y = 1;
                        while (dr.Read())
                        {
                            suministroReconexiones.Add(new Suministro()
                            {
                                iD_Suministro = Convert.ToInt32(dr["ID_Suministro"]),
                                suministro_Numero = dr["Suministro_Numero"].ToString(),
                                suministro_Medidor = dr["Suministro_Medidor"].ToString(),
                                suministro_Cliente = dr["Suministro_Cliente"].ToString(),
                                suministro_Direccion = dr["Suministro_Direccion"].ToString(),
                                suministro_UnidadLectura = dr["Suministro_UnidadLectura"].ToString(),
                                suministro_TipoProceso = dr["Suministro_TipoProceso"].ToString(),
                                suministro_LecturaMinima = dr["Suministro_LecturaMinima"].ToString(),
                                suministro_LecturaMaxima = dr["Suministro_LecturaMaxima"].ToString(),
                                suministro_Fecha_Reg_Movil = dr["Suministro_Fecha_Reg_Movil"].ToString(),
                                suministro_UltimoMes = dr["Suministro_UltimoMes"].ToString(),
                                suministro_NoCortar = Convert.ToInt32(dr["Suministro_NoCortar"]),
                                estado = Convert.ToInt32(dr["Estado"]),
                                suministroOperario_Orden = Convert.ToInt32(dr["SuministroOperario_Orden"]),
                                orden = y++,
                                activo = 1
                            });
                        }
                        sincronizar.suministroReconexion = suministroReconexiones;

                    }
                    else sincronizar.suministroReconexion = null;



                    cn.Close();

                }
                return sincronizar;
            }
            catch (Exception e)
            {
                throw e;
            }
        }





        /*
               public static string saveEstadoMovil(List<EstadoMovil> EstadoMovil)
               {
                   try
                   {
                       using (SqlConnection con = new SqlConnection(db))
                       {
                           con.Open();
                           foreach (var a in EstadoMovil)
                           {

                               using (SqlCommand cmd = new SqlCommand("MOVIL_Estado_Celular", con))
                               {
                                   cmd.CommandTimeout = 0;
                                   cmd.CommandType = CommandType.StoredProcedure;
                                   cmd.Parameters.AddWithValue("@id_operario", a.id_operario);
                                   cmd.Parameters.AddWithValue("@gpsactivo", a.gpsactivo);
                                   cmd.Parameters.AddWithValue("@estadobateria", a.estadobateria);
                                   cmd.Parameters.AddWithValue("@fechahoraandroid", a.fechahoraandroid);
                                   cmd.Parameters.AddWithValue("@modoavion", a.modoavion);
                                   cmd.Parameters.AddWithValue("@plandatos", a.plandatos);
                                   SqlDataReader dr = cmd.ExecuteReader();
                                   dr.Close();
                               }


                           }
                           con.Close();
                       }
                       return "Datos Enviados";
                   }
                   catch (Exception ex)
                   {
                       throw;

                   }


               }

               public static string saveEstadoOperario(List<EstadoOperario> EstadoOperario)
               {
                   try
                   {
                       using (SqlConnection con = new SqlConnection(db))
                       {
                           con.Open();
                           foreach (var a in EstadoOperario)
                           {

                               using (SqlCommand cmd = new SqlCommand("MOVIL_Registro_OperarioGPS", con))
                               {
                                   cmd.CommandTimeout = 0;
                                   cmd.CommandType = CommandType.StoredProcedure;
                                   cmd.Parameters.AddWithValue("@id_operario", a.id_operario);
                                   cmd.Parameters.AddWithValue("@GPS_Latitud", a.GPS_Latitud);
                                   cmd.Parameters.AddWithValue("@GPS_Longitud", a.GPS_Longitud);
                                   cmd.Parameters.AddWithValue("@fecha_GPD", a.fecha_GPD);
                                   cmd.Parameters.AddWithValue("@fecha_Android", a.fecha_Android);
                                   SqlDataReader dr = cmd.ExecuteReader();
                                   dr.Close();
                               }


                           }
                           con.Close();
                       }
                       return "Datos Enviados";
                   }
                   catch (Exception ex)
                   {
                       throw;

                   }


               }
               */

    }
}
