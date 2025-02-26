using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LessonHome3 : MonoBehaviour
{
   [SerializeField] private string _value;
   [SerializeField] private List<string> _list;

   [ContextMenu("Print")]
   private void Print()
   {
      string msg = "List: ";
      for (int i = 0; i < _list.Count; ++i)
         msg += $"\n{_list[i]}";
      Debug.Log(msg);
   }
   
   [ContextMenu("Add")]
   private void Add()
   {
      _list.Add(_value);
   }
   
   [ContextMenu("Remove")]
   private void Remove()
   {
      _list.Remove(_value);
   }
   
   [ContextMenu("Clear")]
   private void Clear()
   {
      _list.Clear();
   }
   [ContextMenu("SortDefault")]
   private void SortDefault()
   {
      _list.Sort();
   }
   
   [ContextMenu("SortAscending")]
   private void SortAscending()
   {
      _list.Sort();
      _list.Sort((a, b) => a.Length.CompareTo(b.Length));

   }
   
   [ContextMenu("SortDescending")]
   private void SortDescending()
   {
      _list.Sort((a, b) => 
      {
         int lengthComparison = b.Length.CompareTo(a.Length); 
         return lengthComparison != 0 ? lengthComparison : b.CompareTo(a);
      });
   }
   
   
}
