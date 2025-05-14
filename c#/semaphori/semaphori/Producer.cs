using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace semaphori
{
    public class Producer
    {
        private int numItemsToProduce;
        private Storage storage;
        private int producerId;

        public Producer(int producerId, Storage storage, int numItemsToProduce)
        {
            this.numItemsToProduce = numItemsToProduce;
            this.storage = storage;
            this.producerId = producerId;

            Thread workerThread = new Thread(RunProduction);
            workerThread.Start();
        }

        private void RunProduction()
        {
            for (int i = 0; i < numItemsToProduce; i++)
            {
                storage.ProduceItem(producerId, i);
            }
            Console.WriteLine($"Producer {producerId} finished production.");
        }
    }
}