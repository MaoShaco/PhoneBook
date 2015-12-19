using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Npgsql;

namespace PhoneBookDao
{
    public static class PostgreConnection
    {
        private static readonly string Connstring =
            $"Server={"127.0.0.1"};Port={"5432"};User Id={"user"};Password={"user"};Database={"PhoneBook"};";

        private static readonly NpgsqlConnection Connection = new NpgsqlConnection(Connstring);

        private static void OpenConn()
        {
            if (Connection.State == ConnectionState.Open)
                Connection.Close();

            Connection.Open();
        }

        private static void CloseConn()
        {
            Connection.Close();
        }

        public static int InsertOnTable(string table, Object insertingObject)
        {
            OpenConn();

            var objectParams = insertingObject.GetType().GetProperties().Aggregate("",
                (current, property) => current + $"'{property.GetValue(insertingObject)}',").Replace("'0',", "");
            objectParams = objectParams.Remove(objectParams.LastIndexOf(','), 1);

            var sql = $"INSERT INTO {table} VALUES ({objectParams}) RETURNING id";

            NpgsqlCommand command = new NpgsqlCommand(sql, Connection);
            var executeScalar = command.ExecuteScalar();
            CloseConn();

            if (executeScalar == null) return 0;
            return (int) executeScalar;

        }

        public static void UpdateOnTable(string table, Object objGen, int idValue)
        {
            OpenConn();

            var objectParams = objGen.GetType().GetProperties().Aggregate("",
                (current, property) =>
                    current +
                    $"{property.ToString().Substring(property.ToString().LastIndexOf(' '))} = '{property.GetValue(objGen)}',")
                .Replace("'0',", "");
            objectParams = objectParams.Remove(objectParams.LastIndexOf(','), 1);

            var sql = $"UPDATE {table} SET {objectParams} WHERE id = {idValue}";

            NpgsqlCommand command = new NpgsqlCommand(sql, Connection);
            command.ExecuteNonQuery();

            CloseConn();
        }

        public static void DeleteOnTable(string table, Object objGen, int idValue)
        {
            OpenConn();

            var sql = $"DELETE FROM {table} WHERE id = {idValue};";

            NpgsqlCommand command = new NpgsqlCommand(sql, Connection);
            command.ExecuteNonQuery();

            CloseConn();
        }

        public static List<Object> QueryAllOnTable(string table)
        {
            OpenConn();

            List<Object> lstSelect = new List<Object>();
            string sql = $"SELECT * FROM {table}";

            NpgsqlCommand command = new NpgsqlCommand(sql, Connection);
            NpgsqlDataReader dr = command.ExecuteReader();

            while (dr.Read())
            {
                for (int i = 0; i < dr.FieldCount; i++)
                {
                    lstSelect.Add(dr[i]);
                }
            }

            CloseConn();

            return lstSelect;
        }

        public static List<Object> QueryOnTableWithParams(string table, Dictionary<string, string> paramsDictionary)
        {
            OpenConn();

            List<Object> lstSelect = new List<Object>();

            var objectParams = paramsDictionary.Aggregate("",
                (current, name) => current + $"{name.Key} = {name.Value} AND ");
            objectParams = objectParams.Remove(objectParams.LastIndexOf("AND", StringComparison.Ordinal));

            var sql = $"SELECT * FROM {table} WHERE {objectParams};";

            NpgsqlCommand command = new NpgsqlCommand(sql, Connection);
            NpgsqlDataReader dr = command.ExecuteReader();

            while (dr.Read())
            {
                for (int i = 0; i < dr.FieldCount; i++)
                {
                    lstSelect.Add(dr[i]);
                }
            }

            CloseConn();

            return lstSelect;
        }
    }
}
