using MyCSharpLib.Services.Logging;
using MyCSharpLib.Services.Serialization.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace MyCSharpLib.Wpf.Controls
{
    [TemplatePart(Name = nameof(PartIdColumn), Type = typeof(DataGridColumn))]
    [TemplatePart(Name = nameof(PartTimeColumn), Type = typeof(DataGridColumn))]
    [TemplatePart(Name = nameof(PartSourceColumn), Type = typeof(DataGridColumn))]
    [TemplatePart(Name = nameof(PartTagColumn), Type = typeof(DataGridColumn))]
    [TemplatePart(Name = nameof(PartEventTypeColumn), Type = typeof(DataGridColumn))]
    [TemplatePart(Name = nameof(PartTitleColumn), Type = typeof(DataGridColumn))]
    [TemplatePart(Name = nameof(PartPayloadColumn), Type = typeof(DataGridColumn))]
    [TemplatePart(Name = nameof(PartProcessIdColumn), Type = typeof(DataGridColumn))]
    [TemplatePart(Name = nameof(PartThreadIdColumn), Type = typeof(DataGridColumn))]
    [TemplatePart(Name = nameof(PartCallStackColumn), Type = typeof(DataGridColumn))]
    [TemplatePart(Name = nameof(PartSearchButton), Type = typeof(Button))]
    public class LogsView : AControl
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
            typeof(LogsView));

        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(
            nameof(SelectedItem),
            typeof(ILogEntry),
            typeof(LogsView));

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

        #region filter

        public static readonly DependencyProperty IdFilterProperty = DependencyProperty.Register(
            nameof(IdFilter),
            typeof(string),
            typeof(LogsView));

        public static readonly DependencyProperty SourceFilterProperty = DependencyProperty.Register(
            nameof(SourceFilter),
            typeof(string),
            typeof(LogsView));

        public static readonly DependencyProperty TagFilterProperty = DependencyProperty.Register(
            nameof(TagFilter),
            typeof(string),
            typeof(LogsView));

        public static readonly DependencyProperty EventTypeFilterProperty = DependencyProperty.Register(
            nameof(EventTypeFilter),
            typeof(TraceEventType),
            typeof(LogsView),
            new PropertyMetadata(NoEventTypeFilter));

        public static readonly DependencyProperty TitleFilterProperty = DependencyProperty.Register(
            nameof(TitleFilter),
            typeof(string),
            typeof(LogsView));

        public static readonly DependencyProperty PayloadFilterProperty = DependencyProperty.Register(
            nameof(PayloadFilter),
            typeof(string),
            typeof(LogsView));

        public static readonly DependencyProperty OperationStackFilterProperty = DependencyProperty.Register(
            nameof(OperationStackFilter),
            typeof(string),
            typeof(LogsView));

        public static readonly DependencyProperty TimeLowerLimitProperty = DependencyProperty.Register(
            nameof(TimeLowerLimit),
            typeof(DateTime?),
            typeof(LogsView));

        public static readonly DependencyProperty TimeUpperLimitProperty = DependencyProperty.Register(
            nameof(TimeUpperLimit),
            typeof(DateTime?),
            typeof(LogsView));

        public static readonly DependencyProperty TimeStampLowerLimitProperty = DependencyProperty.Register(
            nameof(TimeStampLowerLimit),
            typeof(int?),
            typeof(LogsView));

        public static readonly DependencyProperty TimeStampUpperLimitProperty = DependencyProperty.Register(
            nameof(TimeStampUpperLimit),
            typeof(int?),
            typeof(LogsView));

        public static readonly DependencyProperty ProcessIdFilterProperty = DependencyProperty.Register(
            nameof(ProcessIdFilter),
            typeof(int?),
            typeof(LogsView));

        public static readonly DependencyProperty ThreadIdFilterProperty = DependencyProperty.Register(
            nameof(ThreadIdFilter),
            typeof(string),
            typeof(LogsView));

        public static readonly DependencyProperty CallStackFilterProperty = DependencyProperty.Register(
            nameof(CallStackFilter),
            typeof(string),
            typeof(LogsView));

        #endregion filter

        #endregion DEPENDENCY PROPERTIES


        #region FIELDS

#pragma warning disable RECS0016 // Bitwise operation on enum which has no [Flags] attribute
        private const TraceEventType NoEventTypeFilter =
            TraceEventType.Critical |
            TraceEventType.Error |
            TraceEventType.Warning |
            TraceEventType.Information |
            TraceEventType.Verbose |
            TraceEventType.Start |
            TraceEventType.Stop |
            TraceEventType.Suspend |
            TraceEventType.Resume |
            TraceEventType.Transfer;
