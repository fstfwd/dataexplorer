﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DataExplorer.Application.Filters;
using DataExplorer.Application.ScatterPlots;
using DataExplorer.Application.ScatterPlots.Tasks;
using DataExplorer.Domain.Columns;
using DataExplorer.Domain.Filters;
using DataExplorer.Domain.Rows;
using DataExplorer.Domain.ScatterPlots;
using DataExplorer.Domain.Views;
using DataExplorer.Tests.Domain.Columns;
using Moq;
using NUnit.Framework;

namespace DataExplorer.Tests.Application.ScatterPlots.Tasks
{
    [TestFixture]
    public class UpdatePlotsTaskTests
    {
        private UpdatePlotsTask _task;
        private Mock<IFilterRepository> _mockFilterRepository;
        private Mock<IRowRepository> _mockDataRepository;
        private Mock<IViewRepository> _mockViewRepository;
        private Mock<IScatterPlotRenderer> _mockRenderer;
        private ScatterPlot _scatterPlot;
        private ScatterPlotLayout _layout;
        private List<Filter> _filters;
        private Column _column;
        private List<Row> _rows;
        private List<Plot> _plots;

        [SetUp]
        public void SetUp()
        {
            _column = new ColumnBuilder().Build();
            _filters = new List<Filter>();
            _rows = new List<Row>();
            _plots = new List<Plot>();
            _layout = new ScatterPlotLayout();
            _scatterPlot = new ScatterPlot(new Rect(), _plots, _layout);
            
            _mockFilterRepository = new Mock<IFilterRepository>();
            _mockFilterRepository.Setup(p => p.GetAll()).Returns(_filters);

            _mockDataRepository = new Mock<IRowRepository>();
            _mockDataRepository.Setup(p => p.GetAll()).Returns(_rows);

            _mockViewRepository = new Mock<IViewRepository>();
            _mockViewRepository.Setup(p => p.Get<ScatterPlot>()).Returns(_scatterPlot);

            _mockRenderer = new Mock<IScatterPlotRenderer>();
            _mockRenderer.Setup(p => p.RenderPlots(new List<Row>(), _layout)).Returns(new List<Plot>());
            
            _task = new UpdatePlotsTask(
                _mockFilterRepository.Object,
                _mockDataRepository.Object,
                _mockViewRepository.Object,
                _mockRenderer.Object);
        }

        [Test]
        public void TestUpdatePlotsShouldUpdatePlots()
        {
            _task.UpdatePlots();
            Assert.That(_scatterPlot.GetPlots(), Is.EqualTo(_plots));
        }

        [Test]
        public void TestUpdatePlotsShouldIncludeRowsInsideTheFilter()
        {
            var filter = new BooleanFilter(_column, true);
            _filters.Add(filter);
            var row = new Row(new List<object>() { true });
            _rows.Add(row);
            var plot = new Plot();
            _plots.Add(plot);
            _mockRenderer.Setup(p => p.RenderPlots(_rows, _layout)).Returns(_plots);
            _task.UpdatePlots();
            Assert.That(_scatterPlot.GetPlots().Count, Is.EqualTo(1));
        }

        [Test]
        public void TestUpdatePlotsShouldExcludeRowsOutsideTheFilter()
        {
            var row = new Row(new List<object>() { false });
            _rows.Add(row);
            var filter = new BooleanFilter(_column, true);
            _filters.Add(filter);
            _task.UpdatePlots();
            Assert.That(_scatterPlot.GetPlots().Count, Is.EqualTo(0));
        }
    }
}