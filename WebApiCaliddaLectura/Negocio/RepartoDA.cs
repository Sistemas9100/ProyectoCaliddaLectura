using Entidades;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Negocio
{
    public class RepartoDA
    {
        private static string db = ConfigurationManager.ConnectionStrings["conexionDsige"].ConnectionString;

        public static List<Reparto> GetReparto(int operarioId)
        {
            try
            {
                List<Reparto> reparto = new List<Reparto>();
                using (SqlConnection cn = new SqlConnection(db))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand("MOVIL_Reparto_list", cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id_operario_reparto", operarioId);

                        DataTable dt_detalle = new DataTable();
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dt_detalle);
                            foreach (DataRow dr in dt_detalle.Rows)
                            {
                                Reparto rp = new Reparto();
                                rp.id_Reparto = Convert.ToInt32(dr["id_Reparto"]);
                                rp.id_Operario_Reparto = Convert.ToInt32(dr["id_Operario_Reparto"]);
                                rp.foto_Reparto = Convert.ToInt32(dr["foto_Reparto"]);
                                rp.estado = Convert.ToInt32(dr["estado"]);
                                rp.activo = Convert.ToInt32(dr["activo"]);
                                rp.Suministro_Medidor_reparto = dr["Suministro_Medidor"].ToString();
                                rp.Suministro_Numero_reparto = dr["Suministro_Numero"].ToString();
                                rp.Cod_Actividad_Reparto = dr["Cod_Actividad_Reparto"].ToString();
                                rp.Cod_Orden_Reparto = dr["Cod_Orden_Reparto"].ToString();
                                rp.Direccion_Reparto = dr["Direccion_Reparto"].ToString();
                                rp.Cliente_Reparto = dr["Cliente_Reparto"].ToString();
                                rp.CodigoBarra = dr["CodigoBarra"].ToString();
                                reparto.Add(rp);
                            }
                        }
                    }
                }
                return reparto;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        
        public static string saveRepartoRegistro(List<SendReparto> objtbl_Alm_GuiasDet)
        {
            try
            {
                int lastId;
                using (SqlConnection con = new SqlConnection(db))
                {
                    con.Open();
                    foreach (var a in objtbl_Alm_GuiasDet)
                    {

                        using (SqlCommand cmd = new SqlCommand("MOVIL_Reparto_save", con))
                        {
                            cmd.CommandTimeout = 0;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@id_operario_reparto", a.id_Operario_Reparto);
                            cmd.Parameters.AddWithValue("@ID_Registro", 1);
                            cmd.Parameters.AddWithValue("@id_reparto", a.id_Reparto);
                            cmd.Parameters.AddWithValue("@registro_fecha_sqlite", a.registro_fecha_sqlite);
                            cmd.Parameters.AddWithValue("@registro_latitud", a.registro_latitud);
                            cmd.Parameters.AddWithValue("@registro_longitud", a.registro_longitud);
                            cmd.Parameters.AddWithValue("@id_observacion", a.id_observacion);
                            SqlDataReader dr = cmd.ExecuteReader();
                            while (dr.Read())
                            {
                                lastId = dr.GetInt32(0);
                                foreach (var item in a.reparto_foto)
                                {

                                    using (SqlConnection cn = new SqlConnection(db))
                                    {

                                        Mensaje m = new Mensaje();
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
    }
}
