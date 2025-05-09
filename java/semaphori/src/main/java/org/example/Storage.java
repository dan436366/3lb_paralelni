package org.example;
import java.util.LinkedList;
import java.util.Queue;
import java.util.concurrent.Semaphore;
import java.util.concurrent.atomic.AtomicInteger;


public class Storage {
    private final Queue<Integer> items = new LinkedList<>();
    private final int capacity;
    private final AtomicInteger nextItemId = new AtomicInteger(1);
    private final AtomicInteger totalProcessed = new AtomicInteger(0);

    private final Semaphore mutex = new Semaphore(1);
    private final Semaphore emptySlots;
    private final Semaphore fullSlots = new Semaphore(0);

    public Storage(int capacity) {
        this.capacity = capacity;
        this.emptySlots = new Semaphore(capacity);
    }


    public void produce() throws InterruptedException {
        emptySlots.acquire();

        try {
            mutex.acquire();

            int itemId = nextItemId.getAndIncrement();
            items.add(itemId);
            System.out.println("Produced: Item " + itemId + ", in storage: " + items.size() + " of " + capacity);

            mutex.release();
        } finally {
            fullSlots.release();
        }
    }


    public int consume() throws InterruptedException {
        fullSlots.acquire();

        try {
            mutex.acquire();

            int item = items.poll();
            totalProcessed.incrementAndGet();
            System.out.println("Consumed: Item " + item + ", in storage: " + items.size() + " of " + capacity);

            mutex.release();
            return item;
        } finally {
            emptySlots.release();
        }
    }

    public int getTotalProcessed() {
        return totalProcessed.get();
    }
}