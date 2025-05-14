package org.example;

import java.util.ArrayList;
import java.util.List;
import java.util.concurrent.Semaphore;

public class Storage {
    private Semaphore accessMutex;
    private Semaphore itemAvailable;
    private Semaphore spaceAvailable;

    private List<String> itemCollection;

    public Storage(int maxSize) {
        itemCollection = new ArrayList<>(maxSize);

        accessMutex = new Semaphore(1, true);
        spaceAvailable = new Semaphore(maxSize, true);
        itemAvailable = new Semaphore(0, true);
    }

    public void produceItem(int producerId, int itemId) {
        System.out.println("Producer " + producerId + " waiting at storage entrance.");
        System.out.println("Producer " + producerId + " checking for available space.");

        try {
            spaceAvailable.acquire();

            System.out.println("Producer " + producerId + " found space for new item.");
            System.out.println("Producer " + producerId + " waiting for storage access permission.");

            accessMutex.acquire();

            System.out.println("Producer " + producerId + " entered storage area.");

            String newItemName = producerId + "_" + itemId;
            addItem(newItemName);

            System.out.println("Producer " + producerId + " added item " + newItemName + " to storage.");
            System.out.println("Producer " + producerId + " leaving storage area.");

            accessMutex.release();

            System.out.println("Producer " + producerId + " signaling that new item is available.");

            itemAvailable.release();
        } catch (InterruptedException e) {
            Thread.currentThread().interrupt();
            System.out.println("Producer " + producerId + " was interrupted: " + e.getMessage());
        }
    }

    public void consumeItem(int consumerId) {
        System.out.println("Consumer " + consumerId + " waiting at storage entrance.");
        System.out.println("Consumer " + consumerId + " checking for available items.");

        try {
            itemAvailable.acquire();

            System.out.println("Consumer " + consumerId + " found an item to take.");
            System.out.println("Consumer " + consumerId + " waiting for storage access permission.");

            accessMutex.acquire();

            System.out.println("Consumer " + consumerId + " entered storage area.");

            String takenItemName = removeItem();

            System.out.println("Consumer " + consumerId + " took item " + takenItemName + " from storage.");

            accessMutex.release();

            System.out.println("Consumer " + consumerId + " leaving storage area.");

            spaceAvailable.release();

            System.out.println("Consumer " + consumerId + " signaling that space is now available.");
        } catch (InterruptedException e) {
            Thread.currentThread().interrupt();
            System.out.println("Consumer " + consumerId + " was interrupted: " + e.getMessage());
        }
    }

    private void addItem(String itemName) {
        itemCollection.add(itemName);
    }

    private String removeItem() {
        String itemName = itemCollection.get(0);
        itemCollection.remove(0);
        return itemName;
    }
}