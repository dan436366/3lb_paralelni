package org.example;

import java.util.Random;

public class Producer implements Runnable {
    private final int id;
    private final Storage storage;
    private final int itemsToProduceTotal;
    private final Random random = new Random();

    public Producer(int id, Storage storage, int itemsToProduceTotal) {
        this.id = id;
        this.storage = storage;
        this.itemsToProduceTotal = itemsToProduceTotal;
    }

    @Override
    public void run() {
        System.out.println("Producer " + id + " starts. Planned items: " + itemsToProduceTotal);

        int producedItems = 0;

        try {
            while (producedItems < itemsToProduceTotal) {
                storage.produce();
                producedItems++;

                Thread.sleep(random.nextInt(150) + 50);
            }
        } catch (InterruptedException e) {
            Thread.currentThread().interrupt();
            System.out.println("Producer " + id + " was interrupted");
        }

        System.out.println("Producer " + id + " finished. Items produced: " + producedItems);
    }
}