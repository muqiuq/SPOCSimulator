using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPOCSimulator.Utils
{
    public static class InternalMySqlHelper
    {

        private static string GetTypeString(Type type)
        {
            if (type == typeof(string)) return "VARCHAR(100) NOT NULL";
            if (type == typeof(int)) return "INT NOT NULL";
            if (type == typeof(long)) return "BIGINT NOT NULL";
            if (type == typeof(double)) return "DOUBLE NOT NULL";
            return "TEXT";
        }

        private static MySqlDbType GetMySqlDbType(Type type)
        {
            if (type == typeof(string)) return MySqlDbType.VarChar;
            if (type == typeof(int)) return MySqlDbType.Int32;
            if (type == typeof(long)) return MySqlDbType.Int64;
            if (type == typeof(double)) return MySqlDbType.Double;
            return MySqlDbType.Text;
        }

        public static string GetCreateTable(string tableName, Type type)
        {
            var fields = type.GetFields();
            List<string> lineCommands = new List<string>();
            foreach(var field in fields)
            {
                lineCommands.Add(field.Name + " " + GetTypeString(field.FieldType));
            }
            return string.Format("CREATE TABLE {0} ({1});", tableName, string.Join(',', lineCommands));

        }

        public static MySqlCommand GetInsert(MySqlConnection conn, string tableName, object insertObject) 
        {
            List<string> fieldNames = new List<string>();
            List<string> fieldValues = new List<string>();
            var fields = insertObject.GetType().GetFields();
            foreach(var field in fields)
            {
                fieldNames.Add(field.Name);
                fieldValues.Add("@" + field.Name);
            }

            string cmdText = string.Format("INSERT INTO {0} ({1}) VALUES ({2})",
                tableName,
                string.Join(',', fieldNames),
                string.Join(',', fieldValues)
                );

            MySqlCommand cmd = new MySqlCommand(cmdText, conn);
            foreach(var field in fields)
            {
                cmd.Parameters.Add("@" + field.Name, GetMySqlDbType(field.FieldType)).Value = field.GetValue(insertObject);
            }
            return cmd;
        }

        public static MySqlCommand GetBulkInsert(MySqlConnection conn, string tableName, params object[] insertObjects)
        {
            List<string> fieldNames = new List<string>();
            var fields = insertObjects[0].GetType().GetFields();
            foreach (var field in fields)
            {
                fieldNames.Add(field.Name);
            }
            List<string> values = new List<string>();
            foreach(var insertObject in insertObjects)
            {
                List<string> fieldValues = new List<string>();
                foreach (var field in fields)
                {
                    if(field.FieldType == typeof(string))
                    {
                        fieldValues.Add("\"" + MySqlHelper.EscapeString(field.GetValue(insertObject).ToString()) + "\"");
                    }
                    else
                    {
                        fieldValues.Add(field.GetValue(insertObject).ToString());
                    }
                }
                values.Add("(" + string.Join(",", fieldValues) + ")");
            }
            
            string cmdText = string.Format("INSERT INTO {0} ({1}) VALUES {2};",
                tableName,
                string.Join(',', fieldNames),
                string.Join(',', values)
                );

            MySqlCommand cmd = new MySqlCommand(cmdText, conn);
            
            return cmd;
        }
    }
}