#pragma warning restore RECS0016 // Bitwise operation on enum which has no [Flags] attribute

        private const string PartIdColumn = "IdColumn";
        private const string PartTimeColumn = "TimeColumn";
        private const string PartSourceColumn = "SourceColumn";
        private const string PartTagColumn = "TagColumn";
        private const string PartEventTypeColumn = "EventTypeColumn";
        private const string PartTitleColumn = "TitleColumn";
        private const string PartPayloadColumn = "PayloadColumn";
        private const string PartOperationStackColumn = "OperationStackColumn";
        private const string PartTimeStampColumn = "TimeStampColumn";
        private const string PartProcessIdColumn = "ProcessIdColumn";
        private const string PartThreadIdColumn = "ThreadIdColumn";
        private const string PartCallStackColumn = "CallStackColumn";
        private const string PartSearchButton = "SearchButton";

        private static readonly PropertyInfo[] _controlStringProperties = typeof(IControlsStrings).GetProperties();

        private Button _searchButton;
        private readonly Dictionary<string, DataGridColumn> _collumns = new Dictionary<string, DataGridColumn>
        {
            { PartIdColumn, null },
            { PartTimeColumn, null },
            { PartSourceColumn, null },
            { PartTagColumn, null },
            { PartEventTypeColumn, null },
            { PartTitleColumn, null },
            { PartPayloadColumn, null },
            { PartOperationStackColumn, null },
            { PartTimeStampColumn, null },
            { PartProcessIdColumn, null },
            { PartThreadIdColumn, null },
            { PartCallStackColumn, null }
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

        public ILogEntry SelectedItem
        {
            get => (ILogEntry)GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
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

        #region filter

        public string IdFilter
        {
            get => (string)GetValue(IdFilterProperty);
            set => SetValue(IdFilterProperty, value);
        }

        public string SourceFilter
        {
            get => (string)GetValue(SourceFilterProperty);
            set => SetValue(SourceFilterProperty, value);
        }

        public string TagFilter
        {
            get => (string)GetValue(TagFilterProperty);
            set => SetValue(TagFilterProperty, value);
        }

        public TraceEventType EventTypeFilter
        {
            get => (TraceEventType)GetValue(EventTypeFilterProperty);
            set => SetValue(EventTypeFilterProperty, value);
        }

        public string TitleFilter
        {
            get => (string)GetValue(TitleFilterProperty);
            set => SetValue(TitleFilterProperty, value);
        }

        public string PayloadFilter
        {
            get => (string)GetValue(PayloadFilterProperty);
            set => SetValue(PayloadFilterProperty, value);
        }

        public string OperationStackFilter
        {
            get => (string)GetValue(OperationStackFilterProperty);
            set => SetValue(OperationStackFilterProperty, value);
        }

        public DateTime? TimeLowerLimit
        {
            get => (DateTime?)GetValue(TimeLowerLimitProperty);
            set => SetValue(TimeLowerLimitProperty, value);
        }

        public DateTime? TimeUpperLimit
        {
            get => (DateTime?)GetValue(TimeUpperLimitProperty);
            set => SetValue(TimeUpperLimitProperty, value);
        }

        public int? TimeStampLowerLimit
        {
            get => (int?)GetValue(TimeStampLowerLimitProperty);
            set => SetValue(TimeStampLowerLimitProperty, value);
        }

        public int? TimeStampUpperLimit
        {
            get => (int?)GetValue(TimeStampUpperLimitProperty);
            set => SetValue(TimeStampUpperLimitProperty, value);
        }

        public int? ProcessIdFilter
        {
            get => (int?)GetValue(ProcessIdFilterProperty);
            set => SetValue(ProcessIdFilterProperty, value);
        }

        public string ThreadIdFilter
        {
            get => (string)GetValue(ThreadIdFilterProperty);
            set => SetValue(ThreadIdFilterProperty, value);
        }

        public string CallStackFilter
        {
            get => (string)GetValue(CallStackFilterProperty);
            set => SetValue(CallStackFilterProperty, value);
        }

        #endregion filter

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

            _searchButton = GetTemplateChild(PartSearchButton) as Button;
            if (_searchButton == null)
                throw new InvalidOperationException($"You failed to specify the {PartSearchButton} in the template.");
            _searchButton.Click += OnSearchButtonClick;


            UpdateColumnVisibilities();
            UpdateColumnNames();
        }

        #region callbacks

        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is LogsView logsView))
                return;

            if (e.OldValue is INotifyCollectionChanged oldCollection)
                oldCollection.CollectionChanged -= logsView.OnItemsSourceCollectionChanged;
            if (e.NewValue is INotifyCollectionChanged newCollection)
                newCollection.CollectionChanged += logsView.OnItemsSourceCollectionChanged;

            logsView.UpdateFilteredItemsSource();
        }
        
        private static void OnColumnVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is LogsView logsView))
                return;

            var newValue = e.NewValue as Visibility? ?? Visibility.Collapsed;
            logsView.UpdateColumnVisibility(e.Property.Name, newValue);
        }
        
        #endregion callbacks

        private void OnItemsSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) => UpdateFilteredItemsSource();
        protected override void OnStringsPropertyChanged(object sender, PropertyChangedEventArgs e) => UpdateColumnName(e.PropertyName);
        private void OnSearchButtonClick(object sender, RoutedEventArgs e) => UpdateFilteredItemsSource();

        public void UpdateFilteredItemsSource()
        {
            var itemsSource = ItemsSource;

            if (!string.IsNullOrWhiteSpace(IdFilter))
                itemsSource = itemsSource.Where(x => x.Id.ToString().Contains(IdFilter));

            if (!string.IsNullOrWhiteSpace(SourceFilter))
                itemsSource = itemsSource.Where(x => x.Source?.Contains(SourceFilter) == true);

            if (!string.IsNullOrWhiteSpace(TagFilter))
                itemsSource = itemsSource.Where(x => x.Tag?.Contains(TagFilter) == true);

            if (EventTypeFilter != NoEventTypeFilter)
                itemsSource = itemsSource.Where(x => ShouldShowEvent(x.EventType));

            if (!string.IsNullOrWhiteSpace(TitleFilter))
                itemsSource = itemsSource.Where(x => x.Title?.Contains(TitleFilter) == true);

            if (!string.IsNullOrWhiteSpace(PayloadFilter))
                itemsSource = itemsSource.Where(x => x.Payload?.Any(p => p.SerializeJson().Contains(PayloadFilter)) == true);

            if (!string.IsNullOrWhiteSpace(OperationStackFilter))
                itemsSource = itemsSource.Where(x => x.EventCache?.LogicalOperationStack?.Cast<object>().Any(s => s.SerializeJson().Contains(PayloadFilter)) == true);

            if (TimeStampLowerLimit != null && TimeStampUpperLimit != null)
                itemsSource = itemsSource.Where(x => x.EventCache?.Timestamp >= TimeStampLowerLimit && x.EventCache?.Timestamp <= TimeStampUpperLimit);
            else if (TimeStampLowerLimit != null)
                itemsSource = itemsSource.Where(x => x.EventCache?.Timestamp >= TimeStampLowerLimit);
            else if (TimeStampUpperLimit != null)
                itemsSource = itemsSource.Where(x => x.EventCache?.Timestamp <= TimeStampUpperLimit);

            if (ProcessIdFilter != null)
                itemsSource = itemsSource.Where(x => x.EventCache?.ProcessId == ProcessIdFilter);

            if (!string.IsNullOrWhiteSpace(ThreadIdFilter))
                itemsSource = itemsSource.Where(x => x.EventCache?.ThreadId == ThreadIdFilter);

            if (!string.IsNullOrWhiteSpace(CallStackFilter))
                itemsSource = itemsSource.Where(x => x.EventCache?.Callstack?.Contains(CallStackFilter) == true);

            FilteredItemsSource = itemsSource.ToList();
        }

