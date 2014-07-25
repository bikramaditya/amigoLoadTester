// -----------------------------------------------------------------------
// <copyright file="Class1.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Amigo
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class NotifiableObservableCollection
    {
    }

    public class NotifiableObservableCollection<T> : ObservableCollection<T>
    {
        #region DoNotifyCollectionChanged
        /// <summary>
        /// Use this method to foce a reset operation on any views bound to this collection
        /// </summary>
        public void DoNotifyCollectionChanged()
        {
            base.OnCollectionChanged(
                new System.Collections.Specialized.NotifyCollectionChangedEventArgs(
                    System.Collections.Specialized.NotifyCollectionChangedAction.Reset));
        }
        #endregion

        #region DoNotifyCollectionChangeRemove
        /// <summary>
        /// Use this method to force any views bound to this collection to look for a specific
        /// item as deleted..use after changing any property that would cause the object to be filtered.
        /// </summary>
        /// <param name="obj"></param>
        public void DoNotifyCollectionChangeRemove(T obj, int index)
        {
            base.OnCollectionChanged(
                new System.Collections.Specialized.NotifyCollectionChangedEventArgs(
                    System.Collections.Specialized.NotifyCollectionChangedAction.Remove, obj, index));
        }
        #endregion

        #region DoNotifyCollectionChangeAdd
        /// <summary>
        /// Use this method to force any views bound to this collection to look for a specific
        /// item as inserted, use after any operation that should cause new items to appear
        /// </summary>
        /// <param name="obj"></param>
        public void DoNotifyCollectionChangeAdd(T obj, int index)
        {
            base.OnCollectionChanged(
                new System.Collections.Specialized.NotifyCollectionChangedEventArgs(
                System.Collections.Specialized.NotifyCollectionChangedAction.Add, obj, index));
        }
        #endregion
    }
}
