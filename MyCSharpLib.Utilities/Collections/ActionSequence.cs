using System;
using System.Collections;
using System.Collections.Generic;

namespace MyCSharpLib.Utilities.Collections
{
    public class ActionSequence<TKey> : IDictionary<TKey, ActionSequenceAction<TKey>>, IEnumerable<ActionSequenceAction<TKey>>
        where TKey : IEquatable<TKey>
    {
        #region FIELDS

        private readonly object _lock = new object();
        private readonly IDictionary<TKey, ActionSequenceAction<TKey>> _actions = new Dictionary<TKey, ActionSequenceAction<TKey>>();

        private int _version;

        #endregion FIELDS


        #region PROPERTIES

        public TKey Start { get; set; }
        public TKey End { get; set; }

        #endregion PROPERTIES


        #region METHODS

        public void Execute()
        {
            var enumerator = GetEnumerator();
            while (enumerator.MoveNext()) { }
        }

        public ActionSequence<TKey> BeforStart(ActionSequenceLifeCycleEventHandler<TKey> action)
        {
            lock (_lock)
            {
                SequenceStarted += action;
                return this;
            }
        }
        public ActionSequence<TKey> BeforeStep(ActionSequenceLifeCycleEventHandler<TKey> action)
        {
            lock (_lock)
            {
                ExecutingAction += action;
                return this;
            }
        }
        public ActionSequence<TKey> AfterStep(ActionSequenceLifeCycleEventHandler<TKey> action)
        {
            lock (_lock)
            {
                ExecutedAction += action;
                return this;
            }
        }
        public ActionSequence<TKey> AfterEnd(ActionSequenceLifeCycleEventHandler<TKey> action)
        {
            lock (_lock)
            {
                SequenceEnded += action;
                return this;
            }
        }
       
        #endregion METHODS


        #region EVENTS

        public event ActionSequenceLifeCycleEventHandler<TKey> SequenceStarted;
        public event ActionSequenceLifeCycleEventHandler<TKey> ExecutingAction;
        public event ActionSequenceLifeCycleEventHandler<TKey> ExecutedAction;
        public event ActionSequenceLifeCycleEventHandler<TKey> SequenceEnded;

        #endregion EVENTS


        #region ENUMERABLE

        public IEnumerator<ActionSequenceAction<TKey>> GetEnumerator() => new ActionSequenceEnumerator(this);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion ENUMERABLE


        #region IDICTIONARY

        public ICollection<TKey> Keys => _actions.Keys;
        public ICollection<ActionSequenceAction<TKey>> Values => _actions.Values;
        public int Count => _actions.Count;
        public bool IsReadOnly => false;

        public ActionSequenceAction<TKey> this[TKey key]
        {
            get => _actions[key];
            set
            {
                if (ContainsKey(key))
                {
                    _actions[key] = value;
                    _version++;
                }
                else
                    Add(key, value);
            }
        }

        public bool ContainsKey(TKey key) => _actions.ContainsKey(key);
        public bool Contains(KeyValuePair<TKey, ActionSequenceAction<TKey>> item) => _actions.Contains(item);
        public bool TryGetValue(TKey key, out ActionSequenceAction<TKey> value) => _actions.TryGetValue(key, out value);

        public void Add(TKey key, ActionSequenceAction<TKey> value)
        {
            lock (_lock)
            {
                _actions.Add(key, value);
                _version++;
            }
        }

        public void Add(KeyValuePair<TKey, ActionSequenceAction<TKey>> item)
        {
            lock (_lock)
            {
                _actions.Add(item);
                _version++;
            }
        }

        public bool Remove(TKey key)
        {
            lock (_lock)
            {
                var removed = _actions.Remove(key);
                _version++;
                return removed;
            }
        }

        public bool Remove(KeyValuePair<TKey, ActionSequenceAction<TKey>> item)
        {
            lock (_lock)
            {
                var removed = _actions.Remove(item);
                _version++;
                return removed;
            }
        }

        public void Clear()
        {
            lock (_lock)
            {
                _actions.Clear();
                _version++;
            }
        }

        public void CopyTo(KeyValuePair<TKey, ActionSequenceAction<TKey>>[] array, int arrayIndex) => _actions.CopyTo(array, arrayIndex);

        IEnumerator<KeyValuePair<TKey, ActionSequenceAction<TKey>>> IEnumerable<KeyValuePair<TKey, ActionSequenceAction<TKey>>>.GetEnumerator() => _actions.GetEnumerator();

        #endregion IDICTIONARY


        #region CLASSES

        private struct ActionSequenceEnumerator : IEnumerator<ActionSequenceAction<TKey>>, IEnumerator
        {
            private readonly ActionSequence<TKey> _sequence;
            private readonly int _version;

            private TKey _key;

            public ActionSequenceEnumerator(ActionSequence<TKey> sequence)
            {
                _sequence = sequence;
                _version = sequence._version;

                _key = sequence.Start;
                Current = sequence[sequence.Start];
            }


            public ActionSequenceAction<TKey> Current { get; private set; }
            object IEnumerator.Current => Current;


            public void Dispose() { }

            public bool MoveNext()
            {
                lock (_sequence._lock)
                {
                    if (_sequence._version != _version)
                        throw new InvalidOperationException("The sequence changed while iterating over it...");

                    if (_key.Equals(_sequence.Start))
                        _sequence.SequenceStarted?.Invoke(_sequence, _key, Current);
                    
                    _sequence.ExecutingAction?.Invoke(_sequence, _key, Current);
                    Current.Execute();
                    _sequence.ExecutedAction?.Invoke(_sequence, _key, Current);

                    if (_key.Equals(_sequence.End))
                    {
                        _sequence.SequenceEnded?.Invoke(_sequence, _key, Current);
                        return false;
                    }

                    _key = Current.NextAction;
                    Current = _sequence[_key];
                    return true;
                }
            }

            public void Reset()
            {
                _key = _sequence.Start;
                Current = _sequence[_key];
            }
        }

        #endregion CLASSES
    }
}
