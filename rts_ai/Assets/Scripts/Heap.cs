using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// generic heap class
public class Heap<T> where T : IHeapItem<T>
{
    T[] items;
    int currentItemCount;

    // initialize heap with grid size
    public Heap(int maxHeapSize)
    {
        items = new T[maxHeapSize];
    }
    // sets items heapindex, adds it to the heap and sorts it
    public void AddToHeap(T item)
    {
        item.HeapIndex = currentItemCount;
        items[currentItemCount] = item;
        SortUp(item);
        currentItemCount++;
    }
    // sort an item to the correct location on the heap
    private void SortUp(T item)
    {
        int parentIndex = (item.HeapIndex-1)/ 2;
        // get items parent and swaps it if parent is larger
        while(true)
        {
            T parentItem = items[parentIndex];
            if(item.CompareTo(parentItem) > 0 )
            {
                SwapItems(item, parentItem);
            }
            else
            {
                break;
            }
            parentIndex = (item.HeapIndex-1)/ 2;
        }
    }
    // swaps 2 items on the heap
    private void SwapItems(T itemA, T itemB)
    {
        items[itemA.HeapIndex] = itemB;
        items[itemB.HeapIndex] = itemA;
        int itemAIndexTemp = itemA.HeapIndex;
        itemA.HeapIndex = itemB.HeapIndex;
        itemB.HeapIndex = itemAIndexTemp;
    }
    // remove and return root of the heap
    public T RemoveFirstHeapItem()
    {
        T firstItem = items[0];
        currentItemCount--;
        items[0] = items[currentItemCount];
        items[0].HeapIndex = 0;
        SortDown(items[0]);
        return firstItem;
    }
    public int GetItemCount()
    {
        return currentItemCount;
    }
    public bool ContainsItem(T item)
    {
        return Equals(items[item.HeapIndex],item);
    }
    public void UpdateItem(T item)
    {
        SortUp(item);
    }
    private void SortDown(T item)
    {
        while(true)
        {
            int childIndexLeft = item.HeapIndex * 2 + 1;
            int childIndexRight = item.HeapIndex * 2 + 2;
            int swapIndex = 0;
            if(childIndexLeft < currentItemCount)
            {
                swapIndex = childIndexLeft;

                if(childIndexRight < currentItemCount)
                {
                    if (items[childIndexLeft].CompareTo(items[childIndexRight]) < 0)
                    {
                        swapIndex = childIndexRight;
                    }
                }
                if (item.CompareTo(items[swapIndex]) < 0)
                {
                    SwapItems(item, items[swapIndex]);
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }
        }
    }
}
// interface for items that can be added to the heap
public interface IHeapItem<T> : IComparable<T>
{
    int HeapIndex { get; set; }
}
