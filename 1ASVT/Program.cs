using System;
using System.IO;
using System.Threading;

class Program
{
    // Создаем объект мьютекса
    private static Mutex mutex = new Mutex();
    // Путь к файлу, куда будем записывать данные
    private static string filePath = "weatherData.txt";

    static void Main(string[] args)
    {
        // Создаем четыре потока
        Thread thread1 = new Thread(CollectWeatherData);
        Thread thread2 = new Thread(CollectWeatherData);
        Thread thread3 = new Thread(CollectWeatherData);
        Thread thread4 = new Thread(CollectWeatherData);

        // Запускаем потоки
        thread1.Start("Источник 1");
        thread2.Start("Источник 2");
        thread3.Start("Источник 3");
        thread4.Start("Источник 4");

        // Ожидать завершения выполнения всех потоков
        thread1.Join();
        thread2.Join();
        thread3.Join();
        thread4.Join();

        Console.WriteLine("Сбор данных завершен.");
    }

    // Метод, моделирующий сбор данных о погоде и запись их в файл
    static void CollectWeatherData(object source)
    {
        // Генерируем случайные погодные данные
        Random rand = new Random();
        string weatherData = $"{source}: Температура = {rand.Next(-20, 40)}°C, " +
                             $"Влажность = {rand.Next(0, 100)}%\n";

        // Используем мьютекс для обеспечения потокобезопасности при записи в файл
        mutex.WaitOne(); // Получаем доступ к критической секции
        try
        {
            Console.WriteLine($"{source} начинает запись в файл.");
            // Пишем данные в файл
            File.AppendAllText(filePath, weatherData);
            Console.WriteLine($"{source} завершил запись в файл.");
        }
        finally
        {
            // Освобождаем мьютекс
            mutex.ReleaseMutex();
        }
    }
}