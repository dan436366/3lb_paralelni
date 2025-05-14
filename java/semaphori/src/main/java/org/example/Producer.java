package org.example;

public class Producer implements Runnable {
    private int numItemsToProduce;
    private Storage storage;
    private int producerId;
    private Thread thread;

    public Producer(int producerId, Storage storage, int numItemsToProduce) {
        this.numItemsToProduce = numItemsToProduce;
        this.storage = storage;
        this.producerId = producerId;

        this.thread = new Thread(this);
        this.thread.start();
    }

    @Override
    public void run() {
        for (int i = 0; i < numItemsToProduce; i++) {
            storage.produceItem(producerId, i);
        }
        System.out.println("Producer " + producerId + " finished production.");
    }
}