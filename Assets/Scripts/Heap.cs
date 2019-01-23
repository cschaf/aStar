using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Die Heap Klasse.
/// Sie ist nach der Datenstrukur eines Binary Search Tree aufgebaut.
/// Damit lassen sich schneller Objekte finden oder sortieren
/// </summary>
public class Heap<T> where T: IHeapItem<T>{
	protected T[] items; // Items im Heap
	protected int currentItemCount; // Anzahl der Items
	
	public Heap(int maxHeapSize){
		items = new T[maxHeapSize];
		currentItemCount = 0;
	}
	
  /// <summary>
  /// Füge ein Item ans Ende des Heaps und sortiere es ein
  /// </summary>
  /// <param name="item">Item</param>
	public void Add(T item){
		item.HeapIndex = currentItemCount;
		items[currentItemCount] = item;
		SortUp(item);
		currentItemCount++;
	}
	/// <summary>
  /// Entferne das erste Item und sortieren den Heap neu
  /// </summary>
  /// <returns></returns>
	public T RemoveFirst(){
		T firstItem = items[0];
		currentItemCount--;
		items[0] = items[currentItemCount];
		items[0].HeapIndex = 0;
		SortDown(items[0]);
		return firstItem;
	}
	/// <summary>
  /// Sortiere ein Item von unten nach oben im Heap
  /// </summary>
  /// <param name="item">Item</param>
	private void SortUp(T item){
		int parentIndex = (item.HeapIndex-1)/2;
		
		while(true){
			T parentItem = items[parentIndex];
			if(item.CompareTo(parentItem) > 0){
				Swap(item, parentItem);
			}else{
				break;
			}
			
			parentIndex = (item.HeapIndex-1)/2;
		}
	}
	/// <summary>
  /// Sortiere ein Item von oben nach unten im Heap
  /// </summary>
  /// <param name="item"></param>
	private void SortDown(T item){
		while(true){
			int childIndexLeft = item.HeapIndex * 2 + 1;
			int childIndexRight = item.HeapIndex * 2 + 2;
			int swapIndex = 0;
			
			if(childIndexLeft < currentItemCount){
				swapIndex = childIndexLeft;
				if(childIndexRight < currentItemCount){
					if(items[childIndexLeft].CompareTo(items[childIndexRight]) < 0){
						swapIndex = childIndexRight;
					}
				}
				if(item.CompareTo(items[swapIndex]) <0){
					Swap(item, items[swapIndex]);
				}
				else{
					return;
				}
			}
			else{
				return;
			}
		}
	}
	/// <summary>
  /// Aktualisiere ein Item wen ein neuer Pfad mit niedriegeren Kosten gefunden wurde
  /// </summary>
  /// <param name="item">Item</param>
	public void UpdateItem(T item){
		SortUp(item);
	}
	/// <summary>
  /// Gibt die Anzahl der Items zurück
  /// </summary>
	public int Count{
		get{
			return currentItemCount;
		}
	}
	/// <summary>
  /// Prüft ob sich ein bestimmtes Item im Heap befindet
  /// </summary>
  /// <param name="item">Item</param>
  /// <returns></returns>
	public bool Contains(T item){
		return Equals(items[item.HeapIndex], item);
	}
	/// <summary>
  /// Tausche Item A mit Item B
  /// </summary>
  /// <param name="itemA"></param>
  /// <param name="itemB"></param>
	private void Swap(T itemA, T itemB){
		items[itemA.HeapIndex] = itemB;
		items[itemB.HeapIndex] = itemA;
		
		int itemAIndex = itemA.HeapIndex;
		itemA.HeapIndex = itemB.HeapIndex;
		itemB.HeapIndex = itemAIndex;
	}
}
/// <summary>
/// Interface für die Node, damit man sie miteinander Vergleichen kann
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IHeapItem<T>:IComparable<T>{
	int HeapIndex {
		get;
		set;
	}
}
