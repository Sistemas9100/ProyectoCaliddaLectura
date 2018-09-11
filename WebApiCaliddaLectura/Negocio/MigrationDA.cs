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
    public class MigrationDA
    {
        private static string db = ConfigurationManager.ConnectionStrings["conexionDsige"].ConnectionString;

        public static Mensaje verificarCorte(string suministro)
        {
            try
            {
                Mensaje mensaje = null;
                using (SqlConnection cn = new SqlConnection(db))
                {
                    cn.Open();

                    using (SqlCommand cmd = new SqlCommand("USP_VERIFICAR_CORTE", cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Suministro", SqlDbType.VarChar).Value = suministro;
                        SqlDataReader rd = cmd.ExecuteReader();
                        if (rd.HasRows)
                        {
                            while (rd.Read())
                            {
                                mensaje = new Mensaje()
                                {
                                    codigo = rd.GetInt32(0),
                                    mensaje = "Enviado"
                                };
                            }
                        }
                        else
                        {
                            mensaje = null;
                        }
                        rd.Close();
                    }
                }
                return mensaje;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static Migracion getMigracion(int operarioId)
        {
            try
            {
                Migracion migracion = new Migracion();

                migracion.migrationId = 1;

                using (SqlConnection con = new SqlConnection(db))
                {
                    con.Open();

                    // Servicios

                    // Servicios

                    SqlCommand cmdServicio = con.CreateCommand();
                    cmdServicio.CommandTimeout = 0;
                    cmdServicio.CommandType = System.Data.CommandType.StoredProcedure;
                    cmdServicio.CommandText = "USP_SERVICIOS";
                    SqlDataReader drServicio = cmdServicio.ExecuteReader();
                    if (drServicio.HasRows)
                    {
                        List<Servicio> servicio = new List<Servicio>();
                        while (drServicio.Read())
                        {
                            servicio.Add(new Servicio()
                            {
                                id_servicio = drServicio.GetInt32(0),
                                nombre_servicio = drServicio.GetString(1),
                                estado = drServicio.GetInt32(2)
                            });
                        }
                        migracion.servicios = servicio;
                    }
                    else migracion.servicios = null;


                    // Parametro

                    SqlCommand cmdParametro = con.CreateCommand();
                    cmdParametro.CommandTimeout = 0;
                    cmdParametro.CommandType = System.Data.CommandType.StoredProcedure;
                    cmdParametro.CommandText = "USP_PARAMETROS";
                    SqlDataReader drParametro = cmdParametro.ExecuteReader();
                    if (drParametro.HasRows)
                    {
                        List<Parametro> parametro = new List<Parametro>();
                        while (drParametro.Read())
                        {
                            parametro.Add(new Parametro()
                            {
                                id_Configuracion = drParametro.GetInt32(0),
                                nombre_parametro = drParametro.GetString(1),
                                valor = drParametro.GetInt32(2)
                            });
                        }
                        migracion.parametros = parametro;

                    }
                    else migracion.parametros = null;


                    // Suministro


                    SqlCommand cmdSuministro = con.CreateCommand();
                    cmdSuministro.CommandTimeout = 0;
                    cmdSuministro.CommandType = System.Data.CommandType.StoredProcedure;
                    cmdSuministro.CommandText = "USP_LIST_SUMINISTRO";
                    cmdSuministro.Parameters.Add("@ID_Operario", SqlDbType.Int).Value = operarioId;
                    SqlDataReader drSuministro = cmdSuministro.ExecuteReader();
                    if (drSuministro.HasRows)
                    {
                        List<Suministro> suministro = new List<Suministro>();
                        int i = 1;
                        while (drSuministro.Read())
                        {
                            suministro.Add(new Suministro()
                            {
                                iD_Suministro = drSuministro.GetInt32(0),
                                suministro_Numero = drSuministro.GetString(1),
                                suministro_Medidor = drSuministro.GetString(2),
                                suministro_Cliente = drSuministro.GetString(3),
                                suministro_Direccion = drSuministro.GetString(4),
                                suministro_UnidadLectura = drSuministro.GetString(5),
                                suministro_TipoProceso = drSuministro.GetString(6),
                                suministro_LecturaMinima = drSuministro.GetString(7),
                                suministro_LecturaMaxima = drSuministro.GetString(8),
                                suministro_Fecha_Reg_Movil = drSuministro.GetDateTime(9).ToString("dd/MM/yyyy"),
                                suministro_UltimoMes = drSuministro.GetString(10),
                                consumoPromedio = drSuministro.GetDecimal(11),
                                lecturaAnterior = drSuministro.GetString(12),
                                suministro_Instalacion = drSuministro.GetString(13),
                                valida1 = drSuministro.GetInt32(14),
                                valida2 = drSuministro.GetInt32(15),
                                valida3 = drSuministro.GetInt32(16),
                                valida4 = drSuministro.GetInt32(17),
                                valida5 = drSuministro.GetInt32(18),
                                valida6 = drSuministro.GetInt32(19),
                                tipoCliente = drSuministro.GetInt32(20),
                                estado = drSuministro.GetInt32(21),
                                suministroOperario_Orden = drSuministro.GetInt32(22),
                                orden = i++,
                                activo = 1
                            });
                        }
                        migracion.suministroLecturas = suministro;

                    }
                    else migracion.suministroLecturas = null;


                    // SuministroCorte 

                    SqlCommand cmdCortes = con.CreateCommand();
                    cmdCortes.CommandTimeout = 0;
                    cmdCortes.CommandType = System.Data.CommandType.StoredProcedure;
                    cmdCortes.CommandText = "USP_LIST_SUMINISTRO_CORTES";
                    cmdCortes.Parameters.Add("@ID_Operario", SqlDbType.Int).Value = operarioId;
                    cmdCortes.Parameters.Add("@Tipo", SqlDbType.VarChar).Value = "3";
                    SqlDataReader drCortes = cmdCortes.ExecuteReader();
                    if (drCortes.HasRows)
                    {
                        List<Suministro> suministroCorte = new List<Suministro>();
                        int y = 1;
                        while (drCortes.Read())
                        {
                            suministroCorte.Add(new Suministro()
                            {
                                iD_Suministro = drCortes.GetInt32(0),
                                suministro_Numero = drCortes.GetString(1),
                                suministro_Medidor = drCortes.GetString(2),
                                suministro_Cliente = drCortes.GetString(3),
                                suministro_Direccion = drCortes.GetString(4),
                                suministro_UnidadLectura = drCortes.GetString(5),
                                suministro_TipoProceso = drCortes.GetString(6),
                                suministro_LecturaMinima = drCortes.GetString(7),
                                suministro_LecturaMaxima = drCortes.GetString(8),
                                suministro_Fecha_Reg_Movil = drCortes.GetDateTime(9).ToString("dd/MM/yyyy"),
                                suministro_UltimoMes = drCortes.GetString(10),
                                suministro_NoCortar = drCortes.GetInt32(11),
                                estado = drCortes.GetInt32(12),
                                suministroOperario_Orden = drCortes.GetInt32(13),
                                orden = y++,
                                activo = 1
                            });
                        }
                        migracion.suministroCortes = suministroCorte;

                    }
                    else migracion.suministroCortes = null;

                    // SuministroReconexiones

                    SqlCommand cmdReconexiones = con.CreateCommand();
                    cmdReconexiones.CommandTimeout = 0;
                    cmdReconexiones.CommandType = System.Data.CommandType.StoredProcedure;
                    cmdReconexiones.CommandText = "USP_LIST_SUMINISTRO_CORTES";
                    cmdReconexiones.Parameters.Add("@ID_Operario", SqlDbType.Int).Value = operarioId;
                    cmdReconexiones.Parameters.Add("@Tipo", SqlDbType.VarChar).Value = "4";

                    SqlDataReader drReconexiones = cmdReconexiones.ExecuteReader();
                    if (drReconexiones.HasRows)
                    {
                        List<Suministro> suministroReconexiones = new List<Suministro>();
                        int z = 1;
                        while (drReconexiones.Read())
                        {
                            suministroReconexiones.Add(new Suministro()
                            {
                                iD_Suministro = drReconexiones.GetInt32(0),
                                suministro_Numero = drReconexiones.GetString(1),
                                suministro_Medidor = drReconexiones.GetString(2),
                                suministro_Cliente = drReconexiones.GetString(3),
                                suministro_Direccion = drReconexiones.GetString(4),
                                suministro_UnidadLectura = drReconexiones.GetString(5),
                                suministro_TipoProceso = drReconexiones.GetString(6),
                                suministro_LecturaMinima = drReconexiones.GetString(7),
                                suministro_LecturaMaxima = drReconexiones.GetString(8),
                                suministro_Fecha_Reg_Movil = drReconexiones.GetDateTime(9).ToString("dd/MM/yyyy"),
                                suministro_UltimoMes = drReconexiones.GetString(10),
                                suministro_NoCortar = drReconexiones.GetInt32(11),
                                estado = drReconexiones.GetInt32(12),
                                suministroOperario_Orden = drReconexiones.GetInt32(13),
                                orden = z++,
                                activo = 1
                            });
                        }
                        migracion.suministroReconexiones = suministroReconexiones;

                    }
                    else migracion.suministroReconexiones = null;


                    // Tipo Lectura

                    SqlCommand cmdTipo = con.CreateCommand();
                    cmdTipo.CommandTimeout = 0;
                    cmdTipo.CommandType = System.Data.CommandType.StoredProcedure;
                    cmdTipo.CommandText = "USP_LIST_TIPO_LECTURA";
                    SqlDataReader drTipo = cmdTipo.ExecuteReader();
                    if (drTipo.HasRows)
                    {
                        List<TipoLectura> tipoLectura = new List<TipoLectura>();
                        while (drTipo.Read())
                        {
                            tipoLectura.Add(new TipoLectura()
                            {
                                iD_TipoLectura = drTipo.GetInt32(0),
                                tipoLectura_Descripcion = drTipo.GetString(1),
                                tipoLectura_Abreviatura = drTipo.GetString(2),
                                tipoLectura_Estado = drTipo.GetString(3)
                            });
                        }

                        migracion.tipoLecturas = tipoLectura;

                    }
                    else migracion.tipoLecturas = null;


                    //Detalle Grupo

                    SqlCommand cmdGrupo = con.CreateCommand();
                    cmdGrupo.CommandTimeout = 0;
                    cmdGrupo.CommandType = System.Data.CommandType.StoredProcedure;
                    cmdGrupo.CommandText = "USP_LIST_DETALLE_GRUPO";
                    SqlDataReader rdGrupo = cmdGrupo.ExecuteReader();
                    if (rdGrupo.HasRows)
                    {
                        List<DetalleGrupo> detalleGrupo = new List<DetalleGrupo>();
                        int j = 1;
                        while (rdGrupo.Read())
                        {
                            detalleGrupo.Add(new DetalleGrupo()
                            {
                                id = j++,
                                iD_DetalleGrupo = rdGrupo.GetInt32(0),
                                grupo = rdGrupo.GetString(1),
                                codigo = rdGrupo.GetString(2),
                                descripcion = rdGrupo.GetString(3),
                                abreviatura = rdGrupo.GetString(4),
                                estado = rdGrupo.GetString(5),
                                descripcionGrupo = rdGrupo.GetString(6),
                                pideFoto = rdGrupo.GetString(7),
                                noPideFoto = rdGrupo.GetString(8),
                                pideLectura = rdGrupo.GetString(9),
                                id_Servicio = rdGrupo.GetInt32(10)
                            });
                        }
                        migracion.detalleGrupos = detalleGrupo;
                    }
                    else migracion.detalleGrupos = null;


                    // Reparto

                    SqlCommand cmdReparto = con.CreateCommand();
                    cmdReparto.CommandTimeout = 0;
                    cmdReparto.CommandType = System.Data.CommandType.StoredProcedure;
                    cmdReparto.CommandText = "MOVIL_Reparto_list";
                    cmdReparto.Parameters.AddWithValue("@id_operario_reparto", operarioId);
                    SqlDataReader rdReparto = cmdReparto.ExecuteReader();
                    if (rdReparto.HasRows)
                    {
                        List<Reparto> reparto = new List<Reparto>();
                        while (rdReparto.Read())
                        {
                            reparto.Add(new Reparto()
                            {
                                id_Reparto = Convert.ToInt32(rdReparto["id_Reparto"]),
                                id_Operario_Reparto = Convert.ToInt32(rdReparto["id_Operario_Reparto"]),
                                foto_Reparto = Convert.ToInt32(rdReparto["foto_Reparto"]),
                                estado = Convert.ToInt32(rdReparto["estado"]),
                                activo = Convert.ToInt32(rdReparto["activo"]),
                                Suministro_Medidor_reparto = rdReparto["Suministro_Medidor"].ToString(),
                                Suministro_Numero_reparto = rdReparto["Suministro_Numero"].ToString(),
                                Cod_Actividad_Reparto = rdReparto["Cod_Actividad_Reparto"].ToString(),
                                Cod_Orden_Reparto = rdReparto["Cod_Orden_Reparto"].ToString(),
                                Direccion_Reparto = rdReparto["Direccion_Reparto"].ToString(),
                                Cliente_Reparto = rdReparto["Cliente_Reparto"].ToString(),
                                CodigoBarra = rdReparto["CodigoBarra"].ToString()
                            });
                        }
                        migracion.repartoLectura = reparto;
                    }
                    else migracion.repartoLectura = null;

                    con.Close();
                }

                return migracion;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static Mensaje saveListRegistro(List<Registro> registroList)
        {
            try
            {
                int lastId;
                Mensaje m = new Mensaje();
                using (SqlConnection con = new SqlConnection(db))
                {
                    con.Open();

                    foreach (var r in registroList)
                    {
                        if (r.tipo == 1 || r.tipo == 2)
                        {
                            using (SqlCommand cmd = new SqlCommand("USP_SAVE_REGISTRO_LECTURA", con))
                            {
                                cmd.CommandTimeout = 0;
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add("@ID_Registro", SqlDbType.Int).Value = r.iD_Registro;
                                cmd.Parameters.Add("@ID_Operario", SqlDbType.Int).Value = r.iD_Operario;
                                cmd.Parameters.Add("@ID_Suministro", SqlDbType.Int).Value = r.iD_Suministro;
                                cmd.Parameters.Add("@ID_TipoLectura", SqlDbType.Int).Value = r.iD_TipoLectura;
                                cmd.Parameters.Add("@Registro_Fecha_SQLITE", SqlDbType.VarChar).Value = r.registro_Fecha_SQLITE;
                                cmd.Parameters.Add("@Registro_Latitud", SqlDbType.VarChar).Value = r.registro_Latitud;
                                cmd.Parameters.Add("@Registro_Longitud", SqlDbType.VarChar).Value = r.registro_Longitud;
                                cmd.Parameters.Add("@Registro_Lectura", SqlDbType.VarChar).Value = r.registro_Lectura;
                                cmd.Parameters.Add("@Registro_Confirmar_Lectura", SqlDbType.VarChar).Value = r.registro_Confirmar_Lectura;
                                cmd.Parameters.Add("@Registro_Observacion", SqlDbType.VarChar).Value = r.registro_Observacion;
                                cmd.Parameters.Add("@Grupo_Incidencia_Codigo", SqlDbType.VarChar).Value = r.grupo_Incidencia_Codigo;
                                cmd.Parameters.Add("@Registro_TieneFoto", SqlDbType.VarChar).Value = r.registro_TieneFoto;
                                cmd.Parameters.Add("@Registro_TipoProceso", SqlDbType.VarChar).Value = r.registro_TipoProceso;
                                cmd.Parameters.Add("@Fecha_Sincronizacion_Android", SqlDbType.VarChar).Value = r.fecha_Sincronizacion_Android;
                                cmd.Parameters.Add("@Registro_Constancia", SqlDbType.VarChar).Value = r.registro_Constancia;
                                cmd.Parameters.Add("@Registro_Desplaza", SqlDbType.VarChar).Value = r.registro_Desplaza;
                                cmd.Parameters.Add("@Suministro_Numero", SqlDbType.Int).Value = r.suministro_Numero;
                                SqlDataReader dr = cmd.ExecuteReader();
                                if (dr.HasRows)
                                {
                                    while (dr.Read())
                                    {
                                        lastId = dr.GetInt32(0);
                                        if (lastId != 0)
                                        {
                                            foreach (var itemS in r.photos)
                                            {
                                                SqlCommand cmds = con.CreateCommand();
                                                cmds.CommandType = System.Data.CommandType.StoredProcedure;
                                                cmds.CommandText = "USP_SAVE_REGISTRO_PHOTO_LECTURA";
                                                cmds.Parameters.Add("@ID_Registro", SqlDbType.Int).Value = lastId;
                                                cmds.Parameters.Add("@RutaFoto", SqlDbType.VarChar).Value = itemS.rutaFoto;
                                                cmds.ExecuteNonQuery();
                                            }
                                        }

                                        //Nota : No cambiar este mensaje , se valida con el android...arigato <3
                                        m.mensaje = "Datos Enviados";
                                    }
                                }
                                else
                                {
                                    m = null;
                                    return m;
                                }
                            }

                        }
                        else if (r.tipo == 3 || r.tipo == 4)
                        {
                            using (SqlCommand cmd = new SqlCommand("USP_SAVE_REGISTRO_CORTES", con))
                            {
                                cmd.CommandTimeout = 0;
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add("@ID_Registro", SqlDbType.Int).Value = r.iD_Registro;
                                cmd.Parameters.Add("@ID_Operario", SqlDbType.Int).Value = r.iD_Operario;
                                cmd.Parameters.Add("@ID_Suministro", SqlDbType.Int).Value = r.iD_Suministro;
                                cmd.Parameters.Add("@ID_TipoLectura", SqlDbType.Int).Value = r.iD_TipoLectura;
                                cmd.Parameters.Add("@Registro_Fecha_SQLITE", SqlDbType.VarChar).Value = r.registro_Fecha_SQLITE;
                                cmd.Parameters.Add("@Registro_Latitud", SqlDbType.VarChar).Value = r.registro_Latitud;
                                cmd.Parameters.Add("@Registro_Longitud", SqlDbType.VarChar).Value = r.registro_Longitud;
                                cmd.Parameters.Add("@Registro_Lectura", SqlDbType.VarChar).Value = r.registro_Lectura;
                                cmd.Parameters.Add("@Registro_Confirmar_Lectura", SqlDbType.VarChar).Value = r.registro_Confirmar_Lectura;
                                cmd.Parameters.Add("@Registro_Observacion", SqlDbType.VarChar).Value = r.registro_Observacion;
                                cmd.Parameters.Add("@Grupo_Incidencia_Codigo", SqlDbType.VarChar).Value = r.grupo_Incidencia_Codigo;
                                cmd.Parameters.Add("@Registro_TieneFoto", SqlDbType.VarChar).Value = r.registro_TieneFoto;
                                cmd.Parameters.Add("@Registro_TipoProceso", SqlDbType.VarChar).Value = r.registro_TipoProceso;
                                cmd.Parameters.Add("@Fecha_Sincronizacion_Android", SqlDbType.VarChar).Value = r.fecha_Sincronizacion_Android;
                                cmd.Parameters.Add("@Registro_Constancia", SqlDbType.VarChar).Value = r.registro_Constancia;
                                cmd.Parameters.Add("@Registro_Desplaza", SqlDbType.VarChar).Value = r.registro_Desplaza;
                                cmd.Parameters.Add("@Codigo_Resultado", SqlDbType.VarChar).Value = r.codigo_Resultado;
                                cmd.Parameters.Add("@horaActa", SqlDbType.VarChar).Value = r.horaActa == null ? "" : r.horaActa;
                                cmd.Parameters.Add("@Suministro_Numero", SqlDbType.Int).Value = r.suministro_Numero;
                                SqlDataReader dr = cmd.ExecuteReader();
                                if (dr.HasRows)
                                {
                                    while (dr.Read())
                                    {
                                        lastId = dr.GetInt32(0);
                                        if (lastId != 0)
                                        {
                                            foreach (var itemS in r.photos)
                                            {
                                                SqlCommand cmds = con.CreateCommand();
                                                cmds.CommandType = System.Data.CommandType.StoredProcedure;
                                                cmds.CommandText = "USP_SAVE_REGISTRO_PHOTO";
                                                cmds.Parameters.Add("@ID_Registro", SqlDbType.Int).Value = lastId;
                                                cmds.Parameters.Add("@RutaFoto", SqlDbType.VarChar).Value = itemS.rutaFoto;
                                                cmds.ExecuteNonQuery();
                                            }

                                        }
                                        //Nota : No cambiar este mensaje , se valida con el android...arigato <3                                     
                                        m.mensaje = "Datos Enviados";
                                    }
                                }
                                else
                                {
                                    m = null;
                                    return m;
                                }
                            }

                        }
                        else if (r.tipo == 5)
                        {
                            using (SqlCommand cmd = new SqlCommand("MOVIL_Reparto_save", con))
                            {
                                cmd.CommandTimeout = 0;
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@id_operario_reparto", r.iD_Operario);
                                cmd.Parameters.AddWithValue("@ID_Registro", 1);
                                cmd.Parameters.AddWithValue("@id_reparto", r.iD_Suministro);
                                cmd.Parameters.AddWithValue("@registro_fecha_sqlite", r.registro_Fecha_SQLITE);
                                cmd.Parameters.AddWithValue("@registro_latitud", r.registro_Latitud);
                                cmd.Parameters.AddWithValue("@registro_longitud", r.registro_Longitud);
                                cmd.Parameters.AddWithValue("@id_observacion", r.registro_Observacion);
                                SqlDataReader dr = cmd.ExecuteReader();
                                if (dr.HasRows)
                                {
                                    while (dr.Read())
                                    {
                                        lastId = dr.GetInt32(0);
                                        if (lastId != 0)
                                        {
                                            foreach (var item in r.photos)
                                            {
                                                using (SqlConnection cn = new SqlConnection(db))
                                                {
                                                    SqlCommand cmd1 = cn.CreateCommand();
                                                    cmd1.Connection.Open();
                                                    cmd1.CommandType = CommandType.StoredProcedure;
                                                    cmd1.CommandText = "MOVIL_Reparto_save_foto";
                                                    cmd1.Parameters.AddWithValue("@ID_Registro", lastId);
                                                    cmd1.Parameters.AddWithValue("@RutaFoto", item.rutaFoto);
                                                    cmd1.ExecuteNonQuery();
                                                    cmd1.Connection.Close();
                                                }
                                            }
                                        }
                                        m.mensaje = "Datos Enviados";

                                    }
                                }
                                else
                                {
                                    m = null;
                                    return m;
                                }
                            }

                        }
                        else if (r.tipo == 6)
                        {
                            using (SqlCommand cmd = new SqlCommand("USP_SAVE_SUMINISTRO_ENCONTRADO", con))
                            {
                                cmd.CommandTimeout = 0;
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add("@ID_Registro", SqlDbType.Int).Value = r.iD_Registro;
                                cmd.Parameters.Add("@id_operario", SqlDbType.Int).Value = r.iD_Operario;
                                cmd.Parameters.Add("@suministro_medidor", SqlDbType.VarChar).Value = r.iD_Suministro;
                                cmd.Parameters.Add("@suministro_contrato", SqlDbType.VarChar).Value = r.registro_Constancia;
                                cmd.Parameters.Add("@suministro_cliente", SqlDbType.VarChar).Value = r.suministroCliente;
                                cmd.Parameters.Add("@suministro_direccion", SqlDbType.VarChar).Value = r.suministroDireccion;
                                cmd.Parameters.Add("@suministro_observacion", SqlDbType.VarChar).Value = r.registro_Observacion;
                                cmd.Parameters.Add("@fecha_movil", SqlDbType.VarChar).Value = r.registro_Fecha_SQLITE;

                                SqlDataReader dr = cmd.ExecuteReader();
                                if (dr.HasRows)
                                {
                                    while (dr.Read())
                                    {
                                        lastId = dr.GetInt32(0);
                                        if (lastId != 0)
                                        {
                                            foreach (var itemS in r.photos)
                                            {
                                                SqlCommand cmds = con.CreateCommand();
                                                cmds.CommandType = System.Data.CommandType.StoredProcedure;
                                                cmds.CommandText = "USP_SAVE_REGISTRO_PHOTO_LECTURA";
                                                cmds.Parameters.Add("@ID_Registro", SqlDbType.Int).Value = lastId;
                                                cmds.Parameters.Add("@RutaFoto", SqlDbType.VarChar).Value = itemS.rutaFoto;
                                                cmds.ExecuteNonQuery();
                                            }
                                        }
                                        m.mensaje = "Datos Enviados";
                                    }
                                }
                                else
                                {
                                    m = null;
                                    return m;
                                }
                            }
                        }
                        else if (r.tipo == 8)
                        {
                            using (SqlCommand cmd = new SqlCommand("USP_SAVE_ZONA_PELIGROSA", con))
                            {
                                cmd.CommandTimeout = 0;
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add("@ID_Registro", SqlDbType.Int).Value = r.iD_Registro;
                                cmd.Parameters.Add("@id_operario", SqlDbType.Int).Value = r.iD_Operario;
                                cmd.Parameters.Add("@latitud", SqlDbType.VarChar).Value = r.registro_Latitud;
                                cmd.Parameters.Add("@longitud", SqlDbType.VarChar).Value = r.registro_Longitud;
                                cmd.Parameters.Add("@suministro_observacion", SqlDbType.VarChar).Value = r.registro_Observacion;
                                cmd.Parameters.Add("@fecha_movil", SqlDbType.VarChar).Value = r.registro_Fecha_SQLITE;

                                SqlDataReader dr = cmd.ExecuteReader();
                                m = new Mensaje();
                                if (dr.HasRows)
                                {
                                    while (dr.Read())
                                    {
                                        lastId = dr.GetInt32(0);
                                        if (lastId != 0)
                                        {
                                            foreach (var itemS in r.photos)
                                            {
                                                SqlCommand cmds = con.CreateCommand();
                                                cmds.CommandType = System.Data.CommandType.StoredProcedure;
                                                cmds.CommandText = "USP_SAVE_REGISTRO_PHOTO_LECTURA";
                                                cmds.Parameters.Add("@ID_Registro", SqlDbType.Int).Value = lastId;
                                                cmds.Parameters.Add("@RutaFoto", SqlDbType.VarChar).Value = itemS.rutaFoto;
                                                cmds.ExecuteNonQuery();
                                            }
                                        }

                                        m.mensaje = "Datos Enviados";
                                    }
                                }
                                else
                                {
                                    return m;
                                }
                            }
                        }
                        else if (r.tipo == 9)
                        {
                            using (SqlCommand cmd = new SqlCommand("MOVIL_Reparto_selfi_save", con))
                            {
                                cmd.CommandTimeout = 0;
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@id_operario_reparto", r.iD_Operario);
                                cmd.Parameters.AddWithValue("@ID_Registro", 1);
                                cmd.Parameters.AddWithValue("@id_reparto", r.iD_Suministro);
                                cmd.Parameters.AddWithValue("@registro_fecha_sqlite", r.registro_Fecha_SQLITE);
                                cmd.Parameters.AddWithValue("@registro_latitud", r.registro_Latitud);
                                cmd.Parameters.AddWithValue("@registro_longitud", r.registro_Longitud);
                                cmd.Parameters.AddWithValue("@id_observacion", r.registro_Observacion);
                                SqlDataReader dr = cmd.ExecuteReader();
                                if (dr.HasRows)
                                {
                                    while (dr.Read())
                                    {
                                        lastId = dr.GetInt32(0);
                                        if (lastId != 0)
                                        {
                                            foreach (var item in r.photos)
                                            {
                                                using (SqlConnection cn = new SqlConnection(db))
                                                {
                                                    SqlCommand cmd1 = cn.CreateCommand();
                                                    cmd1.Connection.Open();
                                                    cmd1.CommandType = CommandType.StoredProcedure;
                                                    cmd1.CommandText = "MOVIL_Reparto_save_foto";
                                                    cmd1.Parameters.AddWithValue("@ID_Registro", lastId);
                                                    cmd1.Parameters.AddWithValue("@RutaFoto", item.rutaFoto);
                                                    cmd1.ExecuteNonQuery();
                                                    cmd1.Connection.Close();
                                                }
                                            }
                                        }
                                        m.mensaje = "Datos Enviados";

                                    }
                                }
                                else
                                {
                                    m = null;
                                    return m;
                                }
                            }
                        }
                    }

                    con.Close();
                }
                return m;
            }
            catch (Exception ex)
            {
                throw;

            }

        }

        public static Mensaje saveRegistro(Registro r)
        {
            try
            {
                int lastId;
                Mensaje m = null;
                using (SqlConnection con = new SqlConnection(db))
                {
                    con.Open();
                    if (r.tipo == 2 || r.tipo == 1)
                    {
                        using (SqlCommand cmd = new SqlCommand("USP_SAVE_REGISTRO_LECTURA", con))
                        {
                            cmd.CommandTimeout = 0;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@ID_Registro", SqlDbType.Int).Value = r.iD_Registro;
                            cmd.Parameters.Add("@ID_Operario", SqlDbType.Int).Value = r.iD_Operario;
                            cmd.Parameters.Add("@ID_Suministro", SqlDbType.Int).Value = r.iD_Suministro;
                            cmd.Parameters.Add("@ID_TipoLectura", SqlDbType.Int).Value = r.iD_TipoLectura;
                            cmd.Parameters.Add("@Registro_Fecha_SQLITE", SqlDbType.VarChar).Value = r.registro_Fecha_SQLITE;
                            cmd.Parameters.Add("@Registro_Latitud", SqlDbType.VarChar).Value = r.registro_Latitud;
                            cmd.Parameters.Add("@Registro_Longitud", SqlDbType.VarChar).Value = r.registro_Longitud;
                            cmd.Parameters.Add("@Registro_Lectura", SqlDbType.VarChar).Value = r.registro_Lectura;
                            cmd.Parameters.Add("@Registro_Confirmar_Lectura", SqlDbType.VarChar).Value = r.registro_Confirmar_Lectura;
                            cmd.Parameters.Add("@Registro_Observacion", SqlDbType.VarChar).Value = r.registro_Observacion;
                            cmd.Parameters.Add("@Grupo_Incidencia_Codigo", SqlDbType.VarChar).Value = r.grupo_Incidencia_Codigo;
                            cmd.Parameters.Add("@Registro_TieneFoto", SqlDbType.VarChar).Value = r.registro_TieneFoto;
                            cmd.Parameters.Add("@Registro_TipoProceso", SqlDbType.VarChar).Value = r.registro_TipoProceso;
                            cmd.Parameters.Add("@Fecha_Sincronizacion_Android", SqlDbType.VarChar).Value = r.fecha_Sincronizacion_Android;
                            cmd.Parameters.Add("@Registro_Constancia", SqlDbType.VarChar).Value = r.registro_Constancia;
                            cmd.Parameters.Add("@Registro_Desplaza", SqlDbType.VarChar).Value = r.registro_Desplaza;
                            cmd.Parameters.Add("@Suministro_Numero", SqlDbType.Int).Value = r.suministro_Numero;
                            SqlDataReader dr = cmd.ExecuteReader();
                            m = new Mensaje();
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    lastId = dr.GetInt32(0);
                                    if (lastId != 0)
                                    {
                                        foreach (var itemS in r.photos)
                                        {
                                            SqlCommand cmds = con.CreateCommand();
                                            cmds.CommandType = CommandType.StoredProcedure;
                                            cmds.CommandText = "USP_SAVE_REGISTRO_PHOTO_LECTURA";
                                            cmds.Parameters.Add("@ID_Registro", SqlDbType.Int).Value = lastId;
                                            cmds.Parameters.Add("@RutaFoto", SqlDbType.VarChar).Value = itemS.rutaFoto;
                                            cmds.ExecuteNonQuery();
                                        }
                                    }

                                    m.mensaje = "Datos Enviados";
                                }
                            }
                            else
                            {
                                return m;
                            }
                        }
                    }
                    else if (r.tipo == 3 || r.tipo == 4)
                    {
                        using (SqlCommand cmd = new SqlCommand("USP_SAVE_REGISTRO_CORTES", con))
                        {
                            cmd.CommandTimeout = 0;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@ID_Registro", SqlDbType.Int).Value = r.iD_Registro;
                            cmd.Parameters.Add("@ID_Operario", SqlDbType.Int).Value = r.iD_Operario;
                            cmd.Parameters.Add("@ID_Suministro", SqlDbType.Int).Value = r.iD_Suministro;
                            cmd.Parameters.Add("@ID_TipoLectura", SqlDbType.Int).Value = r.iD_TipoLectura;
                            cmd.Parameters.Add("@Registro_Fecha_SQLITE", SqlDbType.VarChar).Value = r.registro_Fecha_SQLITE;
                            cmd.Parameters.Add("@Registro_Latitud", SqlDbType.VarChar).Value = r.registro_Latitud;
                            cmd.Parameters.Add("@Registro_Longitud", SqlDbType.VarChar).Value = r.registro_Longitud;
                            cmd.Parameters.Add("@Registro_Lectura", SqlDbType.VarChar).Value = r.registro_Lectura;
                            cmd.Parameters.Add("@Registro_Confirmar_Lectura", SqlDbType.VarChar).Value = r.registro_Confirmar_Lectura;
                            cmd.Parameters.Add("@Registro_Observacion", SqlDbType.VarChar).Value = r.registro_Observacion;
                            cmd.Parameters.Add("@Grupo_Incidencia_Codigo", SqlDbType.VarChar).Value = r.grupo_Incidencia_Codigo;
                            cmd.Parameters.Add("@Registro_TieneFoto", SqlDbType.VarChar).Value = r.registro_TieneFoto;
                            cmd.Parameters.Add("@Registro_TipoProceso", SqlDbType.VarChar).Value = r.registro_TipoProceso;
                            cmd.Parameters.Add("@Fecha_Sincronizacion_Android", SqlDbType.VarChar).Value = r.fecha_Sincronizacion_Android;
                            cmd.Parameters.Add("@Registro_Constancia", SqlDbType.VarChar).Value = r.registro_Constancia;
                            cmd.Parameters.Add("@Registro_Desplaza", SqlDbType.VarChar).Value = r.registro_Desplaza;
                            cmd.Parameters.Add("@Codigo_Resultado", SqlDbType.VarChar).Value = r.codigo_Resultado;
                            cmd.Parameters.Add("@horaActa", SqlDbType.VarChar).Value = r.horaActa == null ? "" : r.horaActa;
                            cmd.Parameters.Add("@Suministro_Numero", SqlDbType.Int).Value = r.suministro_Numero;
                            SqlDataReader dr = cmd.ExecuteReader();
                            m = new Mensaje();
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    lastId = dr.GetInt32(0);
                                    if (lastId != 0)
                                    {
                                        foreach (var itemS in r.photos)
                                        {
                                            SqlCommand cmds = con.CreateCommand();
                                            cmds.CommandType = CommandType.StoredProcedure;
                                            cmds.CommandText = "USP_SAVE_REGISTRO_PHOTO";
                                            cmds.Parameters.Add("@ID_Registro", SqlDbType.Int).Value = lastId;
                                            cmds.Parameters.Add("@RutaFoto", SqlDbType.VarChar).Value = itemS.rutaFoto;
                                            cmds.ExecuteNonQuery();
                                        }
                                    }
                                    m.mensaje = "Datos Enviados";
                                }
                            }
                            else
                            {
                                return m;
                            }
                        }
                    }
                    else if (r.tipo == 6)
                    {
                        using (SqlCommand cmd = new SqlCommand("USP_SAVE_SUMINISTRO_ENCONTRADO", con))
                        {
                            cmd.CommandTimeout = 0;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@ID_Registro", SqlDbType.Int).Value = r.iD_Registro;
                            cmd.Parameters.Add("@id_operario", SqlDbType.Int).Value = r.iD_Operario;
                            cmd.Parameters.Add("@suministro_medidor", SqlDbType.VarChar).Value = r.iD_Suministro;
                            cmd.Parameters.Add("@suministro_contrato", SqlDbType.VarChar).Value = r.registro_Constancia;
                            cmd.Parameters.Add("@suministro_cliente", SqlDbType.VarChar).Value = r.suministroCliente;
                            cmd.Parameters.Add("@suministro_direccion", SqlDbType.VarChar).Value = r.suministroDireccion;
                            cmd.Parameters.Add("@suministro_observacion", SqlDbType.VarChar).Value = r.registro_Observacion;
                            cmd.Parameters.Add("@fecha_movil", SqlDbType.VarChar).Value = r.registro_Fecha_SQLITE;

                            SqlDataReader dr = cmd.ExecuteReader();
                            m = new Mensaje();
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    lastId = dr.GetInt32(0);
                                    if (lastId != 0)
                                    {
                                        foreach (var itemS in r.photos)
                                        {
                                            SqlCommand cmds = con.CreateCommand();
                                            cmds.CommandType = System.Data.CommandType.StoredProcedure;
                                            cmds.CommandText = "USP_SAVE_REGISTRO_PHOTO_LECTURA";
                                            cmds.Parameters.Add("@ID_Registro", SqlDbType.Int).Value = lastId;
                                            cmds.Parameters.Add("@RutaFoto", SqlDbType.VarChar).Value = itemS.rutaFoto;
                                            cmds.ExecuteNonQuery();
                                        }
                                    }

                                    m.mensaje = "Datos Enviados";
                                }
                            }
                            else
                            {
                                return m;
                            }
                        }
                    }
                    else if (r.tipo == 8)
                    {
                        using (SqlCommand cmd = new SqlCommand("USP_SAVE_ZONA_PELIGROSA", con))
                        {
                            cmd.CommandTimeout = 0;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@ID_Registro", SqlDbType.Int).Value = r.iD_Registro;
                            cmd.Parameters.Add("@id_operario", SqlDbType.Int).Value = r.iD_Operario;
                            cmd.Parameters.Add("@latitud", SqlDbType.VarChar).Value = r.registro_Latitud;
                            cmd.Parameters.Add("@longitud", SqlDbType.VarChar).Value = r.registro_Longitud;
                            cmd.Parameters.Add("@suministro_observacion", SqlDbType.VarChar).Value = r.registro_Observacion;
                            cmd.Parameters.Add("@fecha_movil", SqlDbType.VarChar).Value = r.registro_Fecha_SQLITE;

                            SqlDataReader dr = cmd.ExecuteReader();
                            m = new Mensaje();
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    lastId = dr.GetInt32(0);
                                    if (lastId != 0)
                                    {
                                        foreach (var itemS in r.photos)
                                        {
                                            SqlCommand cmds = con.CreateCommand();
                                            cmds.CommandType = System.Data.CommandType.StoredProcedure;
                                            cmds.CommandText = "USP_SAVE_REGISTRO_PHOTO_LECTURA";
                                            cmds.Parameters.Add("@ID_Registro", SqlDbType.Int).Value = lastId;
                                            cmds.Parameters.Add("@RutaFoto", SqlDbType.VarChar).Value = itemS.rutaFoto;
                                            cmds.ExecuteNonQuery();
                                        }
                                    }

                                    m.mensaje = "Datos Enviados";
                                }
                            }
                            else
                            {
                                return m;
                            }
                        }
                    }
                    con.Close();
                }
                return m;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        // Sincronizar

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



    }
}