#pragma warning disable RECS0016 // Bitwise operation on enum which has no [Flags] attribute
        private bool ShouldShowEvent(TraceEventType eventType) => (EventTypeFilter & eventType) > 0;
#pragma warning restore RECS0016 // Bitwise operation on enum which has no [Flags] attribute

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
            _collumns[PartIdColumn].Visibility = IdColumnVisibility;
            _collumns[PartTimeColumn].Visibility = TimeColumnVisibility;
            _collumns[PartSourceColumn].Visibility = SourceColumnVisibility;
            _collumns[PartTagColumn].Visibility = TagColumnVisibility;
            _collumns[PartEventTypeColumn].Visibility = EventTypeColumnVisibility;
            _collumns[PartTitleColumn].Visibility = TitleColumnVisibility;
            _collumns[PartPayloadColumn].Visibility = PayloadColumnVisibility;
            _collumns[PartOperationStackColumn].Visibility = OperationStackColumnVisibility;
            _collumns[PartTimeStampColumn].Visibility = TimeStampColumnVisibility;
            _collumns[PartProcessIdColumn].Visibility = ProcessIdColumnVisibility;
            _collumns[PartThreadIdColumn].Visibility = ThreadIdColumnVisibility;
            _collumns[PartCallStackColumn].Visibility = CallStackColumnVisibility;
        }

        #endregion METHODS
    }
}
