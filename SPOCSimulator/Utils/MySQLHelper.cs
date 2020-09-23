using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPOCSimulator.Utils
{
    public static class MySQLHelper
    {

        private static string GetTypeString(Type type)
        {
            if (type == typeof(string)) return "VARCHAR(100) NOT NULL";
            if (type == typeof(int)) return "INT NOT NULL";
            if (type == typeof(double)) return "DOUBLE NOT NULL";
            return "TEXT";
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

    }
}
