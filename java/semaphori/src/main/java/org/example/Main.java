package org.example;

import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;
import java.util.concurrent.TimeUnit;

public class Main {
    public static void main(String[] args) {

        final int STORAGE_CAPACITY = 5;
        final int TOTAL_ITEMS = 20;
        final int PRODUCER_COUNT = 3;
        final int CONSUMER_COUNT = 2;

        System.out.println("=== Producer-Consumer Problem with Multiple Producers and Consumers ===");
        System.out.println("Storage capacity: " + STORAGE_CAPACITY);
        System.out.println("Total number of items: " + TOTAL_ITEMS);
        System.out.println("Number of producers: " + PRODUCER_COUNT);
        System.out.println("Number of consumers: " + CONSUMER_COUNT);
        System.out.println();


        Storage storage = new Storage(STORAGE_CAPACITY);


        int[] producerWorkload = distributeItems(TOTAL_ITEMS, PRODUCER_COUNT);
        int[] consumerWorkload = distributeItems(TOTAL_ITEMS, CONSUMER_COUNT);


        ExecutorService executorService = Executors.newFixedThreadPool(PRODUCER_COUNT + CONSUMER_COUNT);


        for (int i = 0; i < PRODUCER_COUNT; i++) {
            final int producerId = i + 1;
            final int itemsToProduce = producerWorkload[i];
            executorService.submit(() -> {
                Producer producer = new Producer(producerId, storage, itemsToProduce);
                producer.run();
                return null;
            });
        }


        for (int i = 0; i < CONSUMER_COUNT; i++) {
            final int consumerId = i + 1;
            final int itemsToConsume = consumerWorkload[i];
            executorService.submit(() -> {
                Consumer consumer = new Consumer(consumerId, storage, itemsToConsume);
                consumer.run();
                return null;
            });
        }

        executorService.shutdown();
        try {
            executorService.awaitTermination(Long.MAX_VALUE, TimeUnit.NANOSECONDS);
        } catch (InterruptedException e) {
            e.printStackTrace();
        }

        System.out.println("\nWork completed! Total produced/consumed: " + storage.getTotalProcessed());
    }

    private static int[] distributeItems(int totalItems, int count) {
        int[] distribution = new int[count];
        int baseCount = totalItems / count;
        int remainder = totalItems % count;

        for (int i = 0; i < count; i++) {
            distribution[i] = baseCount;
            if (i < remainder) {
                distribution[i]++;
            }
        }

        return distribution;
    }
}