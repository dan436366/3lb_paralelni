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
        private readonly int _id;
        private readonly SharedStorage _storage;
        private readonly int _itemsToProduceTotal;
        private int _itemsProduced = 0;

        public Producer(int id, SharedStorage storage, int itemsToProduceTotal)
        {
            _id = id;
            _storage = storage;
            _itemsToProduceTotal = itemsToProduceTotal;
        }

        public void Run()
        {
            Console.WriteLine($"Producer {_id} is starting work. Planned items: {_itemsToProduceTotal}");

            while (_itemsProduced < _itemsToProduceTotal)
            {
                _storage.Produce();
                _itemsProduced++;

                Thread.Sleep(new Random().Next(50, 200));
            }

            Console.WriteLine($"Producer {_id} has finished work. Items produced: {_itemsProduced}");
        }
    }
}
