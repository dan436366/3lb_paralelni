package org.example;

import java.util.Random;

public class Consumer implements Runnable {
    private final int id;
    private final Storage storage;
    private final int itemsToConsumeTotal;
    private final Random random = new Random();

    public Consumer(int id, Storage storage, int itemsToConsumeTotal) {
        this.id = id;
        this.storage = storage;
        this.itemsToConsumeTotal = itemsToConsumeTotal;
    }

    @Override
    public void run() {
        System.out.println("Consumer " + id + " starts. Planned items: " + itemsToConsumeTotal);

        int consumedItems = 0;

        try {
            while (consumedItems < itemsToConsumeTotal) {
                int item = storage.consume();
                consumedItems++;

                Thread.sleep(random.nextInt(200) + 100);
            }
        } catch (InterruptedException e) {
            Thread.currentThread().interrupt();
            System.out.println("Consumer " + id + " was interrupted");
        }

        System.out.println("Consumer " + id + " finished. Items consumed: " + consumedItems);
    }
}