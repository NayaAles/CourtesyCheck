using Google.Apis.Auth.OAuth2;
using Google.Cloud.BigQuery.V2;
using ReaderWriterCsv;
using System.ComponentModel;
using System.Reflection;

namespace CourtesyСheck
{
    public class BigQuery
    {
        private static string Directory = CurrentDirectory.Get(3);
        private static string ProjectId = File.ReadAllText(Directory + @"\ProjectId.txt");
        private static BigQueryClient Client = CreateSqlClient();

        private static string ReadSqlQuery(string path)
        {
            using (var reader = new StreamReader(path))
                return reader.ReadToEnd();
        }

        private static BigQueryClient CreateSqlClient()
        {
            string jsonPath = Directory + @"\credentials.json";

            var credentials = GoogleCredential.FromFile(jsonPath);
            return BigQueryClient.Create(ProjectId, credentials);
        }

        public static List<T> GetQueryResult<T>(bool turnReadFromFile, string queryOrPath)
        {
            string query = queryOrPath;
            if (turnReadFromFile && File.Exists(queryOrPath))
                query = ReadSqlQuery(queryOrPath);

            BigQueryParameter[] parameters = null;
            BigQueryResults results = Client.ExecuteQuery(query, parameters);

            var datas = new List<T>();

            T obj = (T)Activator.CreateInstance(typeof(T));

            var fields = GetFields<T>()
                .Where(x => !x.Contains("<Id>"))
                .ToList();

            int fieldsCount = fields
                .Count();

            foreach (BigQueryRow row in results)
            {
                var tempInData = (T)Activator.CreateInstance(typeof(T));

                for (int j = 0; j < fieldsCount; j++)
                {
                    var data = row[j] == null ? null : row[j].ToString().Trim();
                    if (tempInData != null)
                    {
                        var value = tempInData.GetType()
                          .GetField(fields[j], BindingFlags.Instance | BindingFlags.NonPublic);

                        if (value != null && !String.IsNullOrEmpty(data))
                        {
                            var fieldType = value.FieldType;
                            var resultData = TypeDescriptor.GetConverter(fieldType)
                                .ConvertFrom(data);

                            value.SetValue(tempInData, resultData);
                        }
                    }
                }

                datas.Add(tempInData);
            }

            return datas;
        }

        private static string[] GetFields<T>()
        {
            var fields = typeof(T).GetRuntimeFields()
            .Where(x => !x.FieldType.Attributes.ToString().Contains("NestedPublic")
            && !x.FieldType.Attributes.ToString().Contains("ClassSemanticsMask"))
                .Select(x => x.Name)
                .ToArray();

            return fields;
        }
    }
}
