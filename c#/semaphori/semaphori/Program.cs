using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace semaphori
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Producer-Consumer Problem with Multiple Producers and Consumers ===");

            int storageCapacity = 5;        
            int totalItemsToProcess = 20;    
            int producerCount = 3;       
            int consumerCount = 2;          

            SharedStorage storage = new SharedStorage(storageCapacity);

            int[] producerWorkload = DistributeItems(totalItemsToProcess, producerCount);
            int[] consumerWorkload = DistributeItems(totalItemsToProcess, consumerCount);

            List<Task> tasks = new List<Task>();
            for (int i = 0; i < producerCount; i++)
            {
                int producerId = i + 1;
                int itemsToProduceForThisProducer = producerWorkload[i];
                tasks.Add(Task.Run(() =>
                {
                    var producer = new Producer(producerId, storage, itemsToProduceForThisProducer);
                    producer.Run();
                }));
            }

            for (int i = 0; i < consumerCount; i++)
            {
                int consumerId = i + 1;
                int itemsToConsumeForThisConsumer = consumerWorkload[i];
                tasks.Add(Task.Run(() =>
                {
                    var consumer = new Consumer(consumerId, storage, itemsToConsumeForThisConsumer);
                    consumer.Run();
                }));
            }

            Task.WaitAll(tasks.ToArray());

            Console.WriteLine("\nWork completed! Total produced/consumed: " + storage.TotalProcessed);
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        static int[] DistributeItems(int totalItems, int participantCount)
        {
            int[] distribution = new int[participantCount];
            int baseCount = totalItems / participantCount;
            int remainder = totalItems % participantCount;

            for (int i = 0; i < participantCount; i++)
            {
                distribution[i] = baseCount;
                if (i < remainder) distribution[i]++;
            }

            return distribution;
        }
    }
}
