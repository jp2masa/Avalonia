using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

using Avalonia.Collections;

namespace Avalonia.Controls
{
    public abstract class ResourceDictionaryBase : IDictionary<object, object?>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        private readonly AvaloniaDictionary<object, object?> _inner = new AvaloniaDictionary<object, object?>();

        public object this[object key]
        {
            get => TryGetValue(key, out var value) ? value : throw new KeyNotFoundException();
            set => _inner[key] = value;
        }

        public ICollection<object> Keys => _inner.Keys;

        public ICollection<object> Values => throw new NotSupportedException();

        public int Count => _inner.Count;

        public bool IsReadOnly => false;

        public event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add => ((INotifyCollectionChanged)_inner).CollectionChanged += value;
            remove => ((INotifyCollectionChanged)_inner).CollectionChanged -= value;
        }

        public event PropertyChangedEventHandler PropertyChanged
        {
            add => ((INotifyPropertyChanged)_inner).PropertyChanged += value;
            remove => ((INotifyPropertyChanged)_inner).PropertyChanged -= value;
        }

        public void Add(object key, object value) => _inner.Add(key, value);

        public void Add(KeyValuePair<object, object> item) => Add(item.Key, item.Value);

        public void Clear() => _inner.Clear();

        public bool Contains(KeyValuePair<object, object> item) => throw new NotSupportedException();

        public bool ContainsKey(object key) => _inner.ContainsKey(key);

        public void CopyTo(KeyValuePair<object, object>[] array, int arrayIndex) => throw new NotSupportedException();

        public IEnumerator<KeyValuePair<object, object>> GetEnumerator() => throw new NotSupportedException();

        public bool Remove(object key) => _inner.Remove(key);

        public bool Remove(KeyValuePair<object, object> item) => throw new NotSupportedException();

        public bool TryGetValue(object key, out object value)
        {
            if (_inner.TryGetValue(key, out var x))
            {
                value = x is ResourceFactory factory ? factory.Factory(null) : x;
                return true;
            }

            value = default;
            return false;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
