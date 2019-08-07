using MyCSharpLib.Services.Logging;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace MyCSharpLib.Wpf.Controls
{
    public class LogsView : Control
    {
        #region DEPENDENCY PROPERTIES

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
            nameof(ItemsSource),
            typeof(IEnumerable<ILogEntry>),
            typeof(LogsView),
            new PropertyMetadata(default(IEnumerable<ILogEntry>), OnItemsSourceChanged));

        public static readonly DependencyProperty FilteredItemsSourceProperty = DependencyProperty.Register(
            nameof(FilteredItemsSource),
            typeof(IEnumerable<ILogEntry>),
            typeof(LogsView),
            new PropertyMetadata(default(IEnumerable<ILogEntry>), OnItemsSourceChanged));

        public static readonly DependencyProperty StringsProperty = DependencyProperty.Register(
            nameof(Strings),
            typeof(IControlsStrings),
            typeof(LogsView),
            new PropertyMetadata(new ControlStrings(), OnStringsChanged));

        #region collumn visibilities

        public static readonly DependencyProperty IdColumnVisibilityProperty = DependencyProperty.Register(
            nameof(IdColumnVisibility),
            typeof(Visibility),
            typeof(LogsView),
            new PropertyMetadata(Visibility.Collapsed, OnColumnVisibilityChanged));

        public static readonly DependencyProperty SourceColumnVisibilityProperty = DependencyProperty.Register(
            nameof(SourceColumnVisibility),
            typeof(Visibility),
            typeof(LogsView),
            new PropertyMetadata(Visibility.Visible, OnColumnVisibilityChanged));

        public static readonly DependencyProperty TimeColumnVisibilityProperty = DependencyProperty.Register(
            nameof(TimeColumnVisibility),
            typeof(Visibility),
            typeof(LogsView),
            new PropertyMetadata(Visibility.Visible, OnColumnVisibilityChanged));

        public static readonly DependencyProperty TagColumnVisibilityProperty = DependencyProperty.Register(
            nameof(TagColumnVisibility),
            typeof(Visibility),
            typeof(LogsView),
            new PropertyMetadata(Visibility.Visible, OnColumnVisibilityChanged));

        public static readonly DependencyProperty EventTypeColumnVisibilityProperty = DependencyProperty.Register(
            nameof(EventTypeColumnVisibility),
            typeof(Visibility),
            typeof(LogsView),
            new PropertyMetadata(Visibility.Visible, OnColumnVisibilityChanged));
        
        public static readonly DependencyProperty TitleColumnVisibilityProperty = DependencyProperty.Register(
            nameof(TitleColumnVisibility),
            typeof(Visibility),
            typeof(LogsView),
            new PropertyMetadata(Visibility.Visible, OnColumnVisibilityChanged));
        
        public static readonly DependencyProperty PayloadColumnVisibilityProperty = DependencyProperty.Register(
            nameof(PayloadColumnVisibility),
            typeof(Visibility),
            typeof(LogsView),
            new PropertyMetadata(Visibility.Visible, OnColumnVisibilityChanged));

        public static readonly DependencyProperty OperationStackColumnVisibilityProperty = DependencyProperty.Register(
           nameof(OperationStackColumnVisibility),
           typeof(Visibility),
           typeof(LogsView),
           new PropertyMetadata(Visibility.Collapsed, OnColumnVisibilityChanged));

        public static readonly DependencyProperty TimeStampColumnVisibilityProperty = DependencyProperty.Register(
           nameof(TimeStampColumnVisibility),
           typeof(Visibility),
           typeof(LogsView),
           new PropertyMetadata(Visibility.Collapsed, OnColumnVisibilityChanged));

        public static readonly DependencyProperty ProcessIdColumnVisibilityProperty = DependencyProperty.Register(
           nameof(ProcessIdColumnVisibility),
           typeof(Visibility),
           typeof(LogsView),
           new PropertyMetadata(Visibility.Collapsed, OnColumnVisibilityChanged));

        public static readonly DependencyProperty ThreadIdColumnVisibilityProperty = DependencyProperty.Register(
           nameof(ThreadIdColumnVisibility),
           typeof(Visibility),
           typeof(LogsView),
           new PropertyMetadata(Visibility.Collapsed, OnColumnVisibilityChanged));

        public static readonly DependencyProperty CallStackColumnVisibilityProperty = DependencyProperty.Register(
           nameof(CallStackColumnVisibility),
           typeof(Visibility),
           typeof(LogsView),
           new PropertyMetadata(Visibility.Collapsed, OnColumnVisibilityChanged));
        
        #endregion collumn visibilities

        #endregion DEPENDENCY PROPERTIES


        #region FIELDS

        private static readonly PropertyInfo[] _controlStringProperties = typeof(IControlsStrings).GetProperties();

        private readonly Dictionary<string, DataGridColumn> _collumns = new Dictionary<string, DataGridColumn>
        {
            { "IdColumn", null },
            { "TimeColumn", null },
            { "SourceColumn", null },
            { "TagColumn", null },
            { "EventTypeColumn", null },
            { "TitleColumn", null },
            { "PayloadColumn", null },
            { "OperationStackColumn", null },
            { "TimeStampColumn", null },
            { "ProcessIdColumn", null },
            { "ThreadIdColumn", null },
            { "CallStackColumn", null }
        };
        
        #endregion FIELDS


        #region CONSTRUCTORS

        static LogsView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LogsView), new FrameworkPropertyMetadata(typeof(LogsView)));
        }

        #endregion CONSTRUCTORS


        #region PROPERTIES

        public IEnumerable<ILogEntry> ItemsSource
        {
            get => (IEnumerable<ILogEntry>)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public IEnumerable<ILogEntry> FilteredItemsSource
        {
            get => (IEnumerable<ILogEntry>)GetValue(FilteredItemsSourceProperty);
            set => SetValue(FilteredItemsSourceProperty, value);
        }

        public IControlsStrings Strings
        {
            get => (IControlsStrings)GetValue(StringsProperty);
            set => SetValue(StringsProperty, value);
        }

        #region column visibilities

        public Visibility IdColumnVisibility
        {
            get => (Visibility)GetValue(IdColumnVisibilityProperty);
            set => SetValue(IdColumnVisibilityProperty, value);
        }

        public Visibility SourceColumnVisibility
        {
            get => (Visibility)GetValue(SourceColumnVisibilityProperty);
            set => SetValue(SourceColumnVisibilityProperty, value);
        }

        public Visibility TimeColumnVisibility
        {
            get => (Visibility)GetValue(TimeColumnVisibilityProperty);
            set => SetValue(TimeColumnVisibilityProperty, value);
        }

        public Visibility TagColumnVisibility
        {
            get => (Visibility)GetValue(TagColumnVisibilityProperty);
            set => SetValue(TagColumnVisibilityProperty, value);
        }

        public Visibility EventTypeColumnVisibility
        {
            get => (Visibility)GetValue(EventTypeColumnVisibilityProperty);
            set => SetValue(EventTypeColumnVisibilityProperty, value);
        }

        public Visibility TitleColumnVisibility
        {
            get => (Visibility)GetValue(TitleColumnVisibilityProperty);
            set => SetValue(TitleColumnVisibilityProperty, value);
        }

        public Visibility PayloadColumnVisibility
        {
            get => (Visibility)GetValue(PayloadColumnVisibilityProperty);
            set => SetValue(PayloadColumnVisibilityProperty, value);
        }

        public Visibility OperationStackColumnVisibility
        {
            get => (Visibility)GetValue(OperationStackColumnVisibilityProperty);
            set => SetValue(OperationStackColumnVisibilityProperty, value);
        }

        public Visibility TimeStampColumnVisibility
        {
            get => (Visibility)GetValue(TimeStampColumnVisibilityProperty);
            set => SetValue(TimeStampColumnVisibilityProperty, value);
        }

        public Visibility ProcessIdColumnVisibility
        {
            get => (Visibility)GetValue(ProcessIdColumnVisibilityProperty);
            set => SetValue(ProcessIdColumnVisibilityProperty, value);
        }

        public Visibility ThreadIdColumnVisibility
        {
            get => (Visibility)GetValue(ThreadIdColumnVisibilityProperty);
            set => SetValue(ThreadIdColumnVisibilityProperty, value);
        }

        public Visibility CallStackColumnVisibility
        {
            get => (Visibility)GetValue(CallStackColumnVisibilityProperty);
            set => SetValue(CallStackColumnVisibilityProperty, value);
        }

        #endregion column visibilities

        #endregion PROPERTIES


        #region METHODS

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            
            foreach (var collumn in _collumns.Keys.ToList())
            {
                _collumns[collumn] = GetTemplateChild(collumn) as DataGridColumn;
                if (_collumns[collumn] == null)
                    throw new InvalidOperationException($"You failed to specify the {collumn} in the template.");
            }

            UpdateColumnVisibilities();
            UpdateColumnNames();
        }

        #region callbacks

        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is LogsView loggingControl))
                return;

            if (e.OldValue is INotifyCollectionChanged oldCollection)
                oldCollection.CollectionChanged -= loggingControl.OnItemsSourceCollectionChanged;
            if (e.NewValue is INotifyCollectionChanged newCollection)
                newCollection.CollectionChanged += loggingControl.OnItemsSourceCollectionChanged;

            loggingControl.FilteredItemsSource = loggingControl.FilterItemsSource();
        }

        private static void OnStringsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is LogsView loggingControl))
                return;

            if (e.OldValue is INotifyPropertyChanged oldStrings)
                oldStrings.PropertyChanged += loggingControl.OnStringsPropertyChanged;
            if (e.NewValue is INotifyPropertyChanged newStrings)
                newStrings.PropertyChanged += loggingControl.OnStringsPropertyChanged;

            loggingControl.UpdateColumnNames();
        }

        #region column visibilities

        private static void OnColumnVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is LogsView logsView))
                return;

            var newValue = e.NewValue as Visibility? ?? Visibility.Collapsed;
            logsView.UpdateColumnVisibility(e.Property.Name, newValue);
        }

        #endregion column visibilities

        #endregion callbacks

        private void OnItemsSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            FilteredItemsSource = FilterItemsSource();
        }

        private void OnStringsPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateColumnName(e.PropertyName);
        }

        public IEnumerable<ILogEntry> FilterItemsSource()
        {
            return ItemsSource;
        }

        public void UpdateColumnName(string propertyName)
        {
            foreach (var collumn in _collumns.Keys.ToList())
            {
                if ($"{propertyName}Column" != collumn)
                    continue;

                foreach (var property in _controlStringProperties)
                    if (Equals(property.Name, propertyName))
                    {
                        _collumns[collumn].Header = property.GetValue(Strings);
                        break;
                    }

                break;
            }
        }
        public void UpdateColumnNames()
        {
            foreach (var collumn in _collumns.Keys.ToList())
                foreach (var property in _controlStringProperties)
                    if (Equals($"{property.Name}Column", collumn))
                    {
                        _collumns[collumn].Header = property.GetValue(Strings);
                        break;
                    }
        }

        private void UpdateColumnVisibility(string propertyName, Visibility visibility)
        {
            var l = "Visibility".Length;
            var columnName = propertyName.Remove(propertyName.Length - l,l);
            _collumns[columnName].Visibility = visibility;
        }
        public void UpdateColumnVisibilities()
        {
            _collumns["IdColumn"].Visibility = IdColumnVisibility;
            _collumns["TimeColumn"].Visibility = TimeColumnVisibility;
            _collumns["SourceColumn"].Visibility = SourceColumnVisibility;
            _collumns["TagColumn"].Visibility = TagColumnVisibility;
            _collumns["EventTypeColumn"].Visibility = EventTypeColumnVisibility;
            _collumns["TitleColumn"].Visibility = TitleColumnVisibility;
            _collumns["PayloadColumn"].Visibility = PayloadColumnVisibility;
            _collumns["OperationStackColumn"].Visibility = OperationStackColumnVisibility;
            _collumns["TimeStampColumn"].Visibility = TimeStampColumnVisibility;
            _collumns["ProcessIdColumn"].Visibility = ProcessIdColumnVisibility;
            _collumns["ThreadIdColumn"].Visibility = ThreadIdColumnVisibility;
            _collumns["CallStackColumn"].Visibility = CallStackColumnVisibility;
        }

        #endregion METHODS
    }
}
