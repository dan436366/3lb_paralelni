using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace semaphori
{
    public class Storage
    {
        private Semaphore accessMutex;     
        private Semaphore itemAvailable;   
        private Semaphore spaceAvailable;  

        private List<string> itemCollection; 

        public Storage(int maxSize)
        {
            itemCollection = new List<string>(maxSize);

            accessMutex = new Semaphore(1, 1);
            spaceAvailable = new Semaphore(maxSize, maxSize);
            itemAvailable = new Semaphore(0, maxSize);
        }

        public void ProduceItem(int producerId, int itemId)
        {
            Console.WriteLine($"Producer {producerId} waiting at storage entrance.");
            Console.WriteLine($"Producer {producerId} checking for available space.");

            spaceAvailable.WaitOne();

            Console.WriteLine($"Producer {producerId} found space for new item.");
            Console.WriteLine($"Producer {producerId} waiting for storage access permission.");
            accessMutex.WaitOne();

            Console.WriteLine($"Producer {producerId} entered storage area.");

            string newItemName = $"{producerId}_{itemId}";
            AddItem(newItemName);

            Console.WriteLine($"Producer {producerId} added item {newItemName} to storage.");
            Console.WriteLine($"Producer {producerId} leaving storage area.");

            accessMutex.Release();

            Console.WriteLine($"Producer {producerId} signaling that new item is available.");

            itemAvailable.Release();
        }

        public void ConsumeItem(int consumerId)
        {
            Console.WriteLine($"Consumer {consumerId} waiting at storage entrance.");
            Console.WriteLine($"Consumer {consumerId} checking for available items.");

            itemAvailable.WaitOne();

            Console.WriteLine($"Consumer {consumerId} found an item to take.");
            Console.WriteLine($"Consumer {consumerId} waiting for storage access permission.");

            accessMutex.WaitOne();

            Console.WriteLine($"Consumer {consumerId} entered storage area.");

            string takenItemName = RemoveItem();

            Console.WriteLine($"Consumer {consumerId} took item {takenItemName} from storage.");

            accessMutex.Release();

            Console.WriteLine($"Consumer {consumerId} leaving storage area.");

            spaceAvailable.Release();

            Console.WriteLine($"Consumer {consumerId} signaling that space is now available.");
        }

        private void AddItem(string itemName)
        {
            itemCollection.Add(itemName);
        }

        private string RemoveItem()
        {
            var itemName = itemCollection[0];
            itemCollection.RemoveAt(0);
            return itemName;
        }
    }
}