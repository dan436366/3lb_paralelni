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
        private readonly int _id;
        private readonly SharedStorage _storage;
        private readonly int _itemsToConsumeTotal;
        private int _itemsConsumed = 0;

        public Consumer(int id, SharedStorage storage, int itemsToConsumeTotal)
        {
            _id = id;
            _storage = storage;
            _itemsToConsumeTotal = itemsToConsumeTotal;
        }

        public void Run()
        {
            Console.WriteLine($"Consumer {_id} is starting work. Planned items: {_itemsToConsumeTotal}");

            while (_itemsConsumed < _itemsToConsumeTotal)
            {
                int item = _storage.Consume();
                _itemsConsumed++;

                Thread.Sleep(new Random().Next(100, 300));
            }

            Console.WriteLine($"Consumer {_id} has finished work. Items consumed: {_itemsConsumed}");
        }
    }
}
