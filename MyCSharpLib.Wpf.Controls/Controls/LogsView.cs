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

        public static readonly DependencyProperty IdColumnVisibilityProperty = DependencyProperty.Register(
            nameof(IdColumnVisibility),
            typeof(Visibility),
            typeof(LogsView),
            new PropertyMetadata(Visibility.Collapsed));

        public static readonly DependencyProperty SourceColumnVisibilityProperty = DependencyProperty.Register(
            nameof(SourceColumnVisibility),
            typeof(Visibility),
            typeof(LogsView),
            new PropertyMetadata(Visibility.Visible));

        public static readonly DependencyProperty TimeColumnVisibilityProperty = DependencyProperty.Register(
            nameof(TimeColumnVisibility),
            typeof(Visibility),
            typeof(LogsView),
            new PropertyMetadata(Visibility.Visible));

        public static readonly DependencyProperty TagColumnVisibilityProperty = DependencyProperty.Register(
            nameof(TagColumnVisibility),
            typeof(Visibility),
            typeof(LogsView),
            new PropertyMetadata(Visibility.Visible));

        public static readonly DependencyProperty EventTypeColumnVisibilityProperty = DependencyProperty.Register(
            nameof(EventTypeColumnVisibility),
            typeof(Visibility),
            typeof(LogsView),
            new PropertyMetadata(Visibility.Visible));
        
        public static readonly DependencyProperty TitleColumnVisibilityProperty = DependencyProperty.Register(
            nameof(TitleColumnVisibility),
            typeof(Visibility),
            typeof(LogsView),
            new PropertyMetadata(Visibility.Visible));
        
        public static readonly DependencyProperty PayloadColumnVisibilityProperty = DependencyProperty.Register(
            nameof(PayloadColumnVisibility),
            typeof(Visibility),
            typeof(LogsView),
            new PropertyMetadata(Visibility.Visible));

        #endregion DEPENDENCY PROPERTIES


        #region FIELDS

        private static readonly PropertyInfo[] _controlStringProperties = typeof(IControlsStrings).GetProperties();

        private readonly Dictionary<string, DataGridColumn> _collumns = new Dictionary<string, DataGridColumn>
        {
            { "IdCollumn", null },
            { "TimeCollumn", null },
            { "SourceCollumn", null },
            { "TagCollumn", null },
            { "EventTypeCollumn", null },
            { "TitleCollumn", null },
            { "PayloadCollumn", null },
            { "OperationStackCollumn", null },
            { "TimeStampCollumn", null },
            { "ProcessIdCollumn", null },
            { "ThreadIdCollumn", null },
            { "CallStackCollumn", null }
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

        #endregion callbacks

        private void OnItemsSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            FilteredItemsSource = FilterItemsSource();
        }

        private void OnStringsPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateCollumnName(e.PropertyName);
        }

        public IEnumerable<ILogEntry> FilterItemsSource()
        {
            return ItemsSource;
        }

        public void UpdateCollumnName(string propertyName)
        {
            foreach (var collumn in _collumns.Keys.ToList())
            {
                if ($"{propertyName}Collumn" != collumn)
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
                    if (Equals($"{property.Name}Collumn", collumn))
                    {
                        _collumns[collumn].Header = property.GetValue(Strings);
                        break;
                    }
        }

        #endregion METHODS
    }
}
