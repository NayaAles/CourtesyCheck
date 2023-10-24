
namespace CourtesyСheck
{
    public static class PoliteData
    {
        private const string YearFind = "(YEAR FROM DateInMoscowTz) =";
        private const string MonthFind = "(MONTH FROM DateInMoscowTz) =";

        public static void Calculation(Dictionary<string, string> managers, Dictionary<int, int> period, string pathSqlSample, string pathOut)
        {
            var sqlQuery = "";
            using (StreamReader reader = new StreamReader(pathSqlSample))
                sqlQuery = reader.ReadToEnd();

            var allDatas = new List<Polite>();
            foreach (var key in period)
            {
                string year = key.Key.ToString();

                for (int i = 1; i < key.Value + 1; i++)
                {
                    string month = (int)Math.Log10(i) + 1 == 1 ? String.Concat("0", i.ToString()) : i.ToString();

                    var pastAllInQuery = AddPeriodInQuery(sqlQuery, year, month);
                    var results = BigQuery.GetQueryResult<ManagerByPeriod>(false, pastAllInQuery);

                    var datas = new List<Polite>();
                    foreach (var manager in results)
                    {
                        var persent = Math.Ceiling(Convert.ToDouble(manager.PoliteAll * 100 / manager.EmailsCountAll));
                        int per = Convert.ToInt32(persent);

                        datas.Add(new CourtesyСheck.Polite
                        {
                            Date = string.Concat(month, ".", year),
                            ManagerEmail = manager.ManagerEmail,
                            Name = managers[manager.ManagerEmail],
                            EmailsCountAll = manager.EmailsCountAll,
                            PoliteAll = manager.PoliteAll,
                            Persent = per
                        });

                        allDatas.Add(new CourtesyСheck.Polite
                        {
                            Date = string.Concat(month, ".", year),
                            ManagerEmail = manager.ManagerEmail,
                            Name = managers[manager.ManagerEmail],
                            EmailsCountAll = manager.EmailsCountAll,
                            PoliteAll = manager.PoliteAll,
                            Persent = per
                        });
                    }

                    ReaderWriterCsv.ReaderWriterCsv.SaveToCsv<Polite>(datas, $@"{pathOut}\{year}-{month}Polite.csv", ';', false);
                }
            }

            ReaderWriterCsv.ReaderWriterCsv.SaveToCsv<Polite>(allDatas, $@"{pathOut}\AllDataPolite.csv", ';', false);
        }

        public static List<Manager> GetManagers(string pathSqlManagers)
        {
            return BigQuery.GetQueryResult<Manager>(true, pathSqlManagers);
        }

        private static string AddPeriodInQuery(string sqlQuery, string year, string month)
        {
            int start = sqlQuery.IndexOf(YearFind) + YearFind.Length;
            string yearPast = sqlQuery.Insert(start, year);

            int startNext = yearPast.IndexOf(MonthFind) + MonthFind.Length;
            return yearPast.Insert(startNext, month);
        }

        public static void Check(string pathCheck)
        {
            var results = BigQuery.GetQueryResult<Check>(true, pathCheck);
            ReaderWriterCsv.ReaderWriterCsv.SaveToCsv<Check>(results, $@"{pathCheck}\2023-09Check.csv", ';', false);
        }
    }
}
