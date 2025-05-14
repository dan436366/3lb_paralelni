package org.example;

import java.util.Arrays;

public class Main {
    public static void startSimulation(Storage storage) {
        int[] consumerWorkloads = { 6, 3, 7, 8, 4, 2 };
        int[] producerWorkloads = { 8, 12, 10 };

        int totalProduced = Arrays.stream(producerWorkloads).sum();
        int totalConsumed = Arrays.stream(consumerWorkloads).sum();

        if (totalProduced != totalConsumed) {
            System.out.println("Number of consumers and producers isn't same");
            return;
        }

        for (int i = 0; i < producerWorkloads.length; i++) {
            new Producer(i, storage, producerWorkloads[i]);
        }

        for (int i = 0; i < consumerWorkloads.length; i++) {
            new Consumer(i, storage, consumerWorkloads[i]);
        }
    }

    public static void main(String[] args) {
        int storageCapacity = 10;

        Storage storage = new Storage(storageCapacity);

        startSimulation(storage);
    }
}