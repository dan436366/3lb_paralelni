using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace semaphori
{
    public class Consumer
    {
        private int numItemsToConsume;
        private Storage storage;
        private int consumerId;

        public Consumer(int consumerId, Storage storage, int numItemsToConsume)
        {
            this.numItemsToConsume = numItemsToConsume;
            this.storage = storage;
            this.consumerId = consumerId;

            Thread workerThread = new Thread(RunConsumption);
            workerThread.Start();
        }

        private void RunConsumption()
        {
            for (int i = 0; i < numItemsToConsume; i++)
            {
                storage.ConsumeItem(consumerId);
            }
            Console.WriteLine($"Consumer {consumerId} finished consumption.");
        }
    }
}