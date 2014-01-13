﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataExplorer.Application.Columns;
using DataExplorer.Application.Columns.Queries;
using DataExplorer.Application.Core.Events;
using DataExplorer.Application.Core.Queries;
using DataExplorer.Application.Views.ScatterPlots;
using DataExplorer.Application.Views.ScatterPlots.Events;
using DataExplorer.Domain.Events;
using DataExplorer.Domain.Views.ScatterPlots;
using DataExplorer.Presentation.Core;
using DataExplorer.Presentation.Core.Layout;

namespace DataExplorer.Presentation.Views.ScatterPlots.Layout
{
    public class YAxisLayoutViewModel 
        : BaseViewModel, 
        IYAxisLayoutViewModel,
        IEventHandler<ScatterPlotLayoutChangedEvent>,
        IDomainHandler<ScatterPlotLayoutColumnChangedEvent>
    {
        private readonly IQueryBus _queryBus;
        private readonly IScatterPlotLayoutService _layoutService;
        private List<LayoutItemViewModel> _viewModels; 

        public YAxisLayoutViewModel(
            IQueryBus queryBus,
            IScatterPlotLayoutService layoutService)
        {
            _queryBus = queryBus;
            _layoutService = layoutService;

            _viewModels = new List<LayoutItemViewModel>();
        }

        public string Label
        {
            get { return "y-Axis"; }
        }

        public List<LayoutItemViewModel> Columns
        {
            get { return GetColumnViewModels(); }
        }

        public LayoutItemViewModel SelectedColumn
        {
            get { return GetSelectedColumnViewModel(); }
            set { SetSelectedColumnViewModel(value); }
        }

        private List<LayoutItemViewModel> GetColumnViewModels()
        {
            var columns = _queryBus.Execute(new GetAllColumnsQuery());

            _viewModels = columns
                .Select(p => new LayoutItemViewModel(p))
                .ToList();

            return _viewModels;
        }

        private LayoutItemViewModel GetSelectedColumnViewModel()
        {
            var columnDto = _layoutService.GetYColumn();

            if (columnDto == null)
                return null;

            var viewModel = new LayoutItemViewModel(columnDto);

            return viewModel;
        }

        private void SetSelectedColumnViewModel(LayoutItemViewModel value)
        {
            // TODO: Should this just return or set Y Column to null?
            if (value == null)
                return;

            var column = value.Column;

            _layoutService.SetYColumn(column);
        }

        public void Handle(ScatterPlotLayoutChangedEvent args)
        {
            OnPropertyChanged(() => Columns);
            OnPropertyChanged(() => SelectedColumn);
        }

        public void Handle(ScatterPlotLayoutColumnChangedEvent args)
        {
            OnPropertyChanged(() => SelectedColumn);
        }
    }
}