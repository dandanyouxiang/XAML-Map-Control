﻿// XAML Map Control - https://github.com/ClemensFischer/XAML-Map-Control
// © 2018 Clemens Fischer
// Licensed under the Microsoft Public License (Ms-PL)

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;

namespace MapControl
{
    /// <summary>
    /// An ObservableCollection of IEnumerable of Location. PolygonCollection adds a CollectionChanged
    /// listener to each element that implements INotifyCollectionChanged and, when such an element changes,
    /// fires its own CollectionChanged event with NotifyCollectionChangedAction.Replace for that element.
    /// </summary>
    public class PolygonCollection : ObservableCollection<IEnumerable<Location>>, IWeakEventListener
    {
        protected override void InsertItem(int index, IEnumerable<Location> polygon)
        {
            var observablePolygon = polygon as INotifyCollectionChanged;

            if (observablePolygon != null)
            {
                CollectionChangedEventManager.AddListener(observablePolygon, this);
            }

            base.InsertItem(index, polygon);
        }

        protected override void SetItem(int index, IEnumerable<Location> polygon)
        {
            var observablePolygon = this[index] as INotifyCollectionChanged;

            if (observablePolygon != null)
            {
                CollectionChangedEventManager.RemoveListener(observablePolygon, this);
            }

            base.SetItem(index, polygon);
        }

        protected override void RemoveItem(int index)
        {
            var observablePolygon = this[index] as INotifyCollectionChanged;

            if (observablePolygon != null)
            {
                CollectionChangedEventManager.RemoveListener(observablePolygon, this);
            }

            base.RemoveItem(index);
        }

        protected override void ClearItems()
        {
            foreach (var observablePolygon in this.OfType<INotifyCollectionChanged>())
            {
                CollectionChangedEventManager.RemoveListener(observablePolygon, this);
            }

            base.ClearItems();
        }

        bool IWeakEventListener.ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, sender, sender));

            return true;
        }
    }
}
