using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatencyMonitor
{
    public class ObservableMembersCollection<T> : ObservableCollection<T> where T : INotifyPropertyChanged
    {
        public void  InstertAndWatch(T t)
        {
            //This extension method ties item property changed to collection changed.
            //To ensure that the CollectionChanged event is fired when any member is chaged
            this.Add(t);
            t.PropertyChanged += (s, e) => 
            {
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, t));
            };
            
        }
    }
}
