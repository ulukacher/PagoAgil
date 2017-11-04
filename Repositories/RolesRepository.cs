﻿using PagoAgilFrba.Classes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PagoAgilFrba.Repositories
{
    public class RolesRepository
    {

        public static void AgregarRol(Rol rol)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GDD"].ConnectionString);

            conn.Open();

            string query = "insert into LOS_MANTECOSOS.Roles values (@nombre,@activo); DECLARE @id_rol numeric(18,0) = SCOPE_IDENTITY();";
            foreach (var func in rol.Funcionalidades)
            {
                query += "insert into LOS_MANTECOSOS.FuncionalidadesPorRoles values (@id_rol," 
                    + func.Id.ToString() + ");";
            }
            SqlCommand command = new SqlCommand(query, conn);

            command.Parameters.AddWithValue("@nombre", rol.Nombre);
            command.Parameters.AddWithValue("@activo", 1);

            command.ExecuteNonQuery();

            conn.Close();
        }

        public static int GetCantidadRolesConEseNombre(string nombre)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GDD"].ConnectionString);
            conn.Open();
            SqlCommand command = new SqlCommand();
            string query = "SELECT COUNT(*) as cantidad FROM GD2C2017.LOS_MANTECOSOS.Roles Where rol_nombre = @nombre";
            command.Connection = conn;
            command.CommandText = query;
            command.Parameters.AddWithValue("@nombre", nombre);
            int cant=-1;
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    cant = Convert.ToInt32(reader["cantidad"]);
                }
            }
            conn.Close();
            return cant;
        }

        public static List<Funcionalidad> GetAllFuncionalidades()
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GDD"].ConnectionString);
            conn.Open();
            SqlCommand command = new SqlCommand();
            string query = "SELECT  func_id , func_nombre FROM GD2C2017.LOS_MANTECOSOS.Funcionalidades";
            command.Connection = conn;
            command.CommandText = query;
            List<Funcionalidad> funcs = new List<Funcionalidad>();
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Funcionalidad func = new Funcionalidad();
                    func.Id = Convert.ToInt32(reader["func_id"]);
                    func.Nombre = (string)reader["func_nombre"];
                    funcs.Add(func);
                }
            }
            conn.Close();
            return funcs;
        }

        public static List<Rol> GetAllRoles()
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GDD"].ConnectionString);
            conn.Open();
            SqlCommand command = new SqlCommand();
            string query = "SELECT  rol_id , rol_nombre, rol_activo FROM GD2C2017.LOS_MANTECOSOS.Roles";
            command.Connection = conn;
            command.CommandText = query;
            List<Rol> roles = new List<Rol>();
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Rol rol = new Rol();
                    rol.Id = Convert.ToInt32(reader["rol_id"]);
                    rol.Nombre  = (string)reader["rol_nombre"];
                    rol.Activo = (bool)reader["rol_activo"];
                    roles.Add(rol);
                }
            }
            conn.Close();
            return roles;
        }
    }
}
