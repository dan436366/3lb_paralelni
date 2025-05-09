using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace semaphori
{
    public class SharedStorage
    {
        private readonly Queue<int> _items = new Queue<int>();
        private readonly int _capacity;
        private int _nextItemId = 1;
        private int _totalProcessed = 0;

        private readonly Semaphore _producerSemaphore;
        private readonly Semaphore _consumerSemaphore;
        private readonly Semaphore _storageSemaphore;

        public int TotalProcessed => _totalProcessed;

        public SharedStorage(int capacity)
        {
            _capacity = capacity;

            _producerSemaphore = new Semaphore(1, 1);    
            _consumerSemaphore = new Semaphore(1, 1);     
            _storageSemaphore = new Semaphore(1, 1);     
        }

        public void Produce()
        {
            _producerSemaphore.WaitOne();

            try
            {
                while (_items.Count >= _capacity)
                {
                    _producerSemaphore.Release();
                    Thread.Sleep(50);
                    _producerSemaphore.WaitOne();
                }

                _storageSemaphore.WaitOne();
                try
                {
                    int itemId = _nextItemId++;
                    _items.Enqueue(itemId);
                    Console.WriteLine($"Produced: Item {itemId}, storage now has {_items.Count} items");
                }
                finally
                {
                    _storageSemaphore.Release();
                }
            }
            finally
            {
                _producerSemaphore.Release();
            }
        }

        public int Consume()
        {
            _consumerSemaphore.WaitOne();

            try
            {
                while (_items.Count == 0)
                {
                    _consumerSemaphore.Release();
                    Thread.Sleep(50);
                    _consumerSemaphore.WaitOne();
                }

                _storageSemaphore.WaitOne();
                try
                {
                    int item = _items.Dequeue();
                    _totalProcessed++;
                    Console.WriteLine($"Consumed: Item {item}, storage now has {_items.Count} items");
                    return item;
                }
                finally
                {
                    _storageSemaphore.Release();
                }
            }
            finally
            {
                _consumerSemaphore.Release();
            }
        }
    }
}
