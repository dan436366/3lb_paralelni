using System;
using System.Collections.Generic;
using System.Threading;

class Program
{
    // Константа: максимальна місткість буфера
    const int BUFFER_CAPACITY = 5;

    // Семафори
    static Semaphore empty = new Semaphore(BUFFER_CAPACITY, BUFFER_CAPACITY);
    static Semaphore full = new Semaphore(0, BUFFER_CAPACITY);
    static Semaphore mutex = new Semaphore(1, 1);

    // Буфер як черга
    static Queue<int> buffer = new Queue<int>();

    static int productId = 0;

    static void Producer(object obj)
    {
        int itemsToProduce = (int)obj;
        for (int i = 0; i < itemsToProduce; i++)
        {
            empty.WaitOne();        // чекає, поки є місце
            mutex.WaitOne();        // взаємовиключення

            int product = Interlocked.Increment(ref productId);
            buffer.Enqueue(product);
            Console.WriteLine($"Виробник {Thread.CurrentThread.ManagedThreadId} створив товар {product}");

            mutex.Release();        // звільняє доступ
            full.Release();         // інформує, що є новий товар

            Thread.Sleep(new Random().Next(100, 300));
        }
    }

    static void Consumer(object obj)
    {
        int itemsToConsume = (int)obj;
        for (int i = 0; i < itemsToConsume; i++)
        {
            full.WaitOne();         // чекає, поки є щось у буфері
            mutex.WaitOne();        // взаємовиключення

            int product = buffer.Dequeue();
            Console.WriteLine($"Споживач {Thread.CurrentThread.ManagedThreadId} спожив товар {product}");

            mutex.Release();        // звільняє доступ
            empty.Release();        // інформує, що є вільне місце

            Thread.Sleep(new Random().Next(100, 300));
        }
    }

    static void Main()
    {
        int numProducers = 3;
        int numConsumers = 2;
        int totalProduction = 15;

        // Обчислюємо скільки кожен має обробити
        int itemsPerProducer = totalProduction / numProducers;
        int itemsPerConsumer = totalProduction / numConsumers;

        // Створюємо потоки виробників
        Thread[] producers = new Thread[numProducers];
        for (int i = 0; i < numProducers; i++)
        {
            producers[i] = new Thread(Producer);
            producers[i].Start(itemsPerProducer);
        }

        // Створюємо потоки споживачів
        Thread[] consumers = new Thread[numConsumers];
        for (int i = 0; i < numConsumers; i++)
        {
            consumers[i] = new Thread(Consumer);
            consumers[i].Start(itemsPerConsumer);
        }

        // Чекаємо завершення всіх потоків
        foreach (var producer in producers)
            producer.Join();
        foreach (var consumer in consumers)
            consumer.Join();

        Console.WriteLine("Всі виробники і споживачі завершили роботу.");
    }
}
