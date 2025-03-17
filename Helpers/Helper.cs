using System.Net.Mime;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MasterCore8.Data;
using System.Data.Common;
using System.Data;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Text.Json;
using System.Drawing;

namespace MasterCore8.Helpers
{
    public static class Helper
    {
        public static DateTime ToDate(string date="",string format="", string year="en")
        {            
            if(year == "en") year = "en-US";
            if(year == "th") year = "th-TH";
            if(format == "") {
                format = $"dd-MM-yyyy";
            }
            IFormatProvider culture = System.Globalization.CultureInfo.CreateSpecificCulture(year);
            return DateTime.ParseExact(date, format, culture);
        }

        public static DateTime ToDateTime(string date="", string format="", string year="en")
        {            
            if(year == "en") year = "en-US";
            if(year == "th") year = "th-TH";
            if(format == "") {
                format = $"dd-MM-yyyy HH:mm";
            }
            IFormatProvider culture = System.Globalization.CultureInfo.CreateSpecificCulture(year);
            return DateTime.ParseExact(date, format, culture);
        }

        public static string DateToString(DateTime? date, string format="en",string connector="-", string year="en-US")
        {
            try
            {
                if(year == "en") year = "en-US";
                if(year == "th") year = "th-TH";
                if(format == "en"){
                    format = $"yyyy{connector}MM{connector}dd";
                }else if(format == "th"){
                    format = $"dd{connector}MM{connector}yyyy";
                }
                IFormatProvider culture = System.Globalization.CultureInfo.CreateSpecificCulture(year);
                return date?.ToString(format, culture);
            }
            catch (System.Exception)
            {
                return "";
            }
        }

        public static string DateTimeToString(DateTime? date, string format="en",string connector="-", string year="en-US")
        {            
            try
            {
                if(year == "en") year = "en-US";
                if(year == "th") year = "th-TH";
                if(format == "en"){
                    format = $"yyyy{connector}MM{connector}dd HH:mm";
                }else if(format == "th"){
                    format = $"dd{connector}MM{connector}yyyy HH:mm";
                }
                IFormatProvider culture = System.Globalization.CultureInfo.CreateSpecificCulture(year);
                return date?.ToString(format, culture); 
            }
            catch (System.Exception)
            {
                return "";
                throw;
            }
            
        }

        public static string DictionaryValue(Dictionary<string, string> dic, string key, string defaultValue="")
        {            
            try
            {
                return dic[key];
            }
            catch (System.Exception)
            {
                return defaultValue;
                throw;
            }            
        }

        public static string StringZero(string number="", string defaultValue="")
        {            
            try
            {
                if(number=="0" || string.IsNullOrEmpty(number)){
                    return defaultValue;
                }
                return number;
            }
            catch (System.Exception)
            {
                return defaultValue;
                throw;
            }            
        }
        public static string IntZero(int number=0, string defaultValue="", string format=null)
        {            
            try
            {
                if(number==0){
                    return defaultValue;
                }
                return number.ToString(format);
            }
            catch (System.Exception)
            {
                return defaultValue;
                throw;
            }            
        }

        public static DataTable RawSqlQueryDataTable(ApplicationDbContext context,  string sqlQuery, params DbParameter[] parameters)
        {
            DataTable dataTable = new DataTable();
            DbConnection connection = context.Database.GetDbConnection();
            DbProviderFactory dbFactory = DbProviderFactories.GetFactory(connection);
            using (var cmd = dbFactory.CreateCommand())
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sqlQuery;
                if (parameters != null)
                {
                    foreach (var item in parameters)
                    {
                        cmd.Parameters.Add(item);
                    }
                }
                using (DbDataAdapter adapter = dbFactory.CreateDataAdapter())
                {
                    adapter.SelectCommand = cmd;
                    adapter.Fill(dataTable);
                }
            }
            return dataTable;
        }

        public static async Task<List<T>> RawSqlQueryToList<T>(ApplicationDbContext context,  string sqlQuery, params DbParameter[] parameters)
        {            
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = sqlQuery;
                command.CommandType = CommandType.Text;
                if (parameters != null)
                {
                    foreach (var item in parameters)
                    {
                        command.Parameters.Add(item);
                    }
                }
                context.Database.OpenConnection();

                using (var result = await command.ExecuteReaderAsync())
                {
                    List<T> list = new List<T>();
                    T obj = default(T);
                    while (result.Read()) {
                        obj = Activator.CreateInstance<T>();
                        foreach (PropertyInfo prop in obj.GetType().GetProperties()) {
                            try
                            {
                               if (!object.Equals(result[prop.Name] ?? null, DBNull.Value)) {
                                    prop.SetValue(obj, result[prop.Name] ?? null, null);
                                } 
                            }
                            catch (System.Exception)
                            {                                
                                // throw;
                            }
                            
                        }
                        list.Add(obj);
                    }
                    return list;
                }
            }
        }

        public static string StripHtmlTags(string source)  
        {  
            try
            {                
                return Regex.Replace(source, "<.*?>|&.*?;", string.Empty);
            }
            catch (System.Exception)
            {                
                return source;
            }
        } 

        public static string StringLimit(string source, int limit=20, string defaultConnect = "...")  
        {  
            try
            {
                if (source.Length <= limit)
                {
                    return source;
                }

                return source.Substring(0, limit)+defaultConnect;
            }
            catch (System.Exception)
            {
                return source;
                throw;
            }
            
        }        

        public static string GetVersionByJson(string jsonpath="",string projectname=""){
            try
            {
                var jsonData = System.IO.File.ReadAllText(jsonpath);
                var result_version = jsonData.Substring(jsonData.IndexOf($"{projectname}/")+(projectname.Length+1),15);
                return result_version;
            }
            catch (System.Exception)
            {
                return "DevVersion";
                throw;
            }
        }

        public static string DataTableToJSON(DataTable _dt)
        {
            string data = "[";
            for (int i = 0; i < _dt.Rows.Count; i++)
            {
                string row = "{";
                for (int j = 0; j < _dt.Columns.Count; j++)
                {
                    row += $"\"{_dt.Columns[j].ColumnName.ToString()}\":\"{_dt.Rows[i][j].ToString()}\",";
                }
                row = row.Substring(0, row.Length - 1);
                row += "},";
                data += row;
            }
            data = data.Substring(0, data.Length - 1);
            data += "]";
            return data;
        }

        public static DateTime DateShiftConvert(DateTime dt){
            if(dt.Hour > 7){
                return dt;
            }else{
                Console.WriteLine("=======================");
                Console.WriteLine(dt.AddDays(-1));
                return dt.AddDays(-1);
            }
        }

        public static Image ScaleImage(Image image, int maxWidth, int maxHeight)
        {
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);

            using (var graphics = Graphics.FromImage(newImage))
                graphics.DrawImage(image, 0, 0, newWidth, newHeight);

            return newImage;
        }
    }
}