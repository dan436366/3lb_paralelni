package org.example;

public class Consumer implements Runnable {
    private int numItemsToConsume;
    private Storage storage;
    private int consumerId;
    private Thread thread;

    public Consumer(int consumerId, Storage storage, int numItemsToConsume) {
        this.numItemsToConsume = numItemsToConsume;
        this.storage = storage;
        this.consumerId = consumerId;

        this.thread = new Thread(this);
        this.thread.start();
    }

    @Override
    public void run() {
        for (int i = 0; i < numItemsToConsume; i++) {
            storage.consumeItem(consumerId);
        }
        System.out.println("Consumer " + consumerId + " finished consumption.");
    }
}