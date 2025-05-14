using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace semaphori
{
    public class Program
    {
        public static void StartSimulation(Storage storage)
        {
            int[] consumerWorkloads = { 6, 3, 7, 8, 4, 2 }; 
            int[] producerWorkloads = { 8, 12, 10 };

            int totalProduced = producerWorkloads.Sum();
            int totalConsumed = consumerWorkloads.Sum();

            if (totalProduced != totalConsumed)
            {
                Console.WriteLine("Number of consumers and producers isn't same");
                return;
            }

            for (int i = 0; i < producerWorkloads.Length; i++)
            {
                new Producer(i, storage, producerWorkloads[i]);
            }

            for (int i = 0; i < consumerWorkloads.Length; i++)
            {
                new Consumer(i, storage, consumerWorkloads[i]);
            }
        }

        public static void Main(string[] args)
        {
            int storageCapacity = 10;

            Storage storage = new Storage(storageCapacity);

            StartSimulation(storage);
        }
    }
}