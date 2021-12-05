using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace TestTaskSCV
{
    internal class Program
    {
        private static int countVisits = 0;//количество посещений (можно было конечно создать внутри метода и передавать как ref)
        private static readonly DateTime dateFrom = DateTime.Parse("2016.04.01 00:00:00");//начальная дата проверки
        private static readonly DateTime dateTo = DateTime.Parse("2016.10.02 00:00:00");//конечная дата проверки
        static void Main(string[] args)
        {
            var path = Directory.GetCurrentDirectory();
            var files = Directory.GetFiles(path, "*.csv");//все .scv файлы
            StringBuilder names = new StringBuilder(); //названия файлов которые считываем
            var circle = new Circle() { lon = 32.02394, lat = 54.8707, rad = 5000.0 };//заданный круг
            List<Task> tasks = new List<Task>();
            foreach (var file in files)
            {
                if (Path.GetFileName(file) != "out.csv") //выходной файл не проверяем
                {
                    tasks.Add(Parse(file, circle));
                    names.AppendLine(Path.GetFileName(file));
                }
            }


            Task.WaitAll(tasks.ToArray());

            using (FileStream fs = File.Create(path + "\\out.csv"))
            {
                byte[] info = new UTF8Encoding(true).GetBytes(names + $"\n Number of visits of this control area: {countVisits}");
                fs.Write(info, 0, info.Length);
            }
        }
        static async Task Parse(string path, Circle circle)
        {
            string line;

            using (StreamReader reader = new StreamReader(File.OpenRead(path)))
            {
                Car previousCar = null;
                while ((line = await reader.ReadLineAsync()) != null && previousCar == null)//ищем первую валидную запись которую сможем привести к типу Car
                {
                    try
                    {
                        var valid = new Car(line);
                        previousCar = valid;
                    }
                    catch
                    {
                        continue;//если line крашит конструктор Car (например данными точки NULL) то пропускаем такую строку
                    }
                }
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    try
                    {
                        //по сути для ускорения и уменьшения памяти можно сразу парсить только дату и если она не подходит идти дальше, и создавать объекты типа Point
                        var car = new Car(line);
                        if (previousCar.IsBetween(dateFrom, dateTo) && car.IsBetween(dateFrom, dateTo))
                            if (circle.IsCrossing(previousCar,car))
                                countVisits++; //проверяем что предыдущая "машина" и текущая пересекают круг в данный промежуток времени
                        previousCar = car;
                    }
                    catch
                    {
                        continue; //если line крашит конструктор Car (например данными точки NULL) то пропускаем такую строку
                    }
                }
            }
        }
    }
}
