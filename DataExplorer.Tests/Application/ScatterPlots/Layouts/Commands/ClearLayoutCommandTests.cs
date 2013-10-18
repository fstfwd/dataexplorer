﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DataExplorer.Application.ScatterPlots.Layouts.Commands;
using DataExplorer.Domain.Columns;
using DataExplorer.Domain.Events;
using DataExplorer.Domain.ScatterPlots;
using DataExplorer.Domain.Views;
using DataExplorer.Tests.Domain.Columns;
using Moq;
using NUnit.Framework;

namespace DataExplorer.Tests.Application.ScatterPlots.Layouts.Commands
{
    [TestFixture]
    public class ClearLayoutCommandTests
    {
        private ClearLayoutCommand _command;
        private Mock<IViewRepository> _mockRepository;
        private ScatterPlot _scatterPlot;
        private ScatterPlotLayout _layout;

        [SetUp]
        public void SetUp()
        {
            var column = new ColumnBuilder().Build();
            _layout = new ScatterPlotLayout()
            {
                XAxisColumn = column,
                YAxisColumn = column
            };
            _scatterPlot = new ScatterPlot(new Rect(), null, _layout);
            
            _mockRepository = new Mock<IViewRepository>();
            _mockRepository.Setup(p => p.Get<ScatterPlot>()).Returns(_scatterPlot);

            _command = new ClearLayoutCommand(_mockRepository.Object);
        }

        [Test]
        public void TestExecuteShouldClearXAxis()
        {
            _command.Execute();
            Assert.That(_layout.XAxisColumn, Is.Null);
        }

        [Test]
        public void TestExecuteShouldClearYAxis()
        {
            _command.Execute();
            Assert.That(_layout.YAxisColumn, Is.Null);
        }

        [Test]
        public void TestExecuteShouldRaiseLayoutChangedEvent()
        {
            var wasRaised = false;
            DomainEvents.Register<ScatterPlotLayoutColumnChangedEvent>(p => { wasRaised = true; });
            _command.Execute();
            Assert.That(wasRaised, Is.True);
            DomainEvents.ClearHandlers();
        }
    }
}
