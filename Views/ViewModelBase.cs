﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using Morganizer.Core;

namespace Morganizer.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        private readonly Dictionary<string, PropertyChangedEventArgs> _eventArgsCache;

        protected ViewModelBase()
        {
            _eventArgsCache = new Dictionary<string, PropertyChangedEventArgs>();
        }

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventArgs args;
            if (!_eventArgsCache.TryGetValue(propertyName, out args))
            {
                args = new PropertyChangedEventArgs(propertyName);
                _eventArgsCache.Add(propertyName, args);
            }
            OnPropertyChanged(args);
        }
        protected virtual void OnPropertyChanged(Expression<Func<object>> expression)
        {
            this.PropertyChanged.Raise(this, expression);
        }

        protected void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, args);
        }

        #endregion
    }
}
