package org.example;

import java.util.LinkedList;
import java.util.Queue;
import java.util.Random;
import java.util.concurrent.Semaphore;

public class Main {
    private static final int BUFFER_CAPACITY = 5;


    private static final Semaphore empty = new Semaphore(BUFFER_CAPACITY);
    private static final Semaphore full = new Semaphore(0);
    private static final Semaphore mutex = new Semaphore(1);


    private static final Queue<Integer> buffer = new LinkedList<>();
    private static int productId = 0;


    static class Producer extends Thread {
        private final int itemsToProduce;

        public Producer(int itemsToProduce) {
            this.itemsToProduce = itemsToProduce;
        }

        @Override
        public void run() {
            try {
                for (int i = 0; i < itemsToProduce; i++) {
                    empty.acquire();
                    mutex.acquire();

                    int product = ++productId;
                    buffer.add(product);
                    System.out.println("Producer " + getId() + " produced item " + product);

                    mutex.release();
                    full.release();

                    Thread.sleep(new Random().nextInt(200) + 100);
                }
            } catch (InterruptedException e) {
                e.printStackTrace();
            }
        }
    }

    static class Consumer extends Thread {
        private final int itemsToConsume;

        public Consumer(int itemsToConsume) {
            this.itemsToConsume = itemsToConsume;
        }

        @Override
        public void run() {
            try {
                for (int i = 0; i < itemsToConsume; i++) {
                    full.acquire();
                    mutex.acquire();

                    int product = buffer.poll();
                    System.out.println("Consumer " + getId() + " consumed item " + product);

                    mutex.release();
                    empty.release();

                    Thread.sleep(new Random().nextInt(200) + 100); // simulate work
                }
            } catch (InterruptedException e) {
                e.printStackTrace();
            }
        }
    }

    public static void main(String[] args) {
        int numProducers = 3;
        int numConsumers = 2;
        int totalItems = 15;

        int itemsPerProducer = totalItems / numProducers;
        int itemsPerConsumer = totalItems / numConsumers;

        Thread[] producers = new Thread[numProducers];
        for (int i = 0; i < numProducers; i++) {
            producers[i] = new Producer(itemsPerProducer);
            producers[i].start();
        }

        Thread[] consumers = new Thread[numConsumers];
        for (int i = 0; i < numConsumers; i++) {
            consumers[i] = new Consumer(itemsPerConsumer);
            consumers[i].start();
        }

        try {
            for (Thread p : producers) p.join();
            for (Thread c : consumers) c.join();
        } catch (InterruptedException e) {
            e.printStackTrace();
        }

    }
}
