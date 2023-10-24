using CourtesyСheck;
using ReaderWriterCsv;

var currentDirectory = CurrentDirectory.Get(3);
var pathSqlSample = currentDirectory + @"\query.txt";
var pathOut = currentDirectory + @"\Results\";
var pathSqlManagers = currentDirectory + @"\queryManagers.txt";

//  Change the period before unloading!
var period = new Dictionary<int, int>()
{
    [2023] = 10,
    [2022] = 12,
    [2021] = 12,
    [2020] = 12,
    [2019] = 12,
    [2018] = 12,
    [2017] = 12
};

var managers = new Dictionary<string, string>()
{
    ["ddn"] = "Савельева Дарья",
    ["lnm"] = "Лысиков Назар",
    ["ivg"] = "Горбунов Иван",
    ["bvv"] = "Бондырев Вячеслав",
    ["uas"] = "Шенчук Юлианна",
    ["kap"] = "Капитанов Алексей",
    ["fl2"] = "Бейбит Лаура",
    ["fl1"] = "Мясников Максим",
    ["kir"] = "Рыжов Кирилл",
    ["fl3"] = "Кузнецов Илья",
    ["fl4"] = "Степанова Полина",
    ["kam"] = "Каманина Евгения",
    ["kda"] = "Кидинкин Денис",
    ["zam"] = "Замараев Алексей",
    ["vea"] = "Войтов Евгений",
    ["sap"] = "Сапрыкин Степан",
    ["ism"] = "Манойло Илья",
    ["kal"] = "Калашников Алексей",
    ["ddk"] = "Демченко Диана",
    ["tav"] = "Ткаченко Анастасия"
};

var managersInBigQuery = PoliteData.GetManagers(pathSqlManagers);
var checkManagers = managersInBigQuery.Where(x => managers.ContainsKey(x.ManagerEmail))
    .Count();

if (checkManagers != managers.Count)
    throw new Exception("The list of managers needs to be updated");

PoliteData.Calculation(managers, period, pathSqlSample, pathOut);

//  Examination
//PoliteData.Check(pathOut);


